# UpdateConlictSample 示例说明

本工程聚焦于 SharePoint 文档并发更新冲突的处理，主要演示以下 CSOM API：

- 使用 `File.CheckOut`、`File.CheckIn` 控制文档签出/签入流程，并通过 `ListItemAllFields` 修改 `FileLeafRef`、`Editor`、`Author`、`Modified` 等字段。
- 在 `ExecuteQuery` 过程中捕获因时间戳冲突产生的 `Microsoft.SharePoint.Client.ServerException`，了解 CSOM 对版本冲突的响应行为。
- 通过反射调用 `ListItem.ValidateUpdateListItem`（封装在 `Update` 辅助方法中）展示如何在保留版本的情况下更新列表项字段。
