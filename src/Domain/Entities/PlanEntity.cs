namespace Domain.Entities
{
    public class PlanEntity : BaseEntity
    {
        public PlanEntity(int days, decimal dailyRate, decimal dailyFineRate)
        {
            Days = days;
            DailyRate = dailyRate;
            DailyFineRate = dailyFineRate;
        }

        public int Days { get; private set; }
        public decimal DailyRate { get; private set; }
        public decimal DailyFineRate { get; private set; }
    }
}
