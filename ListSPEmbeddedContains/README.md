# ListSPEmbeddedContains 示例说明

本工程演示如何利用 SharePoint Online 租户管理 API 检索 Loop/Embedded 容器，涉及的关键调用包括：

- 通过 `Tenant.GetSPOContainerTypes` 分别获取 OwningTenant 与 ConsumingTenant 两类容器类型元数据。
- 使用 `Tenant.GetSPOContainersByApplicationId` 根据应用程序 ID（例如 Loop 应用）检索容器集合，并结合 `Tenant.GetSPOContainerByContainerId` 读取容器详细属性。
- 通过 `ClientContext.ExecutingWebRequest` 在租户管理端点上携带 Bearer Token，演示如何调用新的 SPO Container 管理 API 并输出容器配置。
