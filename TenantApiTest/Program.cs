using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using CSOM.Common;

namespace TenantApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestCGetSPOContainerTypes();
        }

        private static void TestCGetSPOContainerTypes()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);

            context.Load(context.Web);
            context.ExecuteQuery();

            Tenant tenant = new Tenant(context);

            var containerTypes1 = tenant.GetSPOContainerTypes(SPContainerTypeTenantType.OwningTenant);
            context.ExecuteQuery();

            var containerTypes2 = tenant.GetSPOContainerTypes(SPContainerTypeTenantType.ConsumingTenant);
            context.ExecuteQuery();
        }


        private static void TestGetSPOContainersByApplicationId()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);


            context.Load(context.Web);
            context.ExecuteQuery();

            Tenant tenant = new Tenant(context);


            var containers = tenant.GetSPOContainersByApplicationId(new Guid("a187e399-0c36-4b98-8f04-1edc167a0996"),
                false, "", SPContainerArchiveStatusFilterProperties.NotArchived);

            context.ExecuteQuery();

            foreach (var c in containers.Value.ContainerCollection)
            {
                var re = tenant.GetSPOContainerByContainerId(c.ContainerId);

                context.ExecuteQuery();
            }
        }

        private static void TestDenyAddAndCustomizePages()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);
            Tenant tenant = new Tenant(context);

            context.Load(context.Web);
            context.ExecuteQuery();


            var url = "https://bigapp.sharepoint.com/sites/waltTestCreateSiteCollection2";


            var siteProperties = tenant.GetSitePropertiesByUrl(url, false);
            context.Load(siteProperties);
            context.ExecuteQuery();

            siteProperties.DenyAddAndCustomizePages = DenyAddAndCustomizePagesStatus.Disabled;
            siteProperties.Update();

        
            context.ExecuteQuery();
        }

        private static void TestGetHubSitesProperties()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);
            Tenant tenant = new Tenant(context);

            context.Load(context.Web);
            context.ExecuteQuery();
            
            var hubSiteProperties = tenant.GetHubSitesProperties();
            
            context.Load(hubSiteProperties);
            context.ExecuteQuery();

            foreach (var site in hubSiteProperties)
            {
                var url = site.SiteUrl;
                var property = tenant.GrantHubSiteRights(url, new[] { "DL_GA_DEV1" }, SPOHubSiteUserRights.Join);
                context.ExecuteQuery();
            }
        }


        private static void TestContainer()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);


            context.Load(context.Web);
            context.ExecuteQuery();

            Tenant tenant = new Tenant(context);


            var containers = tenant.GetSPOContainersByApplicationId(new Guid("a187e399-0c36-4b98-8f04-1edc167a0996"),
                false, "", SPContainerArchiveStatusFilterProperties.NotArchived);

            context.ExecuteQuery();

            foreach (var c in containers.Value.ContainerCollection)
            {
                var re = tenant.GetSPOContainerByContainerId(c.ContainerId);

                context.ExecuteQuery();
            }
        }

        private static void Context_ExecutingWebRequest(object sender, WebRequestEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}