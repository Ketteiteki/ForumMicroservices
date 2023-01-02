using System.IdentityModel.Tokens.Jwt;
using AuthorizationApi.Domain.Entities;

namespace AuthorizationApi.Application.Interfaces;

public interface IJwtTokenService
{
    public string CreateAccessToken(UserEntity user, string signKey);

    public bool TryValidateAccessToken(string token, string signKey, out JwtSecurityToken validatedJwtToken);
}