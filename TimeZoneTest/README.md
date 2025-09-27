# TimeZoneTest 示例说明

本工程比较了 SharePoint CSOM 与服务器端对象模型（SSOM）在处理 `Modified` 时间字段时的行为差异，关键演示内容包括：

- 使用 `ClientContext` 访问文件与列表项，调用 `ListItem.Update`/`ExecuteQuery` 设置不同 `DateTimeKind` 的时间并读取回写结果。
- 在非 .NET 8 环境下，使用 `SPSite`、`SPListItem` 等服务器端对象模型，演示服务器端更新同一字段时的时间转换效果。
- 通过控制台输出比较本地时间、UTC、未指定 Kind 的差异，为排查 SharePoint 时区与时间戳问题提供参考。
