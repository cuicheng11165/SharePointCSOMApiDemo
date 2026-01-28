using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CSOM.Common;

namespace UpdateWebAllProperties
{
    class Program
    {
        static void Main(string[] args)
        {


            System.Net.ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
            
            var siteUrl = EnvConfig.GetSiteUrl("/sites/Eira_Group_CA_03");
            ClientContext context = new ClientContext(siteUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

         


            var adminUrl = EnvConfig.GetAdminCenterUrl();
            ClientContext context1 = new ClientContext(adminUrl);

            context1.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };
       
            var tenant = new Tenant(context1);
            var siteProperties = tenant.GetSitePropertiesByUrl(siteUrl, false);
            siteProperties.DenyAddAndCustomizePages = DenyAddAndCustomizePagesStatus.Disabled;
            siteProperties.Update();
            context1.ExecuteQuery();

         


            var web = context.Web;
            context.Load(web.AllProperties);
            context.ExecuteQuery();
            web.AllProperties["Eira_Radio_01"] = null;
            web.Update();
            context.ExecuteQuery();
        


        }


        private static void GetIndexProperty()
        {
            var indexPropertyKeys = "UAB1AGIAbABpAHMAaAAgAHQAbwAgAEQAaQByAGUAYwB0AG8AcgB5AA==|SQBuAGQAZQB4AFQAZQBzAHQA|RwBBAF8AUABvAGwAaQBjAHkARABpAHMAcABsAGEAeQBOAGEAbQBlAA==|RwBBAF8AUAByAGkAbQBhAHIAeQBTAGkAdABlAEMAbwBsAGwAZQBjAHQAaQBvAG4AQwBvAG4AdABhAGMAdAA=|RwBBAF8AUwBlAGMAbwBuAGQAYQByAHkAUwBpAHQAZQBDAG8AbABsAGUAYwB0AGkAbwBuAEMAbwBuAHQAYQBjAHQA|SQBuAGQAZQB4ADEA|";

            var keys = indexPropertyKeys.Split('|');


            foreach (var key in keys)
            {
                var value = Encoding.Unicode.GetString(Convert.FromBase64String(key));
            }


        }
    }
}
