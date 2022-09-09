using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MaxSoftTechAssignment.DAL.Configurations;
using MaxSoftTechAssignment.DAL.Data;
using MaxSoftTechAssignment.DAL.Entities;
using MaxSoftTechAssignment.DAL.Extentions;
using MaxSoftTechAssignment.DAL.Interfaces;
using MaxSoftTechAssignment.DAL.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MaxSoftTechAssignment.DAL;

public static class ServiceSetup
{
    public static void AddDALServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        
    }
     public static void AddJwtIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var jwtConfiguration = configuration.GetSection(nameof(JwtConfiguration));

            var signingKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(jwtConfiguration["Secret"]));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var issuer = jwtConfiguration[nameof(JwtConfiguration.Issuer)];
            var audience = jwtConfiguration[nameof(JwtConfiguration.Audience)];
            var validFor = TimeSpan.FromMinutes(jwtConfiguration[nameof(JwtConfiguration.ValidFor)].AsInt());
            var refreshTokenTtl = jwtConfiguration[nameof(JwtConfiguration.RefreshTokenTtl)].AsInt();
           
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var tokenValidationParamaters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingCredentials.Key,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.Configure<JwtConfiguration>(options =>
            {
                options.Issuer = issuer;
                options.Audience = audience;
                options.ValidFor = validFor;
                options.SigningCredentials = signingCredentials;
                options.RefreshTokenTtl = refreshTokenTtl;
                
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(configureOptions =>
                {
                    configureOptions.ClaimsIssuer = issuer;
                    configureOptions.TokenValidationParameters = tokenValidationParamaters;
                    configureOptions.SaveToken = true;
                    configureOptions.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.HttpContext.Request.Headers["Authorization"];
                            if (token.Count > 0 && token[0].StartsWith("Token", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = token[0]["Token ".Length..].Trim();
                            }
                            return Task.CompletedTask;

                        },
                        OnAuthenticationFailed = context =>
                        {
                            if(context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                context.Response.Headers.Add("Token-Expired","true");
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddScoped<IJwtFactory, JwtFactory>();
        }
}