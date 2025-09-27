# CSOM_CheckPermission 示例说明

本工程主要演示以下 CSOM API 的使用：

- 通过 `ClientContext` 连接 SharePoint Online 站点，并使用 `Web.CreateDefaultAssociatedGroups` 自动创建默认的成员、访客和所有者组。
- 使用 `Web.GetUserEffectivePermissions` 检索指定用户的权限集合，并结合 `BasePermissions.HasPermissions` 判定用户是否拥有特定权限位。
- 借助 `Web.RoleDefinitions.GetByType` 加载内置权限级别（例如 `RoleType.Contributor`），展示如何在 CSOM 中对权限定义进行读取。

示例代码演示了如何在执行 `ExecuteQuery` 后处理权限结果，以便在自定义工具中复用。
