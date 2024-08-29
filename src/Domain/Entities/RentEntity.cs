using Domain.Enums;

namespace Domain.Entities
{
    public class RentEntity : BaseEntity
    {
        private RentEntity() { }

        public RentEntity(Guid courierId, Guid motorcycleId, PlanEntity plan, DateTime startDate, DateTime expectedEndDate)
        {
            CourierId = courierId;
            StartDate = startDate.AddDays(1);
            ExpectedEndDate = expectedEndDate;
            Plan = plan;
            PlanId = plan.Id;
            MotorcycleId = motorcycleId;
            Status = ERentStatus.InProgress;

            RentalClosure = new RentalClosureEntity(this, expectedEndDate);
            TotalPayment = RentalClosure.TotalPayment;
        }

        public Guid CourierId { get; private set; }
        public Guid PlanId { get; private set; }
        public Guid MotorcycleId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime ExpectedEndDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public ERentStatus Status { get; private set; }
        public decimal TotalPayment { get; private set; }
        public PlanEntity Plan { get; private set; }
        public RentalClosureEntity RentalClosure { get; private set; }

        public void CloseRental(DateTime endDate)
        {
            EndDate = endDate;
            Status = ERentStatus.Completed;
            RentalClosure.UpdateCalculatedValues(endDate);
            TotalPayment = RentalClosure.TotalPayment;
        }
    }
}
