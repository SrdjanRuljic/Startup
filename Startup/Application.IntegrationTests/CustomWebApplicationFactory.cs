using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Application.IntegrationTests
{
    using static Testing;

    internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                IConfigurationRoot integrationConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                                                                                 .AddEnvironmentVariables()
                                                                                 .Build();

                configurationBuilder.AddConfiguration(integrationConfig);
            });

            builder.ConfigureServices((builder, services) =>
            {
                services.Remove<ICurrentUserService>()
                        .AddTransient(provider => Mock.Of<ICurrentUserService>(s => s.UserName == GetCurrentUserName()));

                services.Remove<DbContextOptions<ApplicationDbContext>>()
                        .AddDbContext<ApplicationDbContext>((sp, options) =>
                            options.UseSqlServer(builder.Configuration.GetConnectionString("StartupDb"),
                                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            });
        }
    }
}