namespace IDP.Infrastructure.Common;

public static class SystemConstants
{
    public const string IdentitySchema = "Identity";

    public static class Claims
    {
        public const string Roles = "roles";
        public const string Permissions = "permissions";
        public const string UserId = "id";
        public const string UserName = "userName";
        public const string FirstName = "firstName";
        public const string LastName = "lastName";
    }

    public static class Roles
    {
        public const string Administrator = "Administrator";
        public const string Customer = "Customer";
    }

    public static class Functions
    {
        public const string Company = "COMPANY";

        public static List<string> GetAllFunctions()
        {
            return new List<string>
            {
                Company
            };
        }
    }

    public static class Permissions
    {
        public const string VIEW = "VIEW";
        public const string CREATE = "CREATE";
        public const string UPDATE = "UPDATE";
        public const string DELETE = "DELETE";

        public static List<string> GetAllCommands()
        {
            return new List<string>
            {
                VIEW,
                CREATE,
                UPDATE,
                DELETE
            };
        }
    }
}
