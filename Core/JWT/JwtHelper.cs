using Core.Base.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.JWT;


public static class JwtHelper
{
    private static readonly SecurityTokenConfig Config = new();

    public static string CreateJwtAccessToken(long userId, string username) =>
        CreateJwt(userId, username, Config.AccessTokenSecretKey, Config.AccessTokenLifetime);

    public static string CreateJwtRefreshToken(long userId, string username) =>
        CreateJwt(userId, username, Config.RefreshTokenSecretKey, Config.RefreshTokenLifetime);

    private static string CreateJwt(long userId, string username, string key, TimeSpan lifetime, IEnumerable<Claim>? extraClaims = null)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username.ToLowerInvariant()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (extraClaims != null)
            claims.AddRange(extraClaims);

        var token = new JwtSecurityToken(
            issuer: Config.Issuer,
            audience: Config.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(lifetime),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static bool Validate(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (jwtToken == null)
                return false;

            var payload = jwtToken.Payload;

            if (payload.Iss != Config.Issuer)
                return false;

            if (!payload.Aud.Contains(Config.Audience))
                return false;

            if (payload.ValidTo < DateTime.UtcNow)
                return false;

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static JwtPayload? GetPayload(string token)
    {
        if (string.IsNullOrEmpty(token) || !Validate(token))
            return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken.Payload;
    }

    public static string GetUsername(string token)
    {
        var payload = GetPayload(token);
        return payload?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
    }
}