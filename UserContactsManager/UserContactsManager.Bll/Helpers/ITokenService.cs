using System.Security.Claims;

using UserContactsManager.Bll.Dtos;

namespace UserContacts.Bll.Helpers;

public interface ITokenService
{
    public string GenerateToken(UserGetDto user);
    public string GenerateRefreshToken();
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}






