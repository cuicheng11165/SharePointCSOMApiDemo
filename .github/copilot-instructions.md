# Copilot Instructions for CSOM_API_Test

## Project Overview
This repository contains standalone C# console applications demonstrating SharePoint Online CSOM and Graph API scenarios. Each folder is typically a self-contained project focusing on a specific feature (e.g., File Add, Taxonomy, Permissions).

## Architecture & Shared Components
- **Standalone Projects**: Most folders (e.g., `CSOM File Add Test`, `TenantApiTest`) are independent console apps.
- **Shared Configuration**: The `Configuration` project (namespace `CSOM.Common`) is a shared library referenced by most demos.
- **Config Directory**: A `Config/` folder at the solution root stores runtime configuration and secrets as plain text files.

## Critical Developer Workflows

### 1. Configuration Setup
Before running any code, ensure the `Config/` directory contains the necessary files:
- `HostName.txt`: SharePoint host (e.g., `contoso.sharepoint.com`)
- `ClientId.txt`, `TenantId.txt`: App registration details
- `Certificate.pfx`, `CertificatePassword.txt`: For app-only authentication
- `UserName.txt`, `Password.txt`: For user credentials (less common)

### 2. Authentication & Token Generation
The project uses a "generate and reuse" pattern for authentication tokens:
1.  **Run `ExportCsomToken`**: This project authenticates using the configured certificate and writes a Bearer token to `Config/CSOMAuthorization.txt`.
2.  **Run Demo Projects**: Other projects (e.g., `TenantApiTest`) read this token via `EnvConfig.GetCsomToken()` to authenticate their `ClientContext`.

### 3. Running Demos
- Build the solution or individual projects.
- Run the specific console app for the scenario you want to test.
- **Note**: Some older demos might have hardcoded URLs. Prefer using `EnvConfig.HostName` for new code.

## Coding Conventions & Patterns

### Configuration Access
Always use `CSOM.Common.EnvConfig` to access configuration values.
```csharp
using CSOM.Common;

string siteUrl = EnvConfig.GetSiteUrl("/sites/mysite");
string token = EnvConfig.GetCsomToken();
```

### CSOM Context with Bearer Token
When initializing `ClientContext` with the pre-generated token:
```csharp
using (var context = new ClientContext(siteUrl))
{
    context.ExecutingWebRequest += (sender, e) =>
    {
        e.WebRequestExecutor.RequestHeaders["Authorization"] = EnvConfig.GetCsomToken();
    };
    // ... operations
}
```

### Certificate Handling
Certificates are loaded from `Config/Certificate.pfx` using the password in `Config/CertificatePassword.txt`. See `ExportCsomToken/Program.cs` for the implementation.

## Integration Points
- **SharePoint Online**: Primary target.
- **Microsoft Graph**: Some projects (`ExportGraphToken`) demonstrate Graph API usage.
- **Local File System**: Used heavily for config and token storage.

## Language & Comments
- Code is in C#.
- Comments may be in English.
- Error handling is minimal in demos; focus is on API usage.

