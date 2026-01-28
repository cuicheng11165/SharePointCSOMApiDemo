using System;
using Microsoft.SharePoint.Client;
using CSOM.Common;

namespace UnifiedCsomTests.Scenarios
{
    internal static class TimeZoneScenarios
    {
        /// <summary>
        /// 测试文档的 Modified 时间和时区处理
        /// </summary>
        internal static void TestClientAPI_Document()
        {
            Console.WriteLine("测试 Client API - 文档时间");
            Console.WriteLine("请输入站点 URL (相对路径，回车使用默认):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "";

            Console.WriteLine("请输入文件服务器相对 URL (如 '/sites/Test/DocumentTest/12345.txt'):");
            var fileUrl = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fileUrl))
            {
                Console.WriteLine("需要提供文件 URL");
                return;
            }

            var siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            ClientContext context = new ClientContext(siteUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            var web = context.Web;
            var file = web.GetFileByServerRelativeUrl(fileUrl);
            var listItem = file.ListItemAllFields;

            Console.WriteLine("\n--- 测试不同 DateTimeKind 的行为 ---\n");

            DateTime dateTime = new DateTime(2014, 1, 10, 0, 0, 0, 0, DateTimeKind.Local);
            SetAndOutputModified(context, listItem, dateTime);

            DateTime dateTime1 = new DateTime(2015, 1, 10, 0, 0, 0, 0, DateTimeKind.Local);
            SetAndOutputModified(context, listItem, dateTime1);

            DateTime dateTime2 = new DateTime(2016, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc);
            SetAndOutputModified(context, listItem, dateTime2);

            DateTime dateTime3 = new DateTime(2017, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified);
            SetAndOutputModified(context, listItem, dateTime3);

            Console.WriteLine("文档时间测试完成！");
        }

        /// <summary>
        /// 测试列表项的 Modified 时间和时区处理
        /// </summary>
        internal static void TestClientAPI_ListItem()
        {
            Console.WriteLine("测试 Client API - 列表项时间");
            Console.WriteLine("请输入站点 URL (相对路径，回车使用默认):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "";

            Console.WriteLine("请输入列表标题:");
            var listTitle = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(listTitle)) listTitle = "customList";

            Console.WriteLine("请输入列表项 ID:");
            var itemIdStr = Console.ReadLine();
            if (!int.TryParse(itemIdStr, out int itemId)) itemId = 1;

            var siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            ClientContext context = new ClientContext(siteUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            var web = context.Web;
            var list = web.Lists.GetByTitle(listTitle);
            var listItem = list.GetItemById(itemId);

            Console.WriteLine("\n--- 测试不同 DateTimeKind 的行为 ---\n");

            DateTime dateTime = new DateTime(2014, 1, 10, 0, 0, 0, 0, DateTimeKind.Local);
            SetAndOutputModified(context, listItem, dateTime);

            DateTime dateTime1 = new DateTime(2015, 1, 10, 0, 0, 0, 0, DateTimeKind.Local);
            SetAndOutputModified(context, listItem, dateTime1);

            DateTime dateTime2 = new DateTime(2016, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc);
            SetAndOutputModified(context, listItem, dateTime2);

            DateTime dateTime3 = new DateTime(2017, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified);
            SetAndOutputModified(context, listItem, dateTime3);

            Console.WriteLine("列表项时间测试完成！");
        }

        private static void SetAndOutputModified(ClientContext context, ListItem listItem, DateTime dateTime)
        {
            Console.WriteLine($"设置时间: {dateTime:yyyy-MM-dd HH:mm:ss} , Kind: {dateTime.Kind}");

            listItem["Modified"] = dateTime;
            listItem.Update();
            context.ExecuteQuery();

            context.Load(listItem);
            context.ExecuteQuery();
            var modifiedTime = (DateTime)listItem["Modified"];

            Console.WriteLine($"读取时间: {modifiedTime:yyyy-MM-dd HH:mm:ss} , Kind: {modifiedTime.Kind}");
            Console.WriteLine();
        }
    }
}
