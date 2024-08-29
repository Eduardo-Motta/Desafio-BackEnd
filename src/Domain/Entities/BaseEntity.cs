namespace Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; protected set; }
    }
}
