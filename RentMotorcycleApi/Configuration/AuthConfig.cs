using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RentMotorcycle.Business.Models.Security;
using System.Text;

namespace RentMotorcycleApi.Configuration
{
    public static class AuthConfig
    {
        public static IServiceCollection AddJwtConfig(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection tokensSection = configuration.GetSection("tokens");
            Tokens tokens = tokensSection.Get<Tokens>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = tokens.Issuer,
                    ValidAudience = tokens.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokens.Key))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Roles", policy => policy.RequireRole("ADMIN, USER"));
            });

            return services;
        }
    }
}
