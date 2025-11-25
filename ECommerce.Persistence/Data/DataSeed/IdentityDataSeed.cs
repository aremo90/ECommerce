using ECommerce.Domin.Contract.DataSeeding;
using ECommerce.Domin.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Data.DataSeed
{
    public class IdentityDataSeed : IDataini
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataSeed> _logger;

        public IdentityDataSeed(UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                ILogger<IdentityDataSeed> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitilizeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser
                    {
                        DisplayName = "Hamza Noor",
                        UserName = "hamzanoor",
                        Email = "Hamza@gmail.com",
                        PhoneNumber = "0123456789"
                    };
                    var User02 = new ApplicationUser
                    {
                        DisplayName = "Omar Ahmed",
                        UserName = "OmarAhmed",
                        Email = "Omar@gmail.com",
                        PhoneNumber = "0123456788"
                    };
                    await _userManager.CreateAsync(User01, "P@ssw0rd");
                    await _userManager.CreateAsync(User02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(User01, "Admin");
                    await _userManager.AddToRoleAsync(User02, "SuperAdmin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured during seeding identity data: {ex.Message}");
            }
        }
    }
}
