using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthorizationApi.Application.Interfaces;
using AuthorizationApi.Domain.Constants;
using AuthorizationApi.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationApi.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    public string CreateAccessToken(UserEntity user, string signKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.Default.GetBytes(signKey);

        var claims = new[]
        {
            new Claim(ClaimConstants.Id, user.Id.ToString()),
            new Claim(ClaimConstants.Nickname, user.Nickname),
        };

        var jwtToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256));

        return tokenHandler.WriteToken(jwtToken);
    }

    public bool TryValidateAccessToken(string token, string signKey, out JwtSecurityToken validatedJwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.Default.GetBytes(signKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
            }, out var validatedToken);

            validatedJwtToken = (JwtSecurityToken)validatedToken;

            return true;
        }
        catch
        {
            validatedJwtToken = new JwtSecurityToken();

            return false;
        }
    }
}