# CamlQueryTest 示例说明

本工程围绕 SharePoint CAML 查询语法展开演示，主要包含以下 API 用法：

- 使用 `CamlQuery.CreateAllItemsQuery` 与 `CamlQuery.ViewXml` 构造不同的查询条件和返回字段集合，包括指定 `FolderServerRelativeUrl` 过滤特定文件夹。
- 通过 `List.GetItems` 获取列表项集合，并结合 `ClientContext.Load` 的 `IncludeWithDefaultProperties`/`ListItemCollectionPosition` 实现分页读取。
- 提供基本的 CAML 语法示例，如 `<Where><Geq>` 过滤条件和 `<RowLimit>` 限制，以帮助理解客户端查询的写法。
