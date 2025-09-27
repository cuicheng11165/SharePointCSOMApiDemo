# CSOM File Add Test 示例说明

本工程汇总了多种通过 CSOM 处理文档库文件与托管元数据字段的操作，主要涵盖：

- 使用 `Folder.Files.Add` 搭配 `FileCreationInformation`/`ContentStream`/`Content` 向文档库上传小文件与大文件。
- 展示 `TaxonomySession.GetTaxonomySession`、`TaxonomyFieldValue`、`TaxonomyFieldValueCollection` 的用法，演示如何为托管元数据列生成有效的默认值并通过 `Field.UpdateAndPushChanges` 推送到内容类型。
- 演示 `File.SaveBinaryDirect`、计时工具等辅助方法，为大文件上传、流式写入和性能测量提供参考实现。
