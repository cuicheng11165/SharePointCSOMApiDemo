# ExportGraphToken 示例说明

本工程演示通过应用证书方式获取 Microsoft Graph 的应用权限访问令牌，并发起测试调用，包含以下关键 API：

- 使用 `ConfidentialClientApplicationBuilder.WithCertificate` 构建机密客户端，并调用 `AcquireTokenForClient` 请求 `https://graph.microsoft.com/.default` 范围的访问令牌。
- 借助 `X509Store` 在用户或本地计算机证书存储中查找指定指纹的证书。
- 使用 `HttpClient` 设置 `Authorization: Bearer` 头访问 `https://graph.microsoft.com/v1.0/groups`，演示如何在获取令牌后调用 Graph API。
