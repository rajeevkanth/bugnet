# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade BugNET from .NET Framework 4.5 to .NET 10 (LTS)
**Scope**: 19 projects — convert all legacy csproj to SDK-style, migrate packages.config to PackageReference, change TFM to net10.0, update incompatible packages, fix System.Web API incompatibilities.

### Selected Strategy
**Bottom-Up (Dependency-First)** — Upgrade from leaf nodes to root applications, tier by tier.
**Rationale**: 19 projects with 6-tier dependency graph targeting .NET Framework — Bottom-Up is mandatory for .NET Framework solutions.

```
Tier 6: BugNET_WAP
         ↓
Tier 5: BugNET.MailboxReader.Tests, HttpModule.MailBoxReader
         ↓
Tier 4: BugNET.MailboxReader, BugNET.Tests, HttpModule.Authentication, HttpModule.Localization
         ↓
Tier 3: BugNET.BLL, Provider.SqlDataProvider
         ↓
Tier 2: BugNET.DAL
         ↓
Tier 1: BugNET.Entities, Provider.CkHtmlEditorProvider, Provider.TextboxHtmlProvider
         ↓
Tier 0: BugNET.Common, LumiSoft.Net, Provider.HtmlEditorProvider, Provider.MembershipProviders, BugNET.MercurialChangeGroupHook, BugNET.SubversionHooks
```

## Tasks

### 01-prerequisites: Verify prerequisites and SDK toolchain

Confirm .NET 10 SDK is installed and accessible. Check global.json for any SDK version pins that might conflict with .NET 10. This is a mandatory first step before any project changes.

**Done when**: .NET 10 SDK confirmed installed; global.json validated or updated.

---

### 02-sdk-style-conversion: Convert all projects from legacy to SDK-style csproj

All 19 projects use legacy non-SDK csproj format (ToolsVersion attribute, explicit file listings, packages.config). This task converts all projects to SDK-style format while staying on the current TFM. SDK-style conversion is a structural change separate from the TFM upgrade — these two steps have different failure modes.

Key concerns:
- BugNET_WAP is a WAP (Web Application Project) — legacy csproj with thousands of lines. SDK conversion is possible but the project has WebForms-specific content (.aspx, .ascx, master pages) that needs careful handling.
- packages.config files must be migrated to PackageReference as part of SDK conversion.
- All projects must still build on net45 after this step.

**Done when**: All 19 projects converted to SDK-style format; packages.config files removed; solution restores and builds on existing TFM.

---

### 03-foundation-libs: Upgrade Tier 0 foundation libraries to net10.0

Upgrade the six foundation libraries that have no internal project dependencies: BugNET.Common, LumiSoft.Net, Provider.HtmlEditorProvider, Provider.MembershipProviders, BugNET.MercurialChangeGroupHook, BugNET.SubversionHooks.

Key concerns:
- **BugNET.Common** uses System.Web (HttpRequest extensions) and System.Web.UI (StateBag extensions) — these must be replaced or conditionally compiled. Use Microsoft.AspNetCore.SystemWebAdapters for compatibility or refactor to remove the dependency.
- **Provider.MembershipProviders** uses System.Web.Security — needs ASP.NET Core Identity or compatibility adapter.
- log4net 2.0.5 is incompatible with net10.0; upgrade to log4net 3.x across all projects.
- Mercurial.Net 1.1.1.607 is incompatible — remove or find alternative.

**Done when**: All Tier 0 projects compile targeting net10.0; higher tiers still build on net45.

---

### 04-entities-providers: Upgrade Tier 1 projects to net10.0

Upgrade BugNET.Entities, Provider.CkHtmlEditorProvider, Provider.TextboxHtmlProvider. These depend only on Tier 0 projects.

Key concerns:
- BugNET.Entities uses AutoMapper 3.2.1 which is incompatible with net10.0 — upgrade to AutoMapper 12.x+ or replace with equivalent.
- Provider.CkHtmlEditorProvider and Provider.TextboxHtmlProvider depend on Provider.HtmlEditorProvider (already upgraded in Tier 0).
- CKEditor/CkeditorForASP.NET packages are marked compatible; verify they still work.

**Done when**: BugNET.Entities, Provider.CkHtmlEditorProvider, Provider.TextboxHtmlProvider all compile on net10.0.

---

### 05-data-layer: Upgrade Tier 2 data layer to net10.0

Upgrade BugNET.DAL which depends on BugNET.Common and BugNET.Entities.

Key concerns:
- BugNET.DAL references System.Web — audit and replace with .NET 10 equivalents.
- log4net already upgraded in Tier 0.
- DataProvider pattern (DataProviderCollection, DataProviderManager) should be compatible with .NET 10.

**Done when**: BugNET.DAL compiles and all Tier 0-1 tests pass.

---

### 06-business-layer: Upgrade Tier 3 business logic to net10.0

Upgrade BugNET.BLL and Provider.SqlDataProvider. BugNET.BLL is the core business logic layer with the most complex System.Web dependencies.

Key concerns:
- BugNET.BLL uses HttpContext.Current (Security.cs), System.Web.Security.Membership (UserManager.cs), System.Web.Hosting.HostingEnvironment (HostSettingManager.cs), System.Web.Configuration (UpgradeManager.cs). Add Microsoft.AspNetCore.SystemWebAdapters or replace with .NET 10 equivalents.
- Provider.SqlDataProvider depends on BugNET.DAL and Entities.
- log4net already upgraded.

**Done when**: BugNET.BLL and Provider.SqlDataProvider compile on net10.0.

---

### 07-app-services: Upgrade Tier 4 projects to net10.0

Upgrade BugNET.MailboxReader, BugNET.Tests, HttpModule.Authentication, HttpModule.Localization. These all depend on BugNET.BLL.

Key concerns:
- BugNET.MailboxReader uses HtmlAgilityPack 1.4.9 — upgrade to HtmlAgilityPack 1.12.x.
- BugNET.Tests uses NUnit 3.0.1 — upgrade to NUnit 4.x and add NUnit3TestAdapter and Microsoft.NET.Test.Sdk.
- HttpModule.Authentication and HttpModule.Localization implement IHttpModule (System.Web) — these need to be rewritten as ASP.NET Core middleware or compatibility wrappers.

**Done when**: BugNET.MailboxReader and BugNET.Tests compile and unit tests run; HttpModules compile on net10.0.

---

### 08-integration-services: Upgrade Tier 5 projects to net10.0

Upgrade BugNET.MailboxReader.Tests and HttpModule.MailBoxReader.

Key concerns:
- BugNET.MailboxReader.Tests has the most project references (8 projects) and uses NUnit — update test runner.
- HttpModule.MailBoxReader depends on BugNET.MailboxReader and BugNET.BLL.

**Done when**: Both projects compile on net10.0; MailboxReader tests run.

---

### 09-web-app: Upgrade BugNET_WAP to net10.0

Upgrade the ASP.NET Web Application Project. This is the highest-difficulty project (34 mandatory issues, 47 package issues). Web Forms is not natively supported in .NET 10. The project can be converted to SDK-style and target net10.0, but significant code changes are required to compile.

Key concerns:
- AjaxControlToolkit, Microsoft.AspNet.FriendlyUrls, DotNetOpenAuth, AspNet.ScriptManager.* are all incompatible and have no .NET 10 replacements.
- System.Web.UI (WebForms) is not available in .NET 10 — .aspx pages, user controls, and code-behind files cannot compile.
- BugNET.Tests has a solution-level build dependency on BugNET_WAP — this must be addressed.
- EntityFramework 6.x works on .NET 10 if upgraded to EF 6.5.x.

**Done when**: BugNET_WAP project file is SDK-style and targets net10.0; maximum possible packages updated; compilation status documented.

---

### 10-final-validation: Final solution build and test validation

Run the full solution build, execute all unit tests, and document final status. Address any remaining compilation errors or test failures. Summarize which projects successfully upgraded vs which have remaining issues.

**Done when**: Build executed; test results collected; all resolvable issues fixed; final status documented.

