# SharePoint Query Test (WPF) 示例说明

本工程提供一个基于 WPF 的界面用于执行 SharePoint CAML 查询，核心功能包括：

- 使用 `ClientContext` 根据用户输入的站点 URL、凭据类型和列表名称建立连接，并通过 `Lists.GetByTitle` 获取目标列表。
- 允许用户在界面中编写 CAML 语句，利用 `CamlQuery.ViewXml` 运行查询并通过 `List.GetItems` 获取结果集合。
- 将返回的列表项字段（如 `FileRef`、`FileDirRef`、`ID`）映射为绑定模型，在 `DataGrid` 中可视化展示查询结果，同时提供异常捕获提示。
