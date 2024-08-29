using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateUser(UserEntity entity, CancellationToken cancellationToken)
        {
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserEntity?> FindUser(string username, string password, CancellationToken cancellationToken)
        {
            return await _context.Users.Where(x => x.Identity == username && x.Password == password).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
