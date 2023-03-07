using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public ApplicationDbContextInitialiser(ApplicationDbContext context,
                                           ILogger<ApplicationDbContextInitialiser> logger,
                                           RoleManager<AppRole> roleManager,
                                           UserManager<AppUser> userManager)
    {
        _context = context;
        _logger = logger;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task SeedRolesAsync()
    {
        await _roleManager.CreateAsync(new AppRole()
        {
            Name = Domain.Enums.Roles.Admin.ToString()
        });
        await _roleManager.CreateAsync(new AppRole()
        {
            Name = Domain.Enums.Roles.Moderator.ToString()
        });
        await _roleManager.CreateAsync(new AppRole()
        {
            Name = Domain.Enums.Roles.Basic.ToString()
        });
    }

    public async Task SeedUsersAsync()
    {
        AppUser admin = new AppUser
        {
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            UserName = "admin",
            FirstName = "Admin",
            LastName = "Admin",
        };

        AppUser user = await _userManager.FindByNameAsync(admin.UserName);

        if (user == null)
        {
            await _userManager.CreateAsync(admin, "Administrator_123!");
            await _userManager.AddToRolesAsync(admin, new[] { Domain.Enums.Roles.Admin.ToString() });
        }
    }

    public async Task TrySeedAsync()
    {
        if (!await _roleManager.Roles.AnyAsync())
            await SeedRolesAsync();
        if (!await _userManager.Users.AnyAsync())
            await SeedUsersAsync();
        else
            return;
    }
}