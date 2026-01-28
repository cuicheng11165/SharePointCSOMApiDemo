using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.SharePoint.Client;
using CSOM.Common;

namespace UnifiedCsomTests.Scenarios
{
    internal static class UpdateConflictScenarios
    {
        internal static void Run()
        {
            // Ensure you have configured the correct site URL in Config/HostName.txt 
            // and that the site /sites/DC exists or update the path below.
            string siteUrl = EnvConfig.GetSiteUrl("/sites/DC");
            
            using (ClientContext context = new ClientContext(siteUrl))
            {
                context.ExecutingWebRequest += (sender, e) =>
                {
                    e.WebRequestExecutor.RequestHeaders["Authorization"] = EnvConfig.GetCsomToken();
                };

                var web = context.Web;
                // Note: The file path might need adjustment based on the environment
                // Ensure this file exists in the site
                var file1 = web.GetFileByServerRelativeUrl("/sites/DC/Test Document Id/ddd.docx");

                file1.CheckOut();

                file1.ListItemAllFields["FileLeafRef"] = "ddd.docx";
                // Note: Setting Editor/Author by ID (1) might fail if user 1 doesn't exist or isn't valid.
                // In a real scenario, you might want to look up a user.
                file1.ListItemAllFields["Editor"] = 1;
                file1.ListItemAllFields["Author"] = 1;
                file1.ListItemAllFields["Modified"] = DateTime.UtcNow.AddDays(-1);
                file1.ListItemAllFields.Update();

                file1.CheckIn("", CheckinType.OverwriteCheckIn);

                file1.ListItemAllFields["FileLeafRef"] = "ddd.docx";
                file1.ListItemAllFields["Editor"] = 1;
                file1.ListItemAllFields["Author"] = 1;
                file1.ListItemAllFields["Modified"] = DateTime.Now.AddYears(1);

                file1.ListItemAllFields.Update();

                try 
                {
                    context.ExecuteQuery();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception expected: " + ex.Message);
                }
            }
        }

        public static void Update(Microsoft.SharePoint.Client.File file, string checkInComment, bool keepVersion)
        {
            MethodInfo updateMethod = typeof(ListItem).GetMethod("ValidateUpdateListItem", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
            string fileLeafRef = file.ListItemAllFields.FieldValues.ContainsKey("FileLeafRef") ? file.ListItemAllFields["FileLeafRef"] as string : string.Empty;
            IList<ListItemFormUpdateValue> values = new List<ListItemFormUpdateValue>();
            values.Add(new ListItemFormUpdateValue() { FieldName = "FileLeafRef", FieldValue = fileLeafRef });
            if (updateMethod.GetParameters().Length == 3)
            {
                updateMethod.Invoke(file.ListItemAllFields, new object[] { values, keepVersion, checkInComment });
            }
            else
            {
                updateMethod.Invoke(file.ListItemAllFields, new object[] { values, keepVersion });
            }
        }
    }
}
