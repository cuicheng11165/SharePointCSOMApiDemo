using System;
using System.Collections.Generic;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using CSOM.Common;

namespace TenantApiTest
{
    internal static class TenantApiScenarios
    {
        internal static void TestCGetSPOContainerTypes()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            context.Load(context.Web);
            context.ExecuteQuery();

            Tenant tenant = new Tenant(context);

            var containerTypes1 = tenant.GetSPOContainerTypes(SPContainerTypeTenantType.OwningTenant);
            context.ExecuteQuery();

            var containerTypes2 = tenant.GetSPOContainerTypes(SPContainerTypeTenantType.ConsumingTenant);
            context.ExecuteQuery();
        }

        internal static void TestGetSPOContainersByApplicationId()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };


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

        internal static void TestDenyAddAndCustomizePages()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };


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

        internal static void TestGetHubSitesProperties()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

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

        internal static void TestContainer()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };


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

        internal static void Context_ExecutingWebRequest(object sender, WebRequestEventArgs e)
        {
            throw new NotImplementedException();
        }

        // Demonstrates fetching whether the tenant public CDN is enabled
        internal static void DemoGetTenantCdnEnabled()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);
            Tenant tenant = new Tenant(context);

            var enabledResult = tenant.GetTenantCdnEnabled(SPOTenantCdnType.Public);
            context.ExecuteQuery();

            Console.WriteLine($"Public CDN enabled: {enabledResult.Value}");
        }

        // Demonstrates retrieving the tenant public CDN origin list
        internal static void DemoGetTenantCdnOrigins()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };


            Tenant tenant = new Tenant(context);

            IList<string> origins = tenant.GetTenantCdnOrigins(SPOTenantCdnType.Public);
            context.ExecuteQuery();

            foreach (var origin in origins)
            {
                Console.WriteLine($"Public CDN origin: {origin}");
            }
        }

        // Demonstrates retrieving the tenant public CDN policy collection
        internal static void DemoGetTenantCdnPolicies()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);
            Tenant tenant = new Tenant(context);

            IList<string> policies = tenant.GetTenantCdnPolicies(SPOTenantCdnType.Public);
            context.ExecuteQuery();

            foreach (var policy in policies)
            {
                Console.WriteLine($"Public CDN policy: {policy}");
            }
        }

        // Demonstrates retrieving site properties by site ID
        internal static void DemoGetSitePropertiesBySiteId()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);
            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            Tenant tenant = new Tenant(context);

            var siteId = new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

            var siteProperties = tenant.GetSitePropertiesBySiteId(siteId, true);
            context.Load(siteProperties);
            context.ExecuteQuery();

            Console.WriteLine($"Site URL: {siteProperties.Url}");
        }

        // Demonstrates listing all web templates available to the tenant
        internal static void DemoGetSPOTenantAllWebTemplates()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            Tenant tenant = new Tenant(context);

            var templates = tenant.GetSPOTenantAllWebTemplates();
            context.Load(templates);
            context.ExecuteQuery();

            foreach (var template in templates)
            {
                Console.WriteLine($"Template: {template.Name} - {template.Title}");
            }
        }

        // Demonstrates retrieving site properties by site URL
        internal static void DemoGetSitePropertiesByUrl()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);
            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            Tenant tenant = new Tenant(context);

            var siteUrl = "https://contoso.sharepoint.com/sites/demo";

            try
            {
                var siteProperties = tenant.GetSitePropertiesByUrl(siteUrl, true);
                context.Load(siteProperties);
                context.ExecuteQuery();

                Console.WriteLine($"Site {siteProperties.Url} status: {siteProperties.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DemoGetSitePropertiesByUrl failed: {ex.Message}");
            }
        }

        // Demonstrates inspecting deleted site information by URL
        internal static void DemoGetDeletedSitePropertiesByUrl()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);
            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            Tenant tenant = new Tenant(context);

            var deletedUrl = "https://contoso.sharepoint.com/sites/deleted";

            try
            {
                var deletedSite = tenant.GetDeletedSitePropertiesByUrl(deletedUrl);
                context.Load(deletedSite);
                context.ExecuteQuery();

                Console.WriteLine($"Deleted site url: {deletedSite.Url}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DemoGetDeletedSitePropertiesByUrl failed: {ex.Message}");
            }
        }

        // Demonstrates enumerating deleted sites across the tenant
        internal static void DemoGetDeletedSiteProperties()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);
            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            Tenant tenant = new Tenant(context);

            try
            {
                var deletedSites = tenant.GetDeletedSiteProperties(50);
                context.Load(deletedSites);
                context.ExecuteQuery();

                foreach (var site in deletedSites)
                {
                    Console.WriteLine($"Deleted site: {site.Url}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DemoGetDeletedSiteProperties failed: {ex.Message}");
            }
        }

        // Demonstrates adding a role assignment to an SPO container
        internal static void DemoAddSPOContainerRole()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);
            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            Tenant tenant = new Tenant(context);

            try
            {
                tenant.AddSPOContainerRole("containerId", "loginName", "owner");

                context.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DemoGetDeletedSiteProperties failed: {ex.Message}");
            }
        }

        // Demonstrates encoding a user identity via the Tenant API
        internal static void DemoEncodeClaim()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext context = new ClientContext(adminCenterUrl);
            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            Tenant tenant = new Tenant(context);

            try
            {
                var encodedResult = tenant.EncodeClaim("user@contoso.com");
                context.ExecuteQuery();

                Console.WriteLine($"Encoded claim: {encodedResult.Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DemoEncodeClaim failed: {ex.Message}");
            }
        }

        // Demonstrates decoding a user identity via the Tenant API
        internal static void DemoDecodeClaim()
        {
            var adminCenterUrl = EnvConfig.GetAdminCenterUrl();

            ClientContext encodeContext = new ClientContext(adminCenterUrl);
            encodeContext.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            Tenant encodeTenant = new Tenant(encodeContext);

            try
            {
                var encodedResult = encodeTenant.EncodeClaim("user@contoso.com");
                encodeContext.ExecuteQuery();

                ClientContext decodeContext = new ClientContext(adminCenterUrl);
                decodeContext.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
                {
                    e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                        EnvConfig.GetCsomToken();
                };

                Tenant decodeTenant = new Tenant(decodeContext);

                var decodedResult = decodeTenant.DecodeClaim(encodedResult.Value);
                decodeContext.ExecuteQuery();

                Console.WriteLine($"Decoded claim: {decodedResult.Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DemoDecodeClaim failed: {ex.Message}");
            }
        }
    }
}
