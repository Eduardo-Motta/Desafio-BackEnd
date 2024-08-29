using Domain.Commons;
using Domain.Enums;

namespace Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public UserEntity(Guid id, string identity, string password, EUserRole role)
        {
            Id = id;
            Identity = identity;
            Password = Cryptography.Encrypt(password);
            Role = role;
        }

        public string Identity { get; private set; }
        public string Password { get; private set; }
        public EUserRole Role { get; private set; }
    }
}
