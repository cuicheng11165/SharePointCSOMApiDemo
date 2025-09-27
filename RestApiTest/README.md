# RestApiTest 示例说明

本工程演示了以下 SharePoint/Graph 相关 API 的用法：

- 使用自定义 `RestApibase` 基类配合 `HttpClient` 向 SharePoint Online 发送 REST POST 请求，演示如何设置 `Authorization` 头和 `Accept`/`Content-Type` 等请求头，以调用站点的 `_api/web/.../ShareLink` 接口。 
- 在 `ShareLink` 示例中，构造创建共享链接所需的 JSON 负载，并调用 `/_api/web/Lists(@a1)/GetItemById(@a2)/ShareLink` 端点生成文档共享链接。 
- 通过 `EnvConfig.GetSiteUrl` 读取配置，结合 `Guid` 列表和项标识演示 REST API 与 CSOM 配置的结合使用。 

运行示例前，需要在 `Config` 目录中准备好 SharePoint Online 访问令牌（`CSOMAuthorization.txt`），以便通过 Bearer Token 完成认证。
