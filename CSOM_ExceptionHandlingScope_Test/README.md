# CSOM_ExceptionHandlingScope_Test 示例说明

本工程示范如何在 CSOM 中使用 `ExceptionHandlingScope` 捕获服务器端异常，涉及以下要点：

- 借助 `ExceptionHandlingScope.StartScope/StartTry/StartCatch` 结构，在访问 `Folders.GetByUrl` 时尝试断开权限继承，若目标文件夹不存在则在 `Catch` 分支中创建文件夹并继续操作。
- 通过 `Folder.ListItemAllFields.BreakRoleInheritance(true, true)` 调整列表项权限，展示异常捕获后继续执行 CSOM 命令的方式。
- 在 `ExecuteQuery` 后读取 `exceptionHandlingScope.HasException` 与 `ErrorMessage`，了解服务器端错误的具体信息。
