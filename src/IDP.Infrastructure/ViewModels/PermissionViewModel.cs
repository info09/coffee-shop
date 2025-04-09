using IDP.Infrastructure.Domains;

namespace IDP.Infrastructure.ViewModels;

public class PermissionViewModel : EntityBase<long>
{
    public string RoleId { get; set; } = default!;
    public string Function { get; set; } = default!;
    public string Command { get; set; } = default!;
}
