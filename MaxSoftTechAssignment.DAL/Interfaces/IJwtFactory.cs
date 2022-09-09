using System.Security.Claims;

namespace MaxSoftTechAssignment.DAL.Interfaces;

public interface IJwtFactory
{
    /// <summary>
    /// Creates Jwt token
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="email"></param>
    /// <param name="additionalClaims"></param>
    /// <returns></returns>
    Task<(string token, DateTime expiration)> CreateTokenAsync(string userId, string email,
        IEnumerable<Claim> additionalClaims = default);

}