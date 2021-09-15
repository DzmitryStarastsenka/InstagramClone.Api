using InstagramClone.Domain.Constants.JwtConstants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InstagramClone.Domain.Jwt
{
    public static class JWTHandler
    {
        public static void ConfigureJwtBearerOptions(JwtBearerOptions options, IConfigurationSection configurationSection)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configurationSection.GetValue<string>("privateKey")));

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretKey,
                NameClaimType = JwtClaimsNamesConstants.UserName,
                ValidateIssuer = true,
                ValidIssuer = configurationSection.GetValue<string>("issuer"),
                ValidateAudience = true,
                ValidAudience = configurationSection.GetValue<string>("audience"),
                ValidateLifetime = true,
            };
        }
    }
}