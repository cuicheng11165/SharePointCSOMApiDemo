using Microsoft.SharePoint.Client;
using System;

namespace Permission
{
    internal static class CheckPermissionScenarios
    {
        // 1. 创建默认组
        public static void CreateDefaultGroups(string siteUrl, string userLoginName)
        {
            using (ClientContext context = new ClientContext(siteUrl))
            {
                context.Web.CreateDefaultAssociatedGroups(userLoginName, userLoginName, "DefaultGroup");
                context.ExecuteQuery();
                Console.WriteLine($"Default groups created for {userLoginName}.");
            }
        }

        // 2. 获取用户权限
        public static BasePermissions GetUserEffectivePermissions(string siteUrl, string userLoginName)
        {
            using (ClientContext context = new ClientContext(siteUrl))
            {
                var basePermission = context.Web.GetUserEffectivePermissions(userLoginName);
                context.ExecuteQuery();
                Console.WriteLine($"Fetched effective permissions for {userLoginName}.");
                return basePermission.Value;
            }
        }

        // 3. 获取贡献者权限定义
        public static RoleDefinition GetContributorRoleDefinition(string siteUrl)
        {
            using (ClientContext context = new ClientContext(siteUrl))
            {
                var contributorType = context.Web.RoleDefinitions.GetByType(RoleType.Contributor);
                context.Load(contributorType);
                context.ExecuteQuery();
                Console.WriteLine("Fetched Contributor RoleDefinition.");
                return contributorType;
            }
        }

        // 4. 检查权限位
        public static bool CheckContributorPermissionBits(BasePermissions basePermissions)
        {
            bool hasPermission = basePermissions.HasPermissions(48, 134287360);
            Console.WriteLine($"Has contributor permission bits: {hasPermission}");
            return hasPermission;
        }
    }
}
