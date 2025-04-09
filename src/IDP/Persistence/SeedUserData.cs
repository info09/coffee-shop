using Duende.IdentityModel;
using IDP.Infrastructure.Entities;
using IDP.Infrastructure.Common;
using IDP.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IDP.Persistence;

public static class SeedUserData
{
    public static async void EnsureSeedData(string connectionString)
    {
        var service = new ServiceCollection();
        service.AddLogging();
        service.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        service.AddIdentity<User, IdentityRole>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireDigit = false;
            opt.Password.RequiredLength = 6;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireLowercase = false;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        using (var serviceProvider = service.BuildServiceProvider())
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                await CreateAdminUserAsync(scope, "Huy", "Tran", "HaNoi", Guid.NewGuid().ToString(), "Admin@123$", SystemConstants.Roles.Administrator, "huytq@ics-p.vn");
            }
        }
    }

    private static async Task CreateAdminUserAsync(IServiceScope scope, string firstName, string lastName,
       string address, string id, string password, string role, string email)
    {
        var userManagement = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = await userManagement.FindByNameAsync(email);
        if (user == null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                EmailConfirmed = true,
                Id = id,
            };
            var result = await userManagement.CreateAsync(user, password);
            CheckResult(result);

            var addToRoleResult = await userManagement.AddToRoleAsync(user, role);
            CheckResult(addToRoleResult);

            result = userManagement.AddClaimsAsync(user, new Claim[]
            {
                new(SystemConstants.Claims.UserName, user.UserName),
                new(SystemConstants.Claims.FirstName, user.FirstName),
                new(SystemConstants.Claims.LastName, user.LastName),
                new(SystemConstants.Claims.Roles, role),
                new(JwtClaimTypes.Address, user.Address),
                new(JwtClaimTypes.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id),
            }).Result;
            CheckResult(result);
        }
    }

    private static void CheckResult(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }
    }
}
