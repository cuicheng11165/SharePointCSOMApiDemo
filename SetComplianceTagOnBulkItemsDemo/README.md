# SetComplianceTagOnBulkItemsDemo

该示例项目参考了 `TenantApiTest` 的结构，演示如何通过 `SetComplianceTagOnBulkItems.SetComplianceTagOnBulkItems` 方法一次性为多个 SharePoint Online 列表项/文件设置合规性标签。

## 配置
1. 在仓库根目录的 `Config` 文件夹中准备以下文件，并填入可用值：
   - `HostName.txt`：租户主机名，例如 `contoso.sharepoint.com`。
   - `CSOMAuthorization.txt`：已获取的 CSOM 授权标头（格式为 `Bearer eyJ...`）。
   - 其他文件（如 `ClientId.txt` 等）可复用现有示例的配置方式。

2. 根据你的站点更新 `SetComplianceTagOnBulkItemsScenarios.cs` 中的站点路径与目标文件 URL。

## 运行
在具备 .NET SDK 的环境下执行：

```bash
dotnet run --project SetComplianceTagOnBulkItemsDemo/SetComplianceTagOnBulkItemsDemo.csproj
```

程序会：
1. 使用 `EnvConfig` 从配置文件读取租户信息并建立 `ClientContext`；
2. 调用 `SetComplianceTagOnBulkItems.SetComplianceTagOnBulkItems` 将示例标签应用到演示文件；
3. 在控制台输出已提交的批量标签请求数量。
