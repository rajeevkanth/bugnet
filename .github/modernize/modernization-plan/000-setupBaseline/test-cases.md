# Frozen Behavior Baseline — BugNET Test Cases

> **Captured at**: 2026-07-17 (commit `84f0d9364d47e823de647e8e4f56255fd53a1890`)
> **Purpose**: Frozen test specification used by the `007-integrationTest` task after migration.

---

## External Dependency: SQL Server (SqlDataProvider)

All data access flows through `SqlDataProvider` which opens `SqlConnection` with a connection string from `web.config` (`connectionStringName` in provider config).

### TC-SQL-001 — DataProvider is a single instance
- **Source**: `UnitTests/DataProviderTests.cs` / `BugNET.Tests/DataProviderTests.cs`
- **Action**: Call `DataProviderManager.Provider` twice
- **Expected**: Both references are the same instance (singleton)
- **Post-migration strategy**: REAL — Azure SQL Database with Managed Identity (DefaultAzureCredential)

### TC-SQL-002 — Create a new Issue
- **Source**: `BugNET.Tests/IssueTests.cs::TestCreateNewIssueClean`
- **Action**: Create `Issue` with ProjectId, Title, Description, CategoryId, PriorityId, StatusId, IssueTypeId, ResolutionId, AssignedUsername
- **Expected**: `IssueManager.SaveOrUpdate()` returns `true`; new issue retrievable by Id
- **Post-migration strategy**: REAL — Azure SQL Database

### TC-SQL-003 — Retrieve issue by Id
- **Source**: `BugNET.Tests/IssueTests.cs::TestGetIssueById`
- **Action**: `IssueManager.GetById(issueId)`
- **Expected**: Returns non-null `Issue` with expected fields
- **Post-migration strategy**: REAL — Azure SQL Database

### TC-SQL-004 — Retrieve issues by ProjectId
- **Source**: `BugNET.Tests/IssueTests.cs::TestGetIssuesByProjectId`
- **Action**: `IssueManager.GetByProjectId(projectId)`
- **Expected**: Returns non-empty list of issues
- **Post-migration strategy**: REAL — Azure SQL Database

### TC-SQL-005 — Soft-delete an Issue
- **Source**: `BugNET.Tests/IssueTests.cs::TestDeleteIssue`
- **Action**: `IssueManager.Delete(issueId)`
- **Expected**: Returns `true`; re-fetched issue has `Disabled = true`
- **Post-migration strategy**: REAL — Azure SQL Database

### TC-SQL-006 — Project CRUD
- **Source**: `BugNET.Tests/ProjectTests.cs`
- **Action**: Create / Update / Retrieve / Delete a project
- **Expected**: All operations succeed; retrieved project matches saved data
- **Post-migration strategy**: REAL — Azure SQL Database

---

## External Dependency: Azure Blob Storage (WindowsAzure.Storage 6.2.0)

The `WindowsAzure.Storage` package is declared in `BugNET_WAP/packages.config` as a dependency. The `IssueAttachmentStorageTypes` enum exposes `None=0`, `FileSystem=1`, `Database=2`. The project-seed.json used in tests sets `attachmentStorageType: 3` (Azure Blob, planned but not yet wired in the enum). The `IssueAttachmentManager.SaveOrUpdate` routes to `FileSystem` or `Database` paths; Azure Blob path is a future extension target.

### TC-BLOB-001 — Upload file attachment (FileSystem path)
- **Source**: `UnitTests/IssueAttachmentTests.cs`, `BugNET.Tests/test-cases/testdata/seed-data/attachment-seed.json`
- **Action**: `IssueAttachmentManager.SaveOrUpdate(attachment)` with FileSystem storage type
- **Expected**: File saved to upload path; `IssueAttachment` record created in DB
- **Post-migration strategy**: REAL — Azure Blob Storage (Azure.Storage.Blobs) with Managed Identity; attachment storage type becomes `AzureBlobStorage`

### TC-BLOB-002 — Download file attachment
- **Source**: `UnitTests/IssueAttachmentTests.cs`
- **Action**: `IssueAttachmentManager.GetAttachmentForDownload(attachmentId)`
- **Expected**: Returns `IssueAttachment` with binary content
- **Post-migration strategy**: REAL — Azure Blob Storage download via `BlobClient.DownloadContent()`

### TC-BLOB-003 — Attachment validation (invalid extension)
- **Source**: `IssueAttachmentManager.IsValidFile()`
- **Action**: Attempt upload with disallowed extension
- **Expected**: `ApplicationException` thrown with descriptive message
- **Post-migration strategy**: REAL — Logic is application-side; no Azure change needed

---

## External Dependency: Azure Key Vault (Microsoft.Azure.KeyVault.Core 1.0.0)

The `Microsoft.Azure.KeyVault.Core` package is listed in `BugNET_WAP/packages.config`. No direct API calls to Key Vault were found in the source code — the package is a transitive or planned dependency. Post-migration it will be replaced with `Azure.Security.KeyVault.Secrets` and accessed via Managed Identity.

### TC-KV-001 — Retrieve a secret value
- **Action**: Read a secret (e.g., DB connection string or SMTP password) from Key Vault using `SecretClient.GetSecret(name)`
- **Expected**: Returns non-null string matching stored secret value
- **Post-migration strategy**: REAL — Azure Key Vault Secrets with DefaultAzureCredential; Key Vault URI from environment variable `AZURE_KEY_VAULT_URI`

---

## External Dependency: Logging (log4net 2.0.5)

log4net is used throughout via `LogManager.GetLogger(typeof(T))` and `ILog`. The `AdoNetAppender` writes to SQL Server. Tests verify the appender is loaded.

### TC-LOG-001 — Log4Net configuration loads AdoNetAppender
- **Source**: `UnitTests/LoggingTests.cs::Log4NetConfigurationLoaded`
- **Action**: Call `LogManager.GetRepository().GetAppenders()`
- **Expected**: Appender list contains `"AdoNetAppender"`
- **Post-migration strategy**: MOCK (replaced) — log4net will be removed; console logging via `Microsoft.Extensions.Logging` will be verified by confirming log output reaches stdout

### TC-LOG-002 — Error-level log entry emitted on exception
- **Action**: Trigger a logged error path (e.g., bad SQL connection)
- **Expected**: Log entry appears at ERROR level
- **Post-migration strategy**: MOCK (replaced) — post-migration verifies `ILogger.LogError` is called and message appears in container stdout

---

## Seed Data Summary

| File | Contents |
|------|----------|
| `test-cases/testdata/seed-data/project-seed.json` | Project (id=1, allowAttachments=true, attachmentStorageType=3) |
| `test-cases/testdata/seed-data/issue-seed.json` | Sample issue linked to seed project |
| `test-cases/testdata/seed-data/attachment-seed.json` | Sample attachment linked to seed issue |
| `test-cases/testdata/inputs/sample.txt` | Plain text input file for upload tests |
| `test-cases/testdata/inputs/sample.pdf` | PDF input file for upload tests |
| `test-cases/testdata/inputs/sample-unicode.txt` | Unicode text file for upload tests |
| `test-cases/testdata/expectations/error-access-denied-body.html` | Expected error page for access denied |
| `test-cases/testdata/expectations/error-not-found-body.html` | Expected error page for not found |
