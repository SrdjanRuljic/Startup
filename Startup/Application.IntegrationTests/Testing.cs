using NUnit.Framework;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Respawn;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Domain.Entities.Identity;
using Infrastructure.Identity.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.IntegrationTests
{
    [SetUpFixture]
    public partial class Testing
    {
        private static Respawner _checkpoint;
        private static IConfiguration _configuration;
        private static string _currentUserName;
        private static WebApplicationFactory<Program> _factory;
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

        public static string GetCurrentUserName()
        {
            return _currentUserName;
        }

        public static async Task ResetState()
        {
            try
            {
                await _checkpoint.ResetAsync(_configuration.GetConnectionString("StartupDb")!);
            }
            catch (Exception)
            {
            }

            _currentUserName = null;
        }

        public static async Task<string> RunAsAdministratorAsync()
        {
            return await RunAsUserAsync("admin", "Administrator_123!", new[] { Domain.Enums.Roles.Admin.ToString() });
        }

        public static async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            AppUser user = new AppUser { UserName = userName, Email = userName };

            IdentityResult result = await userManager.CreateAsync(user, password);

            if (roles.Any())
            {
                RoleManager<AppRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

                foreach (string role in roles)
                {
                    await roleManager.CreateAsync(new AppRole()
                    {
                        Name = role
                    });
                }

                await userManager.AddToRolesAsync(user, roles);
            }

            if (result.Succeeded)
            {
                _currentUserName = user.UserName;

                return _currentUserName;
            }

            string errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

            throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
        }

        public static async Task SendAsync(IBaseRequest request)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            ISender sender = scope.ServiceProvider.GetRequiredService<ISender>();

            await sender.Send(request);
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            ISender sender = scope.ServiceProvider.GetRequiredService<ISender>();

            return await sender.Send(request);
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            _factory = new CustomWebApplicationFactory();
            _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
            _configuration = _factory.Services.GetRequiredService<IConfiguration>();

            _checkpoint = Respawner.CreateAsync(_configuration.GetConnectionString("StartupDb"), new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
            }).GetAwaiter().GetResult();
        }
    }
}