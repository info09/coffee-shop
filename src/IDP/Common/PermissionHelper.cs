using IDP.Infrastructure.Common;
using IDP.Infrastructure.ViewModels;

namespace IDP.Common;

public static class PermissionHelper
{
    public static string GetPermission(string function, string command)
    {
        return $"{function}.{command}";
    }

    public static List<PermissionAddModel> GetAllPermissions()
    {
        var permissions = new List<PermissionAddModel>();
        var functions = SystemConstants.Functions.GetAllFunctions();
        var commands = SystemConstants.Permissions.GetAllCommands();

        foreach (var function in functions)
        {
            foreach (var command in commands)
            {
                permissions.Add(new PermissionAddModel
                {
                    Function = function,
                    Command = command
                });
            }
        }

        return permissions;
    }
}
