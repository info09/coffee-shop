using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDP.Entities.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "Administrator".ToUpper(),
                    Id = "b6105f01-18f5-433c-91e0-dbd80d27e7f4"
                },
                new IdentityRole
                {
                    Name = "Customer",
                    NormalizedName = "Customer".ToUpper(),
                    Id = "b4365573-ff95-4015-8dd0-adf0650354a2"
                }
            );
        }
    }
}
