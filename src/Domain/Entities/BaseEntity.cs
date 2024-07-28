namespace Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; private set; } = Guid.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime UpdatedAt { get; protected set; }
    }
}
