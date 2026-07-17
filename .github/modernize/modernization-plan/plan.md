# Modernization Plan: BugNET — Modernize to Azure

**Project**: BugNET

---

## Technical Framework

- **Language**: C# / .NET Framework 4.5
- **Framework**: ASP.NET Web Forms (legacy, non-SDK-style project format)
- **Build Tool**: MSBuild / NuGet (packages.config)
- **Database**: SQL Server (LocalDB in dev via `System.Data.SqlClient`, SqlDataProvider pattern)
- **Key Dependencies**: Entity Framework 6.1.3, WindowsAzure.Storage 6.2.0 (legacy SDK), Microsoft.Azure.KeyVault.Core 1.0.0 (legacy SDK), log4net 2.0.5, ASP.NET Membership / Role / Session providers, DotNetOpenAuth 4.3.4

---

## Overview

> This migration modernizes BugNET, a legacy ASP.NET Web Forms bug-tracking application, from .NET Framework 4.5 to .NET 10 LTS and deploys it to Azure Container Apps. The application currently runs on .NET Framework 4.5 (end-of-life, no `netstandard2.0` support) with local SQL Server, legacy Azure Storage and Key Vault SDKs, and log4net logging. The new architecture will:
>
> - Run on .NET 10 LTS with SDK-style project format, enabling modern Azure SDK compatibility and long-term support
> - Use Azure SQL Database with Managed Identity, eliminating connection-string secrets and improving security posture
> - Use Azure Blob Storage with Managed Identity via the modern `Azure.Storage.Blobs` SDK
> - Use Azure Key Vault Secrets with Managed Identity via the modern `Azure.Security.KeyVault.Secrets` SDK
> - Emit structured console logs suitable for cloud log aggregation (replacing log4net)
> - Deploy as a containerized application on Azure Container Apps for scalable, serverless hosting
>
> The migration follows a phased approach: framework upgrade first, Azure service migration tasks next, integration testing against real provisioned infrastructure, CVE remediation, and finally containerized deployment.

---

## Migration Impact Summary

| Application  | Original Service               | New Azure Service             | Authentication    | Comments                              |
|--------------|-------------------------------|-------------------------------|-------------------|---------------------------------------|
| BugNET       | SQL Server (LocalDB)           | Azure SQL Database            | Managed Identity  | Entity Framework 6 provider update    |
| BugNET       | WindowsAzure.Storage 6.2.0     | Azure Blob Storage (Azure.*)  | Managed Identity  | Replace legacy SDK                    |
| BugNET       | Azure Key Vault Core 1.0.0     | Azure Key Vault Secrets       | Managed Identity  | Replace legacy SDK                    |
| BugNET       | log4net file/event logging     | Console Logging               | N/A               | Cloud-compatible log aggregation      |
| BugNET       | Local / on-prem hosting        | Azure Container Apps          | Managed Identity  | New containerized deployment          |

---

## Open Questions & Questionnaire

- [x] Q: Should the plan include infrastructure provisioning? → A: Yes — provision new infrastructure on Azure
- [x] Q: Azure Subscription ID for provisioning? → A: `48070cbe-fe32-4925-938e-731362e54c47`
- [x] Q: Should the plan include integration testing? → A: Yes — Real mode using provisioned infrastructure
- [x] Q: Should the plan include security/CVE remediation? → A: Yes — include security/CVE remediation
- [x] Q: Which Azure deployment target? → A: Azure Container Apps
