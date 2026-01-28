using System;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.Online.SharePoint.TenantAdministration;
using CSOM.Common;

namespace UnifiedCsomTests.Scenarios
{
    internal static class UpdateWebPropertiesScenarios
    {
        /// <summary>
        /// 更新 Web AllProperties 属性
        /// </summary>
        internal static void UpdateWebAllProperties()
        {
            Console.WriteLine("请输入站点 URL (相对路径，如 /sites/Eira_Group_CA_03):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "/sites/test";

            Console.WriteLine("请输入要更新的属性名称:");
            var propertyName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(propertyName)) propertyName = "CustomProperty";

            Console.WriteLine("请输入属性值 (留空表示设置为 null):");
            var propertyValue = Console.ReadLine();

            var siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            var context = new ClientContext(siteUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            var web = context.Web;
            context.Load(web.AllProperties);
            context.ExecuteQuery();

            Console.WriteLine($"\n更新前属性值: {web.AllProperties[propertyName]}");

            if (string.IsNullOrEmpty(propertyValue))
            {
                web.AllProperties[propertyName] = null;
            }
            else
            {
                web.AllProperties[propertyName] = propertyValue;
            }

            web.Update();
            context.ExecuteQuery();

            Console.WriteLine($"成功更新属性 '{propertyName}'");
        }

        /// <summary>
        /// 列出 Web AllProperties 所有属性
        /// </summary>
        internal static void ListWebAllProperties()
        {
            Console.WriteLine("请输入站点 URL (相对路径，如 /sites/test):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "/sites/test";

            var siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            var context = new ClientContext(siteUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            var web = context.Web;
            context.Load(web.AllProperties);
            context.ExecuteQuery();

            Console.WriteLine("\nWeb AllProperties:");
            foreach (var key in web.AllProperties.FieldValues.Keys)
            {
                Console.WriteLine($"  {key}: {web.AllProperties[key]}");
            }
        }

        /// <summary>
        /// 启用或禁用自定义脚本 (DenyAddAndCustomizePages)
        /// </summary>
        internal static void ToggleDenyAddAndCustomizePages()
        {
            Console.WriteLine("请输入站点 URL (相对路径，如 /sites/test):");
            var siteRelative = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(siteRelative)) siteRelative = "/sites/test";

            Console.WriteLine("是否禁用自定义脚本? (y/n，y=禁用自定义，n=启用自定义):");
            var disable = Console.ReadLine();
            bool shouldDeny = disable?.ToLower() == "y";

            var siteUrl = EnvConfig.GetSiteUrl(siteRelative);
            var adminUrl = EnvConfig.GetAdminCenterUrl();

            var context = new ClientContext(adminUrl);
            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            var tenant = new Tenant(context);
            var siteProperties = tenant.GetSitePropertiesByUrl(siteUrl, false);
            
            siteProperties.DenyAddAndCustomizePages = shouldDeny 
                ? DenyAddAndCustomizePagesStatus.Enabled 
                : DenyAddAndCustomizePagesStatus.Disabled;
            
            siteProperties.Update();
            context.ExecuteQuery();

            Console.WriteLine($"成功设置 DenyAddAndCustomizePages 为: {(shouldDeny ? "Enabled (禁用自定义)" : "Disabled (启用自定义)")}");
        }

        /// <summary>
        /// 解析索引属性键
        /// </summary>
        internal static void DecodeIndexPropertyKeys()
        {
            Console.WriteLine("请输入 Base64 编码的索引属性键 (用 | 分隔):");
            Console.WriteLine("例如: UAB1AGIAbABpAHMAaAAgAHQAbwAgAEQAaQByAGUAYwB0AG8AcgB5AA==|SQBuAGQAZQB4AFQAZQBzAHQA|");
            
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                // 使用默认示例
                input = "UAB1AGIAbABpAHMAaAAgAHQAbwAgAEQAaQByAGUAYwB0AG8AcgB5AA==|SQBuAGQAZQB4AFQAZQBzAHQA|RwBBAF8AUABvAGwAaQBjAHkARABpAHMAcABsAGEAeQBOAGEAbQBlAA==|";
            }

            var keys = input.Split('|');
            Console.WriteLine("\n解码结果:");

            foreach (var key in keys)
            {
                if (string.IsNullOrWhiteSpace(key)) continue;

                try
                {
                    var decoded = Encoding.Unicode.GetString(Convert.FromBase64String(key));
                    Console.WriteLine($"  {key} => {decoded}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  {key} => 解码失败: {ex.Message}");
                }
            }
        }
    }
}
