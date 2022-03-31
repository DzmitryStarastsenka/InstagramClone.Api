using InstagramClone.Domain.Constants.JwtConstants;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InstagramClone.Domain.Jwt
{
    public class JwtTokenGenerator
    {
        protected readonly IConfiguration Configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected string GenerateTokenInner(string userName, string fullName, string expiresInName)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimsNamesConstants.UserName, userName),
                new Claim(JwtClaimsNamesConstants.FullName, fullName)
            };

            var jwt = GenerateJwtSecurityToken(claims, expiresInName);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        protected JwtSecurityToken GenerateJwtSecurityToken(IList<Claim> claims, string expiresInName)
        {
            var now = DateTime.UtcNow;

            return new JwtSecurityToken(
                issuer: GetConfigValue("issuer"),
                audience: GetConfigValue("audience"),
                notBefore: now,
                claims: claims,
                expires: GetExpiresIn(now, expiresInName),
                signingCredentials:
                    new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature));
        }

        public bool ValidateToken(string token)
        {
            try
            {
                new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = GetConfigValue("audience"),
                    ValidateIssuer = true,
                    ValidIssuer = GetConfigValue("issuer"),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = GetSymmetricSecurityKey()
                },
                    out _);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool ValidateJwtTokenWithoutLifetime(string token)
        {
            try
            {
                new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
                {
                    ValidateLifetime = false,
                    ValidateAudience = true,
                    ValidAudience = GetConfigValue("audience"),
                    ValidateIssuer = true,
                    ValidIssuer = GetConfigValue("issuer"),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = GetSymmetricSecurityKey()
                },
                   out _);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool ValidateLifeTime(string token)
        {
            try
            {
                new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = GetSymmetricSecurityKey()
                },
                    out _);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static JwtSecurityToken ReadJwtToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }

        protected virtual DateTime GetExpiresIn(DateTime now, string expiresInName)
        {
            return now.AddDays(int.Parse(GetConfigValue(expiresInName)));
        }

        protected string GetConfigValue(string valueKey)
        {
            return Configuration.GetValue<string>($"JwtToken:{valueKey}");
        }

        private SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetConfigValue("privateKey")));
        }
    }
}