namespace Domain.Entities
{
    public class RentalClosureEntity : BaseEntity
    {
        const decimal ADDITIONAL_DAILY_CHARGE = 50.0m;

        private RentalClosureEntity() { }

        public RentalClosureEntity(RentEntity rent, DateTime endDate)
        {
            Rent = rent;
            RentId = rent.Id;
            EndDate = endDate.Date;

            Calculate();
        }

        public Guid RentId { get; private set; }
        public DateTime EndDate { get; private set; }
        public decimal PenaltyAmountForUnusedDay { get; private set; }
        public decimal CostForUsedDays { get; private set; }
        public decimal TotalPayment { get; private set; }
        public decimal TotalAdditionalDailyAmount { get; private set; }
        public int ExceededDays { get; private set; }
        public RentEntity Rent { get; private set; }

        public void Calculate()
        {
            CalculateExceededDaysInDays();
            CalculateCostForUsedDaysInplan();
            CalculateTotalAdditionalDailyAmount();
            CalculatePenaltyAmountForUnusedDay();
            CalculateTotalPayment();
        }

        public void UpdateCalculatedValues(DateTime endDate)
        {
            EndDate = endDate.Date;
            Calculate();
        }

        private DateTime CalculatePlannedEndDate()
        {
            return Rent.StartDate.AddDays(Rent.Plan.Days - 1);
        }

        private void CalculateExceededDaysInDays()
        {
            if (EndDate <= CalculatePlannedEndDate())
            {
                ExceededDays = 0;
                return;
            }

            ExceededDays = EndDate.Subtract(CalculatePlannedEndDate()).Days;
        }

        private void CalculateTotalPayment()
        {
            TotalPayment = CostForUsedDays + PenaltyAmountForUnusedDay + TotalAdditionalDailyAmount;
        }

        private void CalculateTotalAdditionalDailyAmount()
        {
            TotalAdditionalDailyAmount = 0;

            if (ExceededDays > 0)
            {
                TotalAdditionalDailyAmount = ExceededDays * ADDITIONAL_DAILY_CHARGE;
            }
        }

        private void CalculateCostForUsedDaysInplan()
        {
            var usedDaysInplan = CalculateUsedDaysWithinPlan();

            CostForUsedDays = usedDaysInplan * Rent.Plan.DailyRate;
        }

        private void CalculatePenaltyAmountForUnusedDay()
        {
            PenaltyAmountForUnusedDay = 0;

            if (ExceededDays > 0)
            {
                return;
            }

            if (Rent.Plan.DailyFineRate == 0)
            {
                return;
            }

            var calculatedUnusedDay = CalculatedUnusedDay();

            if (calculatedUnusedDay == 0)
            {
                return;
            }

            PenaltyAmountForUnusedDay = calculatedUnusedDay * (Rent.Plan.DailyRate * (Rent.Plan.DailyFineRate / 100)) + calculatedUnusedDay * Rent.Plan.DailyRate;
        }

        private int CalculatedUnusedDay()
        {
            if (EndDate >= CalculatePlannedEndDate())
            {
                return 0;
            }

            return CalculatePlannedEndDate().Subtract(EndDate).Days;
        }

        private int CalculateUsedDaysWithinPlan()
        {
            if (EndDate >= CalculatePlannedEndDate())
            {
                return Rent.Plan.Days;
            }

            return Rent.Plan.Days - CalculatePlannedEndDate().Subtract(EndDate).Days;
        }
    }
}
