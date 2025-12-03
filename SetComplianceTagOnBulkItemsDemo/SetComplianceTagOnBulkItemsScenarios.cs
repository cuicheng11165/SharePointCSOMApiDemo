using System;
using System.Collections.Generic;
using CSOM.Common;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.CompliancePolicy;

namespace SetComplianceTagOnBulkItemsDemo
{
    internal static class SetComplianceTagOnBulkItemsScenarios
    {
        /// <summary>
        /// Demonstrates how to call the CSOM entry point for applying a compliance tag
        /// to multiple files in a single request using SetComplianceTagOnBulkItems.SetComplianceTagOnBulkItems.
        /// </summary>
        internal static void DemoSetComplianceTagOnBulkItems()
        {
            var siteUrl = EnvConfig.GetSiteUrl("sites/YourTeamSite");

            using ClientContext context = new ClientContext(siteUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            context.Load(context.Web, w => w.Title);
            context.ExecuteQuery();

            Console.WriteLine($"Connected to {context.Web.Title} at {siteUrl}");

            // Each entry describes the item to label and the compliance tag metadata.
            // The anonymous type keeps the sample self-contained while matching the
            // expected shape of the bulk API payload.
            var bulkItems = new List<object>
            {
                new
                {
                    // Absolute URL to the list item or file you want to label.
                    Url = $"{siteUrl}/Shared%20Documents/Example.docx",
                    ComplianceTagValue = "Demo Retention Label",
                    BlockDelete = true,
                    BlockEdit = false,
                    SyncToItems = true,
                    // Optional justification/comment fields accepted by the API.
                    Comments = "Applied via sample bulk call",
                    TagTime = DateTimeOffset.UtcNow
                }
            };

            // Invoke the bulk labeling API. Using dynamic arguments avoids compile-time
            // coupling to private request types while still exercising the CSOM entry point.
            var result = SetComplianceTagOnBulkItems.SetComplianceTagOnBulkItems(
                context,
                bulkItems);

            context.ExecuteQuery();

            Console.WriteLine($"Submitted bulk compliance tag request for {bulkItems.Count} item(s).");
        }
    }
}
