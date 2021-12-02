using Login.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Login.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        public TokenService(IOptions<JwtSettings> option)
        {
            _jwtSettings = option.Value;
        }
        public string GetToken(User user)
        {
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(30)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName.ToString()),
                    new Claim("Name", user.UserName.ToString())
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.Now.AddSeconds(30),
                signingCredentials: creds,
                claims: claims
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        public void RefreshJwtToken(TokenValidatedContext context)
        {
            string tokenString = context.Request.Cookies["Authorization"];
            User user = ReadToken(tokenString);

            var jwt = GetToken(user);
            try
            {
                context.Response.Cookies.Append("Authorization", jwt, GetCookieSetting(context.Request, true));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured during refresh token. " + ex);
            }
        }

        private User ReadToken(string tokenString)
        {
            User user = new User();
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
            foreach (Claim claim in token.Claims)
            {
                if (claim.Type == "Name")
                {
                    user.UserName = claim.Value;
                }
            }
            return user;
        }

        public CookieOptions GetCookieSetting(HttpRequest request, bool httpOnly)
        {
            return new CookieOptions()
            {
                HttpOnly = httpOnly,
                SameSite = SameSiteMode.Strict,
                Domain = request.Host.Host
            };

        }
    }
}
