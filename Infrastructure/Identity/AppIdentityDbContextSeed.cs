using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager){
            if(!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "ali",
                    Email = "ali@test.com",
                    UserName = "ali@test.com",
                    Address = new Address
                    {
                        FirstName = "ali",
                        LastName = "ali",
                        Street = "golesta 10",
                        City = "znjan",
                        State = "zn",
                        ZipCode = "98213"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}