using System;
using CSOM.Common;
using RestApiTest.ShareLinkApi;

namespace RestApiTest
{
    internal static class RestApiScenarios
    {
        internal static void CreateShareLink()
        {
            var siteUrl = EnvConfig.GetSiteUrl("/contentstorage/x8FNO-xtskuCRX2_fMTHLT17vJaIE59ArxPpSSZt3Zw");

            var listId = new Guid("0c240898-1266-4f7a-987e-9992aabb8e7d");

            const int itemId = 5;

            new ShareLink().CreateShareLink(siteUrl, listId, itemId);
        }
    }
}
