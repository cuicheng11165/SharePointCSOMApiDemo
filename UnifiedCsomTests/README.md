# Unified CSOM Tests

## 项目概述
这是一个统一的 SharePoint CSOM API 测试控制台应用程序，整合了原来分散在多个独立项目中的所有 CSOM 测试场景。

## 整合的项目
本项目整合了以下原始项目的测试场景：

1. **CSOM File Add Test** - 文件添加和操作场景
2. **Permission** - 权限管理场景
3. **TenantApiTest** - Tenant API 测试场景
4. **SPListApiTest** - 列表 API 测试场景
5. **SetColumnDefaultValue** - 列默认值设置场景
6. **ListSPEmbeddedContains** - Container 相关场景
7. **SetComplianceTagOnBulkItemsDemo** - 合规标签批量设置
8. **UpdateConflictSample** - 更新冲突测试
9. **CSOM_View_Test** - 视图测试
10. **CSOM_ExceptionHandlingScope_Test** - 异常处理范围测试
11. **TimeZoneTest** - 时区测试
12. **CamlQueryTest** - CAML 查询测试
13. **Taxonomy API** - 分类法 API 测试
14. **UpdateWebAllProperties** - Web 属性更新测试

## 项目结构
```
UnifiedCsomTests/
├── Program.cs                              # 主程序，包含交互式菜单系统
├── UnifiedCsomTests.csproj                # 项目文件
├── Scenarios/                             # 所有测试场景类
│   ├── FileAddScenarios.cs               # 文件操作场景
│   ├── PermissionScenarios.cs            # 权限管理场景
│   ├── TenantApiScenarios.cs             # Tenant API 场景
│   ├── SPListApiScenarios.cs             # 列表 API 场景
│   ├── SetColumnDefaultValueScenarios.cs # 列默认值场景
│   ├── ContainerScenarios.cs             # Container 场景
│   ├── ComplianceTagScenarios.cs         # 合规标签场景
│   └── UpdateConflictScenarios.cs        # 更新冲突场景
└── README.md                              # 本文件
```

## 功能特性

### 交互式菜单系统
程序提供了友好的命令行交互界面，包含以下主菜单选项：

1. **文件操作场景**
   - 使用字节数组添加文件
   - 使用流添加文件
   - 使用流添加大文件
   - 使用 SaveBytes 添加文件
   - 使用 SaveStream 添加文件
   - 使用分块上传大文件
   - 更新托管元数据默认值

2. **权限管理场景**
   - 创建默认组
   - 获取用户有效权限
   - 获取贡献者角色定义
   - 检查权限位

3. **Tenant API 场景**
   - 获取容器类型
   - 获取应用容器
   - 设置网站禁止自定义页面
   - 获取 Hub 站点属性
   - 测试容器 API

4. **列表 API 场景**
   - 打印站点标题
   - 设置列默认值并添加文件

5. **Container 场景**
   - 导出应用容器信息

6. **其他场景**
   - 合规标签批量设置
   - 更新冲突测试

## 配置要求

### 前置条件
1. 确保 `Config/` 目录存在并包含以下配置文件：
   - `HostName.txt` - SharePoint 主机名
   - `ClientId.txt` - 应用程序客户端 ID
   - `TenantId.txt` - 租户 ID
   - `Certificate.pfx` - 证书文件
   - `CertificatePassword.txt` - 证书密码
   - `CSOMAuthorization.txt` - CSOM 授权 Token（通过 ExportCsomToken 生成）

### 依赖项
- .NET 8.0
- Microsoft.SharePointOnline.CSOM (16.1.27518.12000)
- Microsoft.Windows.Compatibility (8.0.0)
- Configuration 项目（共享配置库）

## 使用方法

### 1. 生成认证 Token
在运行测试之前，需要先生成 CSOM 授权 Token：
```bash
cd ExportCsomToken
dotnet run
```
这将在 `Config/CSOMAuthorization.txt` 中生成 Bearer Token。

### 2. 运行统一测试程序
```bash
cd UnifiedCsomTests
dotnet run
```

### 3. 使用交互式菜单
程序启动后，按照屏幕提示选择要测试的场景：
- 输入数字选择主菜单项
- 在子菜单中选择具体的测试场景
- 某些场景需要输入参数（如站点 URL、用户登录名等）
- 测试完成后按任意键返回菜单
- 输入 0 返回上一级或退出程序

### 4. 示例操作流程
```
1. 启动程序
2. 选择 "1. 文件操作场景"
3. 选择 "1. 使用字节数组添加文件"
4. 等待操作完成
5. 按任意键返回菜单
6. 选择其他场景或输入 0 退出
```

## 优势

### 相比原始分散项目的优势：
1. **统一入口** - 一个控制台程序访问所有测试场景
2. **易于导航** - 交互式菜单系统，清晰的场景分类
3. **减少重复** - 共享配置和通用代码
4. **易于维护** - 集中管理所有 CSOM 测试代码
5. **一致的模式** - 所有场景使用统一的 EnvConfig 配置模式
6. **更好的组织** - 按功能模块组织的 Scenarios 文件夹

## 开发指南

### 添加新场景
1. 在 `Scenarios/` 文件夹中创建新的场景类
2. 使用 `internal static class` 和 `internal static` 方法
3. 使用 `EnvConfig.GetSiteUrl()` 或 `EnvConfig.GetAdminCenterUrl()` 获取 URL
4. 使用 `EnvConfig.GetCsomToken()` 获取认证 Token
5. 在 `Program.cs` 的相应菜单中添加新场景的调用

### 场景类模板
```csharp
using System;
using CSOM.Common;
using Microsoft.SharePoint.Client;

namespace UnifiedCsomTests.Scenarios
{
    internal static class YourScenarios
    {
        internal static void YourMethod()
        {
            var siteUrl = EnvConfig.GetSiteUrl("/sites/yoursite");
            using ClientContext context = new ClientContext(siteUrl);

            context.ExecutingWebRequest += (object? sender, WebRequestEventArgs e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers[System.Net.HttpRequestHeader.Authorization] =
                    EnvConfig.GetCsomToken();
            };

            // 你的测试代码
            context.Load(context.Web);
            context.ExecuteQuery();
            
            Console.WriteLine($"完成: {context.Web.Title}");
        }
    }
}
```

## 注意事项
1. 运行测试前确保已配置好 Config 目录中的所有文件
2. 某些场景需要特定的 SharePoint 站点结构和权限
3. Token 有时效性，过期后需要重新运行 ExportCsomToken
4. 某些场景可能需要调整硬编码的列表名、字段名等
5. 建议在测试环境中运行，避免影响生产数据

## 故障排除

### 常见问题
1. **认证失败** - 检查 CSOMAuthorization.txt 是否存在且未过期
2. **站点不存在** - 修改场景代码中的站点相对路径
3. **列表/字段不存在** - 根据实际环境调整场景中的列表和字段名称
4. **权限不足** - 确保证书关联的应用程序具有足够的权限

## 许可证
与父项目保持一致

## 贡献
欢迎提交 Pull Request 添加新的测试场景或改进现有功能
