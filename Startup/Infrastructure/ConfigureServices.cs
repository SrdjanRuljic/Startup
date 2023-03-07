﻿using Application.Common.Excentions;
using Application.Common.Interfaces;
using Application.Exceptions;
using Domain.Entities.Identity;
using Infrastructure.Auth;
using Infrastructure.EmailSender;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Text;

namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            string connectionString = configuration.GetConnectionString("StartupDb");

            services.AddScoped<ApplicationDbContextInitialiser>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("StartupDb"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromHours(2));

            services.Configure<IdentityOptions>(options =>
                options.SignIn.RequireConfirmedEmail = true);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = configuration["Jwt:Issuer"],

                    ValidateAudience = false,
                    ValidAudience = configuration["Jwt:Audience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:AccessTokenSecret"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.AddUnauthorisedExcention(context.ErrorDescription);
                        await context.Response.WriteAsync(ErrorMessages.Unauthorised);
                    }
                };
            });

            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<IManagersService, ManagersService>();
            services.AddTransient<IDateTimeService, DateTimeService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(Domain.Enums.Roles.Admin.ToString());
                });

                options.AddPolicy("RequireAuthorization", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });

            EmailConfiguration emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();

            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSenderService, EmailSenderService>();

            return services;
        }
    }
}