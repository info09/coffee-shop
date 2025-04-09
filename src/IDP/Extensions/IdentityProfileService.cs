using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IDP.Infrastructure.Entities;
using IDP.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IDP.Infrastructure.Repositories;
using IDP.Common;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace IDP.Extensions;

public class IdentityProfileService : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IRepositoryManager _repositoryManager;

    public IdentityProfileService(IUserClaimsPrincipalFactory<User> claimsFactory, UserManager<User> userManager, IRepositoryManager repositoryManager, RoleManager<IdentityRole> roleManager)
    {
        _claimsFactory = claimsFactory;
        _userManager = userManager;
        _repositoryManager = repositoryManager;
        _roleManager = roleManager;
    }
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        if (user == null)
        {
            throw new ArgumentNullException("User Id not found!");
        }

        var principal = await _claimsFactory.CreateAsync(user);
        var claims = principal.Claims.ToList();
        var roles = await _userManager.GetRolesAsync(user);
        var roleNames = await _roleManager.Roles.Where(i => roles.Contains(i.Id)).Select(i => i.Name).ToListAsync();
        var permissionQuery = await _repositoryManager.Permission.GetPermissionsByUser(user);
        var permissions = permissionQuery.Select(x => PermissionHelper.GetPermission(x.Function, x.Command));
        var allPermissions = PermissionHelper.GetAllPermissions().Select(i => PermissionHelper.GetPermission(i.Function, i.Command));
        var userPermissions = roleNames.Contains(SystemConstants.Roles.Administrator) ? allPermissions : permissions;
        //Add more claims like this
        claims.Add(new Claim(SystemConstants.Claims.FirstName, user.FirstName));
        claims.Add(new Claim(SystemConstants.Claims.LastName, user.LastName));
        claims.Add(new Claim(SystemConstants.Claims.UserName, user.UserName!));
        claims.Add(new Claim(SystemConstants.Claims.UserId, user.Id));
        claims.Add(new Claim(ClaimTypes.Name, user.UserName!));
        claims.Add(new Claim(ClaimTypes.Email, user.Email!));
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        claims.Add(new Claim(SystemConstants.Claims.Roles, string.Join(";", roles)));
        claims.Add(new Claim(SystemConstants.Claims.Permissions, JsonSerializer.Serialize(userPermissions)));

        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}
