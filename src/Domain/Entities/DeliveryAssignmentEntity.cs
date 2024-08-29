namespace Domain.Entities
{
    public class DeliveryAssignmentEntity : BaseEntity
    {
        public DeliveryAssignmentEntity(Guid orderId, Guid courierId)
        {
            CourierId = courierId;
            OrderId = orderId;
            AssignedAt = DateTime.Now;
        }

        public Guid CourierId { get; private set; }
        public Guid OrderId { get; private set; }
        public DateTime AssignedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public OrderEntity Order { get; set; }

        public void CompleteDelivery()
        {
            CompletedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
