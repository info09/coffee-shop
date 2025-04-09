﻿using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IDP.Infrastructure.Entities;
using IDP.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IDP.Extensions;

public class IdentityProfileService : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;
    private readonly UserManager<User> _userManager;

    public IdentityProfileService(IUserClaimsPrincipalFactory<User> claimsFactory, UserManager<User> userManager)
    {
        _claimsFactory = claimsFactory;
        _userManager = userManager;
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
        //var permissionQuery = await _repositoryManager.Permission.GetPermissionsByUser(user);
        //var permissions = permissionQuery.Select(x => PermissionHelper.GetPermission(x.Function, x.Command));
        //Add more claims like this
        claims.Add(new Claim(SystemConstants.Claims.FirstName, user.FirstName));
        claims.Add(new Claim(SystemConstants.Claims.LastName, user.LastName));
        claims.Add(new Claim(SystemConstants.Claims.UserName, user.UserName!));
        claims.Add(new Claim(SystemConstants.Claims.UserId, user.Id));
        claims.Add(new Claim(ClaimTypes.Name, user.UserName!));
        claims.Add(new Claim(ClaimTypes.Email, user.Email!));
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        claims.Add(new Claim(SystemConstants.Claims.Roles, string.Join(";", roles)));
        //claims.Add(new Claim(SystemConstants.Claims.Permissions, JsonSerializer.Serialize(permissions)));

        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}
