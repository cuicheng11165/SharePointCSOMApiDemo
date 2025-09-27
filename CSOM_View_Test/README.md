# CSOM_View_Test 示例说明

本工程演示如何使用 CSOM 获取列表视图及其字段信息，主要步骤如下：

- 通过 `ClientContext.Web.Lists.GetById` 获取目标列表，并在 `Load` 时结合 `Views.IncludeWithDefaultProperties(v => v.ViewFields)` 载入视图字段集合。
- 使用 `Web.GetFileByServerRelativeUrl` 读取视图对应的 `.aspx` 文件，并获取其 `ETag` 等属性。
- 示例展示了在 `ExecuteQuery` 前后多次加载不同对象的标准模式，可帮助理解视图与文件之间的关系。
