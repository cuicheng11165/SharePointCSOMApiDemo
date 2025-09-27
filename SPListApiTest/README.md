# SPListApiTest 示例说明

本工程展示了如何在 CSOM 中使用现有的访问令牌连接 SharePoint 站点并读取站点信息，主要步骤包括：

- 通过 `EnvConfig.GetSiteUrl`/`GetCsomToken`（或 `GetToken`）加载配置的站点地址与 Bearer Token。
- 使用 `ClientContext.ExecutingWebRequest` 事件手动向每个请求写入 `Authorization` 请求头，实现无凭据的令牌认证。
- 调用 `ClientContext.Load(context.Web)` 并执行 `ExecuteQuery`，获取站点标题等基础属性。

该示例可作为基于 CSOM 的列表/站点后续操作的认证模板。
