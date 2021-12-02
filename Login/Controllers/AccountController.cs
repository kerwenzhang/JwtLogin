using Login.Models;
using Login.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ITokenService tokenService;
        public AccountController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            IActionResult response = Unauthorized();

            // Connect remote server to validate user name and password
            bool bValidated = await ValidateUserByWcfService(user);
            if (bValidated)
            {
                var token = this.tokenService.GetToken(user);
                Response.Cookies.Append("Authorization", token, this.tokenService.GetCookieSetting(Request, true));
                Response.Cookies.Append("UserName", user.UserName, this.tokenService.GetCookieSetting(Request, false));

                response = Ok();
            }
            return response;
        }

        private Task<bool> ValidateUserByWcfService(User user)
        {
            return Task.FromResult(true);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Authorization", this.tokenService.GetCookieSetting(Request, true));
            Response.Cookies.Delete("UserName", this.tokenService.GetCookieSetting(Request, false));
            return Ok();
        }
    }
}
