using Application.Common.Interfaces;
using Application.System.Commands.SeedData;
using Domain.Entities.Identity;
using Infrastructure.Identity;
using Infrastructure.Identity.Extensions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI;

namespace Application.IntegrationTests
{
    [SetUpFixture]
    public class Testing
    {
        private static Checkpoint _checkpoint;
        private static IConfigurationRoot _configuration;
        private static string _currentUserUserName;
        private static IServiceScopeFactory _scopeFactory;

        public static async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync();
        }

        public static async Task<int> CountAsync<TEntity>() where TEntity : class
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await context.Set<TEntity>().CountAsync();
        }

        public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await context.FindAsync<TEntity>(keyValues);
        }

        public static async Task ResetState()
        {
            await _checkpoint.Reset(_configuration.GetConnectionString("StartupDb"));

            _currentUserUserName = null;
        }

        public static async Task<string> RunAsAdministratorAsync()
        {
            return await RunAsUserAsync("administrator@local", "Administrator1234!", new[] { "Administrator" });
        }

        public static async Task<string> RunAsDefaultUserAsync()
        {
            return await RunAsUserAsync("test@local", "Testing1234!", Array.Empty<string>());
        }

        public static async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            AppUser user = new AppUser
            {
                UserName = userName,
                Email = userName
            };

            IdentityResult result = await userManager.CreateAsync(user, password);

            if (roles.Any())
            {
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                foreach (string role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                await userManager.AddToRolesAsync(user, roles);
            }

            if (result.Succeeded)
            {
                _currentUserUserName = user.UserName;

                return _currentUserUserName;
            }

            string errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

            throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            ISender mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            return await mediator.Send(request);
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }

        [OneTimeSetUp]
        public async Task RunBeforeAnyTests()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                      .AddJsonFile("appsettings.json", true, true)
                                                                      .AddEnvironmentVariables();

            _configuration = builder.Build();

            Startup startup = new Startup(_configuration);

            ServiceCollection services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w => w.EnvironmentName == "Development" && w.ApplicationName == "WebAPI"));

            services.AddLogging();

            startup.ConfigureServices(services);

            // Replace service registration for ICurrentUserService
            // Remove existing registration
            ServiceDescriptor currentUserServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ICurrentUserService));

            if (currentUserServiceDescriptor != null)
                services.Remove(currentUserServiceDescriptor);

            // Register testing version
            services.AddTransient(provider => Mock.Of<ICurrentUserService>(s => s.UserName == _currentUserUserName));
            services.AddTransient(provider => Mock.Of<IJwtFactory>());

            _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };

            await EnsureDatabase();
        }

        private static async Task EnsureDatabase()
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new SeedDataCommand(), CancellationToken.None);
        }
    }
}