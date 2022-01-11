﻿using Application.Common.Interfaces;
using Domain.Entities.Identity;
using System.Threading.Tasks;

namespace Application.System.Commands.SeedData.DefaultUsers
{
    public static class Admin
    {
        public static async Task SeedAsync(IManagersService managersServices)
        {
            //Seed Default User
            AppUser admin = new AppUser
            {
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                UserName = "admin",
                FirstName = "Admin",
                LastName = "Admin",
            };

            AppUser user = await managersServices.FindByUserNameAsync(admin.UserName);

            if (user == null)
                await managersServices.CreateUserAsync(admin, "Administrator_123!", new string[] { Domain.Enums.Roles.Admin.ToString() });
        }
    }
}