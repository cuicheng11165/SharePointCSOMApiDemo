# UpdateWebAllProperties 示例说明

本工程演示如何同时调用 SharePoint Online 租户管理 API 与站点级 CSOM API 更新站点属性，关键步骤包括：

- 通过 `Tenant.GetSitePropertiesByUrl` 读取站点属性对象，修改 `DenyAddAndCustomizePages` 后调用 `Update` 并执行 `ExecuteQuery` 提交变更。
- 在站点上下文中加载 `Web.AllProperties`，写入自定义属性键值并调用 `Web.Update` 进行持久化。
- 示例中同样展示了在自签名证书或开发环境下通过 `ServicePointManager.ServerCertificateValidationCallback` 跳过 TLS 证书验证的方式。
