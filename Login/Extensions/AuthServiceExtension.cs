using Login.Models;
using Login.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Extensions
{
    public static class AuthServiceExtension
    {
        public static IServiceCollection AddAuthService(this IServiceCollection services)
        {
            IOptions<JwtSettings> jwtSettings = services.BuildServiceProvider().GetRequiredService<IOptions<JwtSettings>>();
            ITokenService tokenService = services.BuildServiceProvider().GetRequiredService<ITokenService>();
            services.AddAuthorization()
               .AddAuthentication(x =>
               {
                   x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                   x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               })
               .AddJwtBearer(option =>
               {
                   option.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.FromSeconds(30),

                       ValidateAudience = true,
                       AudienceValidator = (m, n, z) =>
                       {
                           return m != null && m.FirstOrDefault().Equals(jwtSettings.Value.Audience);
                       },
                       ValidateIssuer = true,
                       ValidIssuer = jwtSettings.Value.Issuer,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.SecurityKey))
                   };
                   option.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           if (context.Request.Cookies.ContainsKey("Authorization"))
                           {
                               context.Token = context.Request.Cookies["Authorization"];

                           }
                           return Task.CompletedTask;
                       },
                       OnTokenValidated = context =>
                       {
                           Task.Run(() =>
                           {
                               try
                               {
                                   tokenService.RefreshJwtToken(context);
                               }
                               catch (Exception ex)
                               {
                                   Console.WriteLine("Failed to refresh token " + ex);
                               }
                           });

                           return Task.CompletedTask;
                       }
                   };
               });

            return services;
        }
    }
}
