using Domain.Commons;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Either<Error, UserEntity>> Authenticate(string identity, string password, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting service to search for user");

                var user = await _userRepository.FindUser(identity, Cryptography.Encrypt(password), cancellationToken);

                if (user is null)
                {
                    _logger.LogWarning("User not found");
                    return Either<Error, UserEntity>.LeftValue(new Error("Not found"));
                }

                return Either<Error, UserEntity>.RightValue(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while searching for the user");
                return Either<Error, UserEntity>.LeftValue(new Error("An error occurred while searching for the user"));
            }
        }
    }
}
