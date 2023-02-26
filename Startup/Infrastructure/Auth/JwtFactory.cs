using Application.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Infrastructure.Auth
{
    public class JwtFactory : IJwtFactory
    {
        private readonly IConfiguration _config;
        private readonly IDateTimeService _dateTimeService;

        public JwtFactory(IConfiguration config, IDateTimeService dateTimeService)
        {
            _config = config;
            _dateTimeService = dateTimeService;
        }

        public string GenerateEncodedToken(string username, string[] roles)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:AccessTokenSecret"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            Claim[] GetClaims()
            {
                List<Claim> claims = new List<Claim>();

                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, username));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                foreach (var item in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }
                return claims.ToArray();
            }

            JwtSecurityToken token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"],
                                                          audience: _config["Jwt:Audience"],
                                                          GetClaims(),
                                                          expires: _dateTimeService.Now.AddHours(8),
                                                          signingCredentials: credentials);

            string encodeToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodeToken;
        }

        public string GenerateEncodedToken()
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:RefreshTokenSecret"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"],
                                                          audience: _config["Jwt:Audience"],
                                                          expires: _dateTimeService.Now.AddHours(24),
                                                          signingCredentials: credentials);

            string encodeToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodeToken;
        }

        public bool Validate(string refreshToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],

                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:RefreshTokenSecret"])),
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            try
            {
                jwtSecurityTokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}