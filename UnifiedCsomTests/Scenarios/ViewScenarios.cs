using System;
using System.Net;
using Microsoft.SharePoint.Client;
using CSOM.Common;

namespace UnifiedCsomTests.Scenarios
{
    internal static class ViewScenarios
    {
        /// <summary>
        /// 测试视图和视图字段的加载
        /// </summary>
        internal static void TestViewAndViewFields()
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            Console.WriteLine("请输入站点 URL (相对路径，如 /sites/yoursite，回车使用默认):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "";

            Console.WriteLine("请输入列表 GUID (回车使用默认测试 GUID):");
            var listGuidStr = Console.ReadLine();
            Guid listGuid;
            if (string.IsNullOrWhiteSpace(listGuidStr))
            {
                listGuid = new Guid("2d2d3e92-add6-4070-9117-837f21eb71a0");
            }
            else
            {
                listGuid = new Guid(listGuidStr);
            }

            var siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            using (ClientContext clientContext = new ClientContext(siteUrl))
            {
                clientContext.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
                {
                    e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                        EnvConfig.GetCsomToken();
                };

                var currentWeb = clientContext.Web;
                List list = currentWeb.Lists.GetById(listGuid);

                clientContext.Load(list, l => l.Views.IncludeWithDefaultProperties(v => v.ViewFields));
                clientContext.ExecuteQuery();

                Console.WriteLine($"列表加载成功，视图数量: {list.Views.Count}");

                if (list.Views.Count > 0)
                {
                    var viewFile = clientContext.Web.GetFileByServerRelativeUrl(list.Views[0].ServerRelativeUrl);
                    clientContext.Load(viewFile, v => v.ETag);
                    clientContext.ExecuteQuery();

                    Console.WriteLine($"第一个视图 URL: {list.Views[0].ServerRelativeUrl}");
                    Console.WriteLine($"视图文件 ETag: {viewFile.ETag}");
                }
            }

            Console.WriteLine("视图测试完成！");
        }
    }
}
