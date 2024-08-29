using System.Security.Claims;

namespace RentApi.Extensions
{
    public static class UserExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        public static string GetUserIdentity(this ClaimsPrincipal user)
        {
            var userIdentityClaim = user.Claims.FirstOrDefault(x => x.Type == "identity")?.Value;

            return userIdentityClaim ?? string.Empty;
        }
    }
}
