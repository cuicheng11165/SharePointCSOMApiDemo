# ExportGraphDelegateToken 示例说明

本工程演示如何通过用户名密码（ROPC）流程获取 Microsoft Graph 的委托访问令牌，并调用 Graph API，主要包含：

- 使用 `PublicClientApplicationBuilder.Create` 构建公共客户端，并通过 `AcquireTokenByUsernamePassword` 获取带有 `User.Read` 权限的访问令牌。
- 将纯文本密码转换为 `SecureString`，满足 MSAL ROPC 接口的安全要求。
- 采用 `HttpClient` 设置 `Authorization: Bearer` 头调用 `https://graph.microsoft.com/v1.0/me`，验证令牌的有效性，并对 `MsalUiRequiredException`、`MsalServiceException` 等错误进行分类处理。
