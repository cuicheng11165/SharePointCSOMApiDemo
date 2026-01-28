using System;
using System.Net;
using Microsoft.SharePoint.Client;
using CSOM.Common;

namespace UnifiedCsomTests.Scenarios
{
    internal static class ExceptionHandlingScopeScenarios
    {
        /// <summary>
        /// 演示 ExceptionHandlingScope 的使用 - 在一个请求中处理可能的异常
        /// </summary>
        internal static void TestTryCatchFolderCreation()
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            Console.WriteLine("请输入站点 URL (相对路径，如 /sites/yoursite):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "/sites/simmon1750";

            Console.WriteLine("请输入列表标题 (回车使用 'Documents'):");
            var listTitle = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(listTitle)) listTitle = "Documents";

            Console.WriteLine("请输入文件夹 URL (如 'Shared%20Documents/f2'):");
            var folderUrl = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(folderUrl)) folderUrl = "Shared%20Documents/f2";

            var siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            using (ClientContext clientContext = new ClientContext(siteUrl))
            {
                clientContext.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
                {
                    e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                        EnvConfig.GetCsomToken();
                };

                var currentWeb = clientContext.Web;
                var listGetById = currentWeb.Lists.GetByTitle(listTitle);

                var exceptionHandlingScope = new ExceptionHandlingScope(clientContext);

                using (var currentScope = exceptionHandlingScope.StartScope())
                {
                    using (exceptionHandlingScope.StartTry())
                    {
                        // 尝试获取文件夹并设置权限
                        var folder = listGetById.RootFolder.Folders.GetByUrl(folderUrl);
                        folder.ListItemAllFields.BreakRoleInheritance(true, true);
                    }
                    using (exceptionHandlingScope.StartCatch())
                    {
                        // 如果文件夹不存在，则创建它并设置权限
                        var folder = listGetById.RootFolder.Folders.Add(folderUrl);
                        folder.ListItemAllFields.BreakRoleInheritance(true, true);
                    }
                }

                clientContext.ExecuteQuery();

                // 输出服务器端是否出现异常
                Console.WriteLine($"服务器端是否出现异常: {exceptionHandlingScope.HasException}");
                if (exceptionHandlingScope.HasException)
                {
                    Console.WriteLine($"服务器端异常信息: {exceptionHandlingScope.ErrorMessage}");
                }
                else
                {
                    Console.WriteLine("操作成功完成！");
                }
            }
        }
    }
}
