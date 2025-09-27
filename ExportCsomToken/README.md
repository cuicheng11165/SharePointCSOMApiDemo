# ExportCsomToken 示例说明

本工程演示如何在 SharePoint CSOM 场景下获取并导出访问令牌，涉及以下关键 API：

- 使用 `Microsoft.Identity.Client.ConfidentialClientApplicationBuilder` 配合证书指纹，调用 `AcquireTokenForClient` 获取针对 SharePoint Online 的应用专用访问令牌（`https://{tenant}.sharepoint.com/.default`）。
- 通过 `System.Security.Cryptography.X509Certificates.X509Store` 在当前用户证书存储中查找指定指纹的证书。
- 将获取到的 Bearer Token 写入配置文件，同时在 `ClientContext` 的 `ExecutingWebRequest` 事件中注入 `Authorization` 头，实现 CSOM 在无用户名密码情况下的应用身份认证。
