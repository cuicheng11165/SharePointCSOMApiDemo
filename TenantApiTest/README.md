# TenantApiTest 示例说明

本工程演示了多项 SharePoint Online 租户级管理 API 的调用方式，主要包括：

- 使用 `Tenant.GetSPOContainerTypes`/`GetSPOContainersByApplicationId`/`GetSPOContainerByContainerId` 枚举并读取 Loop（SPO Container）相关的租户信息。
- 通过 `Tenant.GetSitePropertiesByUrl` 载入指定站点的租户属性，并修改 `DenyAddAndCustomizePages` 等设置后调用 `Update` 提交。
- 展示了标准的 `ClientContext.Load` + `ExecuteQuery` 调用链路，可作为其他租户管理脚本的基础模板。
