using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration e_config;

        private readonly SymmetricSecurityKey e_key;
        public TokenService(IConfiguration config )
        {
            e_config = config;
            e_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(e_config["Token:Key"]));
        }
        public string CreateToken(AppUser user)
        {
            var cliams = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.DisplayName)
            };
            var creds = new SigningCredentials(e_key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(cliams),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = e_config["Token:Issuer"]
            };

            var tokenHandler  = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}