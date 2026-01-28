using System;
using Microsoft.SharePoint.Client;
using CSOM.Common;

namespace UnifiedCsomTests.Scenarios
{
    internal static class CamlQueryScenarios
    {
        /// <summary>
        /// 基本 CAML 查询示例
        /// </summary>
        internal static void BasicCamlQuery()
        {
            Console.WriteLine("请输入站点 URL (相对路径，回车使用默认):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "";

            Console.WriteLine("请输入列表标题 (回车使用 'Announcements'):");
            var listTitle = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(listTitle)) listTitle = "Announcements";

            string siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            ClientContext clientContext = new ClientContext(siteUrl);

            clientContext.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            List oList = clientContext.Web.Lists.GetByTitle(listTitle);

            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = @"
                                    <View>
                                      <Query>
                                        <Where>
                                          <Geq>
                                            <FieldRef Name='ID'/>
                                            <Value Type='Number'>10</Value>
                                          </Geq>
                                        </Where>
                                      </Query>
                                      <RowLimit>100</RowLimit>
                                    </View>";

            ListItemCollection collListItem = oList.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();

            Console.WriteLine($"查询结果数量: {collListItem.Count}");
            foreach (ListItem oListItem in collListItem)
            {
                Console.WriteLine($"ID: {oListItem.Id}, Title: {oListItem["Title"]}");
            }
        }

        /// <summary>
        /// 分页查询示例
        /// </summary>
        internal static void PaginatedCamlQuery()
        {
            Console.WriteLine("请输入站点 URL (相对路径，回车使用默认):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "";

            Console.WriteLine("请输入列表标题 (回车使用 'VersionTest'):");
            var listTitle = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(listTitle)) listTitle = "VersionTest";

            Console.WriteLine("请输入文件夹服务器相对 URL (回车跳过):");
            var folderUrl = Console.ReadLine();

            string siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            ClientContext clientContext = new ClientContext(siteUrl);

            clientContext.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            List oList = clientContext.Web.Lists.GetByTitle(listTitle);
            clientContext.Load(oList);
            clientContext.ExecuteQuery();

            ListItemCollection listItems;
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View><RowLimit>2</RowLimit></View>";

            if (!string.IsNullOrWhiteSpace(folderUrl))
            {
                camlQuery.FolderServerRelativeUrl = folderUrl;
            }

            ListItemCollectionPosition? pos = null;
            int pageCount = 0;

            do
            {
                camlQuery.ListItemCollectionPosition = pos;
                listItems = oList.GetItems(camlQuery);
                clientContext.Load(listItems, 
                    items => items.ListItemCollectionPosition, 
                    items => items.IncludeWithDefaultProperties(item => item["FSObjType"])
                        .Where(item => (string)item["FSObjType"] == "0"));
                clientContext.ExecuteQuery();

                pageCount++;
                Console.WriteLine($"第 {pageCount} 页，项目数: {listItems.Count}");

                foreach (ListItem item in listItems)
                {
                    Console.WriteLine($"  ID: {item.Id}, Title: {item["Title"]}");
                }

                pos = listItems.ListItemCollectionPosition;
            }
            while (pos != null);

            Console.WriteLine($"分页查询完成，共 {pageCount} 页");
        }

        /// <summary>
        /// 创建带字段的查询
        /// </summary>
        internal static void CreateAllItemsQuery()
        {
            Console.WriteLine("创建带字段的 CAML 查询示例");

            var query = CamlQuery.CreateAllItemsQuery(100, new[] { "Editor", "Column1", "Column2" });
            
            Console.WriteLine("请输入文件夹服务器相对 URL (可选，回车跳过):");
            var folderUrl = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(folderUrl))
            {
                query.FolderServerRelativeUrl = folderUrl;
            }

            Console.WriteLine($"查询 XML: {query.ViewXml}");
            Console.WriteLine($"文件夹 URL: {query.FolderServerRelativeUrl ?? "(未设置)"}");
            Console.WriteLine("查询对象已创建！");
        }
    }
}
