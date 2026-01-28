using System;
using UnifiedCsomTests.Scenarios;

namespace UnifiedCsomTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===============================================");
            Console.WriteLine("   SharePoint CSOM API 统一测试控制台");
            Console.WriteLine("===============================================\n");

            while (true)
            {
                DisplayMainMenu();
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            FileOperationsMenu();
                            break;
                        case "2":
                            PermissionMenu();
                            break;
                        case "3":
                            TenantApiMenu();
                            break;
                        case "4":
                            ListApiMenu();
                            break;
                        case "5":
                            ContainerMenu();
                            break;
                        case "6":
                            ViewMenu();
                            break;
                        case "7":
                            ExceptionHandlingMenu();
                            break;
                        case "8":
                            CamlQueryMenu();
                            break;
                        case "9":
                            TimeZoneMenu();
                            break;
                        case "10":
                            TaxonomyMenu();
                            break;
                        case "11":
                            WebPropertiesMenu();
                            break;
                        case "12":
                            OtherScenariosMenu();
                            break;
                        case "0":
                            Console.WriteLine("\n退出程序...");
                            return;
                        default:
                            Console.WriteLine("\n无效选择，请重试。\n");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n错误: {ex.Message}");
                    Console.WriteLine($"详细信息: {ex.StackTrace}\n");
                }

                Console.WriteLine("\n按任意键继续...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("主菜单:");
            Console.WriteLine("  1. 文件操作场景");
            Console.WriteLine("  2. 权限管理场景");
            Console.WriteLine("  3. Tenant API 场景");
            Console.WriteLine("  4. 列表 API 场景");
            Console.WriteLine("  5. Container 场景");
            Console.WriteLine("  6. 视图操作场景");
            Console.WriteLine("  7. 异常处理场景");
            Console.WriteLine("  8. CAML 查询场景");
            Console.WriteLine("  9. 时区测试场景");
            Console.WriteLine("  10. 托管元数据场景");
            Console.WriteLine("  11. Web 属性场景");
            Console.WriteLine("  12. 其他场景");
            Console.WriteLine("  0. 退出");
            Console.Write("\n请选择: ");
        }

        static void FileOperationsMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 文件操作场景 ===\n");
            Console.WriteLine("  1. 使用字节数组添加文件 (AddFileWithBytes)");
            Console.WriteLine("  2. 使用流添加文件 (AddFileWithStream)");
            Console.WriteLine("  3. 使用流添加大文件 (AddLargeFileWithStream)");
            Console.WriteLine("  4. 使用 SaveBytes 添加文件 (AddFileWithSaveBytes)");
            Console.WriteLine("  5. 使用 SaveStream 添加文件 (AddFileWithSaveStream)");
            Console.WriteLine("  6. 使用分块上传 (AddFileWithContinueUpload)");
            Console.WriteLine("  7. 更新托管元数据默认值 (UpdateManagedMetadataDefaultValue)");
            Console.WriteLine("  0. 返回主菜单");
            Console.Write("\n请选择: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    FileAddScenarios.AddFileWithBytes();
                    break;
                case "2":
                    FileAddScenarios.AddFileWithStream();
                    break;
                case "3":
                    FileAddScenarios.AddLargeFileWithStream();
                    break;
                case "4":
                    FileAddScenarios.AddFileWithSaveBytes();
                    break;
                case "5":
                    FileAddScenarios.AddFileWithSaveStream();
                    break;
                case "6":
                    FileAddScenarios.AddFileWithContinueUpload();
                    break;
                case "7":
                    FileAddScenarios.UpdateManagedMetadataDefaultValue();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("无效选择");
                    break;
            }
        }

        static void PermissionMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 权限管理场景 ===\n");
            Console.WriteLine("请输入站点 URL (相对路径，如 /sites/yoursite):");
            var siteRelative = Console.ReadLine() ?? "/sites/simmon1456";
            
            Console.WriteLine("请输入用户登录名 (如 i:0#.f|membership|user@domain.com):");
            var userLogin = Console.ReadLine() ?? "i:0#.f|membership|simmon@baron.space";

            Console.WriteLine("\n  1. 创建默认组");
            Console.WriteLine("  2. 获取用户有效权限");
            Console.WriteLine("  3. 获取贡献者角色定义");
            Console.WriteLine("  0. 返回主菜单");
            Console.Write("\n请选择: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            var siteUrl = CSOM.Common.EnvConfig.GetSiteUrl(siteRelative);

            switch (choice)
            {
                case "1":
                    PermissionScenarios.CreateDefaultGroups(siteUrl, userLogin);
                    break;
                case "2":
                    var perms = PermissionScenarios.GetUserEffectivePermissions(siteUrl, userLogin);
                    Console.WriteLine($"权限值: {perms}");
                    break;
                case "3":
                    var role = PermissionScenarios.GetContributorRoleDefinition(siteUrl);
                    Console.WriteLine($"角色: {role.Name}");
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("无效选择");
                    break;
            }
        }

        static void TenantApiMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Tenant API 场景 ===\n");
            Console.WriteLine("  1. 获取容器类型 (GetSPOContainerTypes)");
            Console.WriteLine("  2. 获取应用容器 (GetSPOContainersByApplicationId)");
            Console.WriteLine("  3. 设置网站禁止自定义页面 (DenyAddAndCustomizePages)");
            Console.WriteLine("  4. 获取 Hub 站点属性 (GetHubSitesProperties)");
            Console.WriteLine("  5. 测试容器 API (TestContainer)");
            Console.WriteLine("  0. 返回主菜单");
            Console.Write("\n请选择: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    TenantApiScenarios.TestCGetSPOContainerTypes();
                    break;
                case "2":
                    TenantApiScenarios.TestGetSPOContainersByApplicationId();
                    break;
                case "3":
                    TenantApiScenarios.TestDenyAddAndCustomizePages();
                    break;
                case "4":
                    TenantApiScenarios.TestGetHubSitesProperties();
                    break;
                case "5":
                    TenantApiScenarios.TestContainer();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("无效选择");
                    break;
            }
        }

        static void ListApiMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 列表 API 场景 ===\n");
            Console.WriteLine("  1. 打印站点标题 (PrintSiteTitle)");
            Console.WriteLine("  2. 设置列默认值并添加文件 (SetDefaultValueAndAddFile)");
            Console.WriteLine("  0. 返回主菜单");
            Console.Write("\n请选择: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    SPListApiScenarios.PrintSiteTitle();
                    break;
                case "2":
                    SetColumnDefaultValueScenarios.SetDefaultValueAndAddFile();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("无效选择");
                    break;
            }
        }

        static void ContainerMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Container 场景 ===\n");
            Console.WriteLine("  1. 导出应用容器信息 (DumpContainersByApplicationId)");
            Console.WriteLine("  0. 返回主菜单");
            Console.Write("\n请选择: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ContainerScenarios.DumpContainersByApplicationId();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("无效选择");
                    break;
            }
        }

        static void OtherScenariosMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 其他场景 ===\n");
            Console.WriteLine("  1. 合规标签批量设置 (SetComplianceTagOnBulkItems)");
            Console.WriteLine("  2. 更新冲突测试 (UpdateConflict)");
            Console.WriteLine("  0. 返回主菜单");
            Console.Write("\n请选择: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ComplianceTagScenarios.DemoSetComplianceTagOnBulkItems();
                    break;
                case "2":
                    UpdateConflictScenarios.Run();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("无效选择");
                    break;
            }
        }

        static void ViewMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 视图操作场景 ===\n");
            Console.WriteLine("  1. 测试视图和视图字段 (TestViewAndViewFields)");
            Console.WriteLine("  0. 返回主菜单");
            Console.Write("\n请选择: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ViewScenarios.TestViewAndViewFields();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("无效选择");
                    break;
            }
        }

        static void ExceptionHandlingMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 异常处理场景 ===\n");
            Console.WriteLine("  1. 测试 Try/Catch 文件夹创建 (TestTryCatchFolderCreation)");
            Console.WriteLine("  0. 返回主菜单");
            Console.Write("\n请选择: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ExceptionHandlingScopeScenarios.TestTryCatchFolderCreation();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("无效选择");
                    break;
            }
        }

        static void CamlQueryMenu()
        {
            Console.Clear();
            Console.WriteLine("=== CAML 查询场景 ===\n");
            Console.WriteLine("  1. 基本 CAML 查询 (BasicCamlQuery)");
            Console.WriteLine("  2. 分页 CAML 查询 (PaginatedCamlQuery)");
            Console.WriteLine("  3. 创建所有项查询 (CreateAllItemsQuery)");
            Console.WriteLine("  0. 返回主菜单");
            Console.Write("\n请选择: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    CamlQueryScenarios.BasicCamlQuery();
                    break;
                case "2":
                    CamlQueryScenarios.PaginatedCamlQuery();
                    break;
                case "3":
                    CamlQueryScenarios.CreateAllItemsQuery();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("无效选择");
                    break;
            }
        }

        static void TimeZoneMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 时区测试场景 ===\n");
            Console.WriteLine("  1. 测试文档时区 (TestClientAPI_Document)");
            Console.WriteLine("  2. 测试列表项时区 (TestClientAPI_ListItem)");
            Console.WriteLine("  0. 返回主菜单");
            Console.Write("\n请选择: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    TimeZoneScenarios.TestClientAPI_Document();
                    break;
                case "2":
                    TimeZoneScenarios.TestClientAPI_ListItem();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("无效选择");
                    break;
            }
        }

        static void TaxonomyMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 托管元数据场景 ===\n");
            Console.WriteLine("  1. 创建组、术语集和术语 (CreateGroupTermSetAndTerms)");
            Console.WriteLine("  2. 列出术语集中的术语 (ListTermsInTermSet)");
            Console.WriteLine("  3. 按名称获取术语集 (GetTermSetByName)");
            Console.WriteLine("  4. 创建托管元数据字段 (CreateManagedMetadataField)");
            Console.WriteLine("  0. 返回主菜单");
            Console.Write("\n请选择: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    TaxonomyScenarios.CreateGroupTermSetAndTerms();
                    break;
                case "2":
                    TaxonomyScenarios.ListTermsInTermSet();
                    break;
                case "3":
                    TaxonomyScenarios.GetTermSetByName();
                    break;
                case "4":
                    TaxonomyScenarios.CreateManagedMetadataField();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("无效选择");
                    break;
            }
        }

        static void WebPropertiesMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Web 属性场景 ===\n");
            Console.WriteLine("  1. 更新 Web AllProperties (UpdateWebAllProperties)");
            Console.WriteLine("  2. 列出所有 Web AllProperties (ListWebAllProperties)");
            Console.WriteLine("  3. 切换禁止自定义页面状态 (ToggleDenyAddAndCustomizePages)");
            Console.WriteLine("  4. 解码索引属性键 (DecodeIndexPropertyKeys)");
            Console.WriteLine("  0. 返回主菜单");
            Console.Write("\n请选择: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    UpdateWebPropertiesScenarios.UpdateWebAllProperties();
                    break;
                case "2":
                    UpdateWebPropertiesScenarios.ListWebAllProperties();
                    break;
                case "3":
                    UpdateWebPropertiesScenarios.ToggleDenyAddAndCustomizePages();
                    break;
                case "4":
                    UpdateWebPropertiesScenarios.DecodeIndexPropertyKeys();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("无效选择");
                    break;
            }
        }
    }
}