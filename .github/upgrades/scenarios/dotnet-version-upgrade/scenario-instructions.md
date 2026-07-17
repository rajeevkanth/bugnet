# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0
- **Strategy**: Upgrade all projects to net10.0; convert from legacy non-SDK format to SDK-style; migrate packages.config to PackageReference.

## Source Control
- **Working Branch**: app-modernize-20260717011412
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## Strategy
**Selected**: Bottom-Up (Dependency-First)
**Rationale**: 19 projects with 6-tier dependency graph; .NET Framework solution — Bottom-Up is mandatory.

### Execution Constraints
- Strict tier ordering: Tier N must complete before Tier N+1 begins
- SDK-style conversion is separate from TFM upgrade — always done first on current TFM
- Each tier task includes build validation before proceeding
- Package log4net must be upgraded to 3.x across all projects
- NUnit must be upgraded to 4.x and NUnit3TestAdapter + Microsoft.NET.Test.Sdk added for test projects
- Microsoft.AspNetCore.SystemWebAdapters may be needed for System.Web compatibility in BLL/Common

## Decisions
- WAP project (BugNET_WAP) uses ASP.NET Web Forms with heavy System.Web dependency - will attempt upgrade with compatibility shims where possible
- Projects using System.Web will need Microsoft.AspNetCore.SystemWebAdapters or code changes
- Unit tests (BugNET.Tests) must pass after upgrade
