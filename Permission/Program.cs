using System;
using CSOM.Common;

namespace Permission
{
    class Program
    {
        static void Main(string[] args)
        {
            string siteUrl = EnvConfig.GetSiteUrl("/sites/simmon1456");
            string userLoginName = "i:0#.f|membership|simmon@baron.space";
            CheckPermissionScenarios.CreateDefaultGroups(siteUrl, userLoginName);
            Console.WriteLine("Permission check completed.");
            Console.ReadLine();
        }
    }
}
