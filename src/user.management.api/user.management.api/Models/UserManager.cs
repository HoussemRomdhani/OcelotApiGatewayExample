using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace user.management.api.Models
{
    public static class UserManager
    {
        private static IEnumerable<User> _users = new List<User> {
           new User(1, "houssem.romdhani", "romdhanihoussem77@gmail.com", new List<string> { "PRODUCTS_ADMIN", "PRODUCTS_TEST"}),
           new User(2, "samir.romdhani", "samir.romdhani@gmail.com", new List<string> { "PRODUCTS_GUEST", "PRODUCTS_TEST"})
    };

        public static User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public static dynamic Login(string name)
        {
            var user = _users.FirstOrDefault(x => x.Name == name);
            if (user == null)
                return string.Empty;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var now = DateTime.UtcNow;

                var claims = new List<Claim>
                {
                    new Claim("name", user.Name),
                    new Claim("userId", user.Id.ToString())
                    //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                };

                var audiances = new List<Claim>
               {
                    new Claim(JwtRegisteredClaimNames.Aud, "user.management"),
                    new Claim(JwtRegisteredClaimNames.Aud,  "products")
               };

                claims.AddRange(audiances);
               foreach (var role in user.Roles)
                claims.Add(new Claim("Permission", role));

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("THIS_IS_A_TOP_SECRET_KEY"));
                //var tokenValidationParameters = new TokenValidationParameters
                //{
                //    ValidateIssuerSigningKey = true,
                //    IssuerSigningKey = signingKey,
                //    ValidateIssuer = true,
                //    ValidIssuer = "api.factory",
                //    ValidateAudience = true,
                //    ValidAudiences = new string[] { "user.management", "products" },
                //    ValidateLifetime = true,
                //    ClockSkew = TimeSpan.Zero,
                //    RequireExpirationTime = true,
                //};

                var jwt = new JwtSecurityToken(
                    issuer: "api.factory",
                    null,
                  //audience: new string[] { "user.management", "products" },
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(20)),
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
    }
}
