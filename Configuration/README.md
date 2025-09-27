# Configuration 示例说明

本工程提供 `CSOM.Common.EnvConfig` 辅助类，集中演示以下配置读取能力：

- 通过 `AppDomain.CurrentDomain.BaseDirectory` 组合路径，定位解决方案根目录下的 `Config` 文件夹。
- 使用 `File.ReadAllText` 读取诸如 `HostName.txt`、`ClientId.txt`、`TenantId.txt`、`CSOMAuthorization.txt` 等配置，封装为静态属性供其他工程引用。
- 提供 `GetSiteUrl`、`GetAdminCenterUrl`、`GetCsomToken` 等便捷方法，简化 SharePoint/Graph 示例工程中的站点地址拼接与令牌获取。
