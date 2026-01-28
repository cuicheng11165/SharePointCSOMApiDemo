using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using Microsoft.SharePoint.Client;
using CSOM.Common;

namespace CSOM_View_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            var siteUrl = EnvConfig.GetSiteUrl("");
            using (ClientContext clientContext = new ClientContext(siteUrl))
            {
                clientContext.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
                {
                    e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                        EnvConfig.GetCsomToken();
                };
              

                var currentWeb = clientContext.Web;

                List list = currentWeb.Lists.GetById(new Guid("2d2d3e92-add6-4070-9117-837f21eb71a0"));

                clientContext.Load(list, l => l.Views.IncludeWithDefaultProperties(v => v.ViewFields));

                clientContext.ExecuteQuery();

                var viewFile = clientContext.Web.GetFileByServerRelativeUrl(list.Views[0].ServerRelativeUrl);
                clientContext.Load(viewFile, v => v.ETag);

                clientContext.ExecuteQuery();

            }
        }
    }
}
