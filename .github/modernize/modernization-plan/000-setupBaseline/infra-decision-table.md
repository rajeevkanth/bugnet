# Infrastructure Decision Table — BugNET Migration Baseline

> **Captured at**: 2026-07-17 (commit `84f0d9364d47e823de647e8e4f56255fd53a1890`)
> **Purpose**: Records the real/mock strategy for each external dependency used by the `007-integrationTest` task.

---

| Dependency | Current Technology | Target Azure Service | Integration Test Mode | Auth Method (post-migration) | Notes |
|---|---|---|---|---|---|
| **Relational Database** | SQL Server LocalDB (`System.Data.SqlClient`) | Azure SQL Database | REAL | Managed Identity (DefaultAzureCredential / Active Directory Default) | Connection string secret eliminated; `Microsoft.Data.SqlClient` with AAD token |
| **Blob / File Storage** | Local filesystem upload path + `WindowsAzure.Storage` SDK (NuGet dep) | Azure Blob Storage (`Azure.Storage.Blobs`) | REAL | Managed Identity (DefaultAzureCredential) | `StorageAccountUri` from env var `AZURE_STORAGE_ACCOUNT_URI`; container name configurable |
| **Secret Store** | `Microsoft.Azure.KeyVault.Core` 1.0.0 (NuGet dep, no direct API calls found) | Azure Key Vault Secrets (`Azure.Security.KeyVault.Secrets`) | REAL | Managed Identity (DefaultAzureCredential) | `SecretClient` initialised with `AZURE_KEY_VAULT_URI` env var |
| **Logging** | log4net 2.0.5 (`ILog`, `AdoNetAppender`) | Console logging (`Microsoft.Extensions.Logging`) | MOCK (replaced) | N/A | log4net removed; tests verify `ILogger` is wired and messages appear on stdout/stderr |
| **SMTP / Email** | SMTP server via `HostSettingNames` (Pop3Server, SMTPServer) | No change (SMTP remains) | MOCK | N/A | SMTP not replaced; test suite mocks outbound email |
| **Authentication** | ASP.NET Membership + DotNetOpenAuth | ASP.NET Core Identity (post-upgrade) | MOCK | N/A | OAuth/OpenId flows mocked in unit tests |

---

## Decision Details

### SQL Server → Azure SQL Database (REAL mode)
- Integration tests connect to the provisioned Azure SQL Database instance.
- Container App Managed Identity must have `db_datareader`, `db_datawriter`, and `db_ddladmin` (for schema migrations) roles.
- Connection string format: `Server=<server>.database.windows.net;Database=<db>;Authentication=Active Directory Default;`
- Env var: `AZURE_SQL_CONNECTION_STRING` or individual parts `AZURE_SQL_SERVER`, `AZURE_SQL_DATABASE`.

### WindowsAzure.Storage → Azure Blob Storage (REAL mode)
- Integration tests upload and download actual blobs from a provisioned Azure Storage account.
- Container App Managed Identity must have **Storage Blob Data Contributor** role on the storage account.
- Env var: `AZURE_STORAGE_ACCOUNT_URI` (e.g., `https://<account>.blob.core.windows.net`).
- Blob container name: `bugnet-attachments` (configurable via `AZURE_STORAGE_CONTAINER_NAME`).

### Azure Key Vault Core → Azure Key Vault Secrets (REAL mode)
- Integration tests call `SecretClient.GetSecret()` against the provisioned Key Vault.
- Container App Managed Identity must have **Key Vault Secrets User** role.
- Env var: `AZURE_KEY_VAULT_URI` (e.g., `https://<vault>.vault.azure.net`).

### log4net → Console Logging (MOCK / replaced)
- No real external service involved.
- Post-migration tests verify that `ILoggerFactory` / `ILogger<T>` is registered in DI and that no log4net references remain.
- Test assertion: log output captured from stdout matches expected log level and message pattern.

---

## Environment Variables Required for Integration Tests

| Variable | Description | Example |
|---|---|---|
| `AZURE_SQL_SERVER` | Azure SQL Server hostname | `bugnet-sql.database.windows.net` |
| `AZURE_SQL_DATABASE` | Azure SQL Database name | `bugnetdb` |
| `AZURE_STORAGE_ACCOUNT_URI` | Azure Storage Account Blob Service URI | `https://bugnetstore.blob.core.windows.net` |
| `AZURE_STORAGE_CONTAINER_NAME` | Blob container for attachments | `bugnet-attachments` |
| `AZURE_KEY_VAULT_URI` | Azure Key Vault URI | `https://bugnetkv.vault.azure.net` |
| `AZURE_CLIENT_ID` | Managed Identity client ID (if user-assigned) | `<guid>` |
