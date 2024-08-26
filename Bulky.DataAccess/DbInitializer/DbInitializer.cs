using Bulky.DataAccess.Services;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDbContext Context;
        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext Context) 
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.Context = Context;
        }
        public void Initialize()
        {
            //migrations if they are not applied
            try
            {
                if (Context.Database.GetPendingMigrations().Count() > 0)
                {
                    Context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
            }

            //create roles if they are not created
            if (!roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

                //if roles are not created, then we will create admin user as well
                userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "ahmedtaaher@gmail.com",
                    Email = "ahmedtaaher@gmail.com",
                    Name = "Ahmed Taher",
                    PhoneNumber = "123456789",
                    StreetAddress = "test 123",
                    State = "IL",
                    PostalCode = "1234",
                    City = "Cairo"
                }, "AhmedA123*").GetAwaiter().GetResult();


                ApplicationUser user = Context.ApplicationUsers.FirstOrDefault(u => u.Email == "ahmedtaaher@gmail.com");
                userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

            }

            return;
        }

    }
}
