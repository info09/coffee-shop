using IDP.Infrastructure.Entities;
using IDP.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IDP.Extensions;

public class IDPUserStore : UserStore<User, IdentityRole, ApplicationDbContext>
{
    public IDPUserStore(ApplicationDbContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
    {
    }

    public override async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        var query = from userRole in Context.UserRoles
                    join role in Context.Roles on userRole.RoleId equals role.Id
                    where userRole.UserId.Equals(user.Id)
                    select role.Id;

        return await query.ToListAsync(cancellationToken);
    }
}
