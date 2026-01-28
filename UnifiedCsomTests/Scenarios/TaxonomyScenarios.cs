using System;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using CSOM.Common;

namespace UnifiedCsomTests.Scenarios
{
    internal static class TaxonomyScenarios
    {
        /// <summary>
        /// 创建分类组、术语集和术语
        /// </summary>
        internal static void CreateGroupTermSetAndTerms()
        {
            Console.WriteLine("请输入站点 URL (相对路径，如 /sites/mmstest001):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "/sites/mmstest001";

            var siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            var context = new ClientContext(siteUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            TaxonomySession session = TaxonomySession.GetTaxonomySession(context);
            context.Load(session.TermStores);
            context.ExecuteQuery();

            var termStore = session.TermStores[0];

            Console.WriteLine("请输入组名称:");
            var groupName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(groupName)) groupName = "TestGroup1";

            Console.WriteLine("请输入术语集名称:");
            var termSetName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(termSetName)) termSetName = "TestTermSet1";

            // 创建组
            var group = termStore.CreateGroup(groupName, Guid.NewGuid());

            // 创建术语集
            var termSet = group.CreateTermSet(termSetName, Guid.NewGuid(), 1033);

            // 创建术语
            var term = termSet.CreateTerm("testTerm1", 1033, Guid.NewGuid());

            // 创建子术语
            var subTerm1 = term.CreateTerm("subTerm1", 1033, Guid.NewGuid());

            context.ExecuteQuery();

            Console.WriteLine($"成功创建组 '{groupName}' 和术语集 '{termSetName}'");
        }

        /// <summary>
        /// 获取并列出术语集中的术语
        /// </summary>
        internal static void ListTermsInTermSet()
        {
            Console.WriteLine("请输入站点 URL (相对路径，如 /sites/mmstest001):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "/sites/mmstest001";

            Console.WriteLine("请输入术语集 GUID:");
            var termSetGuidStr = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(termSetGuidStr))
            {
                Console.WriteLine("需要提供术语集 GUID");
                return;
            }

            var siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            var context = new ClientContext(siteUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            TaxonomySession session = TaxonomySession.GetTaxonomySession(context);
            context.Load(session.TermStores);
            context.ExecuteQuery();

            var termStore = session.TermStores[0];
            var termset = termStore.GetTermSet(new Guid(termSetGuidStr));
            context.Load(termset.Terms);
            context.ExecuteQuery();

            Console.WriteLine($"\n术语集包含 {termset.Terms.Count} 个术语:");
            foreach (var term in termset.Terms)
            {
                Console.WriteLine($"  - {term.Name} (ID: {term.Id})");
            }
        }

        /// <summary>
        /// 按名称获取术语集
        /// </summary>
        internal static void GetTermSetByName()
        {
            Console.WriteLine("请输入站点 URL (相对路径，如 /sites/mmstest001):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "/sites/mmstest001";

            Console.WriteLine("请输入术语集名称:");
            var termSetName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(termSetName)) termSetName = "TestTermSet1";

            var siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            var context = new ClientContext(siteUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            TaxonomySession session = TaxonomySession.GetTaxonomySession(context);
            context.Load(session.TermStores);
            context.ExecuteQuery();

            var termStore = session.TermStores[0];
            var termSets = termStore.GetTermSetsByName(termSetName, 1033);

            context.Load(termSets);
            context.ExecuteQuery();

            Console.WriteLine($"\n找到 {termSets.Count} 个名为 '{termSetName}' 的术语集");

            if (termSets.Count > 0)
            {
                context.Load(termSets[0].Terms);
                context.ExecuteQuery();

                var set0 = termSets[0];
                Console.WriteLine($"第一个术语集 ID: {set0.Id}");
                Console.WriteLine($"包含 {set0.Terms.Count} 个术语");
            }
        }

        /// <summary>
        /// 创建托管元数据字段
        /// </summary>
        internal static void CreateManagedMetadataField()
        {
            Console.WriteLine("请输入站点 URL (相对路径，如 /sites/mmstest001):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "/sites/mmstest001";

            Console.WriteLine("请输入术语集 GUID:");
            var termSetGuidStr = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(termSetGuidStr))
            {
                Console.WriteLine("需要提供术语集 GUID");
                return;
            }

            Console.WriteLine("请输入 SSP ID (Term Store ID):");
            var sspIdStr = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(sspIdStr))
            {
                Console.WriteLine("需要提供 SSP ID");
                return;
            }

            var siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            var context = new ClientContext(siteUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            Web web = context.Web;
            context.Load(web);
            context.ExecuteQuery();

            string columnname = "MyManagedMetadataField" + Guid.NewGuid().ToString("N").Substring(0, 8);
            
            Field f = web.Fields.AddFieldAsXml(
                $"<Field Type='TaxonomyFieldType' Name='{columnname}' DisplayName='{columnname}' ShowField='Term1033' />",
                false,
                AddFieldOptions.DefaultValue);

            context.Load(f);
            context.ExecuteQuery();

            TaxonomyField taxField = context.CastTo<TaxonomyField>(f);
            taxField.SspId = new Guid(sspIdStr);
            taxField.TermSetId = new Guid(termSetGuidStr);
            taxField.AllowMultipleValues = false;
            taxField.Open = true;
            taxField.TargetTemplate = string.Empty;
            taxField.AnchorId = Guid.Empty;
            taxField.Update();
            context.ExecuteQuery();

            Console.WriteLine($"成功创建托管元数据字段: {columnname}");
            Console.WriteLine($"字段 ID: {f.Id}");
        }
    }
}
