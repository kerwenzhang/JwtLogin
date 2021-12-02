using Login.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Services
{
    public interface ITokenService
    {
        public string GetToken(User user);
        public CookieOptions GetCookieSetting(HttpRequest request, bool httpOnly);
        public void RefreshJwtToken(TokenValidatedContext context);
    }
}
