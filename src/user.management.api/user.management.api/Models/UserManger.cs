using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace user.management.api.Models
{
    public static class UserManger
    {
        private static IEnumerable<User> _users = new List<User> {
           new User(1, "houssem.romdhani", "romdhanihoussem77@gmail.com"),
           new User(2, "samir.romdhani", "samir.romdhani@gmail.com")
    };

        public static User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public static dynamic Login(string name, string password)
        {
            if (name == "houssem.romdhani" && password == "password")
            {
                var now = DateTime.UtcNow;

                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                };

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("THIS_IS_A_TOP_SECRET_KEY"));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = "api.factory",
                    ValidateAudience = true,
                    ValidAudience = "allApps",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };

                var jwt = new JwtSecurityToken(
                    issuer: "api.factory",
                    audience: "allApps",
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var responseJson = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)TimeSpan.FromMinutes(2000).TotalSeconds
                };

                return responseJson;
            }
            else
                return string.Empty;
        }
    }
}
