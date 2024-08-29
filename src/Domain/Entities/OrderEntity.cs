using Domain.Enums;

namespace Domain.Entities
{
    public class OrderEntity : BaseEntity
    {
        private OrderEntity() { }

        public OrderEntity(EOrderDeliveryStatus deliveryStatus)
        {
            DeliveryStatus = EOrderDeliveryStatus.Pending;
        }

        public EOrderDeliveryStatus DeliveryStatus { get; set; }
        public DeliveryAssignmentEntity? DeliveryAssignment { get; set; }

        public void StartDelivery(Guid courierId)
        {
            DeliveryStatus = EOrderDeliveryStatus.InProgress;
            DeliveryAssignment = new DeliveryAssignmentEntity(Id, courierId);
        }

        public void CompletedDelivery()
        {
            if (DeliveryAssignment is null)
            {
                throw new InvalidOperationException("Delivery assignment not created");
            }

            DeliveryStatus = EOrderDeliveryStatus.Completed;
            DeliveryAssignment.CompleteDelivery();
        }
    }
}
