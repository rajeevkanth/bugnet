# BugNET_WAP

## Summary

| Metric | Value |
|--------|-------|
| Total Issues | 12 |
| Mandatory Blockers | 3 |
| Potential Issues | 5 |

## Component Information

| Property | Value |
|----------|-------|
| Language | C# |
| Frameworks | .NETFramework,Version=v4.5 |
| Build tools | MSBuild |

## Cloud Readiness Issues

| Issue Name | Criticality | Story Points | Occurrences |
|------------|-------------|--------------|-------------|
| Logging to local or network paths detected | Mandatory | 3 | [5](#Logging_to_local_or_network_paths_detected) |
| Windows authentication detected | Mandatory | 3 | [4](#Windows_authentication_detected) |
| Hardcoded local or network paths detected | Mandatory | 1 | [1](#Hardcoded_local_or_network_paths_detected) |
| Old .NET Framework dependency detected | Potential | 3 | [19](#Old_NET_Framework_dependency_detected) |
| Hardcoded URLs detected | Potential | 1 | [10](#Hardcoded_URLs_detected) |
| SQL database connection detected | Potential | 3 | [4](#SQL_database_connection_detected) |
| Session state stored in-proc or in local process is detected | Potential | 3 | [1](#Session_state_stored_in-proc_or_in_local_process_is_detected) |
| Database dependency detected | Potential | 5 | [1](#Database_dependency_detected) |
| Hardcoded sensitive data detected | Optional | 3 | [17](#Hardcoded_sensitive_data_detected) |
| Connection strings without configuration builders detected | Optional | 3 | [8](#Connection_strings_without_configuration_builders_detected) |
| Static content detected | Optional | 3 | [1](#Static_content_detected) |
| System.Data.SqlClient dependency detected | Optional | 3 | [1](#System_Data_SqlClient_dependency_detected) |

### Issue Details

<details id="Logging_to_local_or_network_paths_detected">
<summary><b>Logging to local or network paths detected</b> — affected files</summary>

- `BugNET_WAP/Web.Debug.config`
- `BugNET.MercurialChangeGroupHook/App.config`
- `BugNET.SubversionHooks/app.config`
- `BugNET.SubversionHooks/app.config`
- `BugNET.Tests/App.config`

</details>

<details id="Windows_authentication_detected">
<summary><b>Windows authentication detected</b> — affected files</summary>

- `BugNET_WAP/Web.config`
- `BugNET_WAP/Web.Release.config`
- `BugNET.MailboxReader.Tests/App.config`
- `BugNET.Tests/App.config`

</details>

<details id="Hardcoded_local_or_network_paths_detected">
<summary><b>Hardcoded local or network paths detected</b> — affected files</summary>

- `BugNET.MailboxReader.Tests/Helpers.cs (line 37)`

</details>

<details id="Old_NET_Framework_dependency_detected">
<summary><b>Old .NET Framework dependency detected</b> — affected files</summary>

- `BugNET_WAP/BugNET_WAP.csproj`
- `BugNET.BLL/BugNET.BLL.csproj`
- `BugNET.Common/BugNET.Common.csproj`
- `BugNET.DAL/BugNET.DAL.csproj`
- `BugNET.Entities/BugNET.Entities.csproj`
- `BugNET.MailboxReader/BugNET.MailboxReader.csproj`
- `BugNET.MailboxReader.Tests/BugNET.MailboxReader.Tests.csproj`
- `BugNET.MercurialChangeGroupHook/BugNET.MercurialChangeGroupHook.csproj`
- `BugNET.SubversionHooks/BugNET.SubversionHooks.csproj`
- `BugNET.Tests/BugNET.Tests.csproj`
- `Library/HttpModules/Authentication/HttpModule.Authentication.csproj`
- `Library/HttpModules/Localization/HttpModule.Localization.csproj`
- `Library/HttpModules/MailBoxReader/HttpModule.MailBoxReader.csproj`
- `LumiSoft.Net/LumiSoft.Net.csproj`
- `Library/Providers/HtmlEditorProviders/CkHtmlEditorProvider/Provider.CkHtmlEditorProvider.csproj`
- `Library/Providers/HtmlEditorProviders/HtmlEditorProvider/Provider.HtmlEditorProvider.csproj`
- `Library/Providers/MembershipProviders/Provider.MembershipProviders.csproj`
- `Library/Providers/DataProviders/SqlDataProvider/Provider.SqlDataProvider.csproj`
- `Library/Providers/HtmlEditorProviders/TextboxHtmlProvider/Provider.TextboxHtmlProvider.csproj`

</details>

<details id="Hardcoded_URLs_detected">
<summary><b>Hardcoded URLs detected</b> — affected files</summary>

- `BugNET.Common/IsEmail.cs (line 446)`
- `BugNET.Common/IsEmail.cs (line 864)`
- `BugNET.Common/IsEmail.cs (line 824)`
- `BugNET.Common/IsEmail.cs (line 298)`
- `BugNET.Common/IsEmail.cs (line 282)`
- `BugNET.Common/IsEmail.cs (line 111)`
- `BugNET.Common/IsEmail.cs (line 122)`
- `BugNET.Common/IsEmail.cs (line 133)`
- `BugNET.Common/IsEmail.cs (line 94)`
- `BugNET.Common/IsEmail.cs (line 874)`

</details>

<details id="SQL_database_connection_detected">
<summary><b>SQL database connection detected</b> — affected files</summary>

- `BugNET_WAP/Web.config`
- `BugNET_WAP/Web.Release.config`
- `BugNET.MailboxReader.Tests/App.config`
- `BugNET.Tests/App.config`

</details>

<details id="Session_state_stored_in-proc_or_in_local_process_is_detected">
<summary><b>Session state stored in-proc or in local process is detected</b> — affected files</summary>

- `BugNET_WAP/Web.config`

</details>

<details id="Database_dependency_detected">
<summary><b>Database dependency detected</b> — affected files</summary>

- `BugNET.Tests/App.config`

</details>

<details id="Hardcoded_sensitive_data_detected">
<summary><b>Hardcoded sensitive data detected</b> — affected files</summary>

- `BugNET.BLL/WebProfile.cs (line 116)`
- `BugNET.BLL/WebProfile.cs (line 104)`
- `BugNET.BLL/WebProfile.cs (line 108)`
- `BugNET.BLL/WebProfile.cs (line 120)`
- `BugNET.BLL/UserManager.cs (line 417)`
- `BugNET.BLL/UserManager.cs (line 418)`
- `BugNET.BLL/SourceIntegrationManager.cs (line 215)`
- `BugNET.BLL/SourceIntegrationManager.cs (line 136)`
- `BugNET.BLL/SourceIntegrationManager.cs (line 214)`
- `BugNET.MercurialChangeGroupHook/AppSettings.cs (line 23)`
- `BugNET.MercurialChangeGroupHook/App.config`
- `BugNET.MercurialChangeGroupHook/App.config`
- `BugNET.MercurialChangeGroupHook/Program.cs (line 67)`
- `BugNET.SubversionHooks/CommandExecutor.cs (line 74)`
- `BugNET.SubversionHooks/CommandExecutor.cs (line 73)`
- `BugNET.SubversionHooks/IssueTrackerIntegration.cs (line 87)`
- `Library/Providers/DataProviders/SqlDataProvider/SqlDataProviderConstants.cs (line 30)`

</details>

<details id="Connection_strings_without_configuration_builders_detected">
<summary><b>Connection strings without configuration builders detected</b> — affected files</summary>

- `BugNET_WAP/Web.config`
- `BugNET_WAP/Web.config`
- `BugNET_WAP/Web.Debug.config`
- `BugNET_WAP/Web.Release.config`
- `BugNET.MailboxReader.Tests/App.config`
- `BugNET.MailboxReader.Tests/App.config`
- `BugNET.MercurialChangeGroupHook/App.config`
- `BugNET.Tests/App.config`

</details>

<details id="Static_content_detected">
<summary><b>Static content detected</b> — affected files</summary>

- `BugNET_WAP/BugNET_WAP.csproj`

</details>

<details id="System_Data_SqlClient_dependency_detected">
<summary><b>System.Data.SqlClient dependency detected</b> — affected files</summary>

- `BugNET_WAP/Web.config`

</details>

---

## Codebase Insights

> **Note:** These documents are generated by AI and may contain inaccuracies or incomplete information. Please review carefully.

> **Codebase Insights aren't available yet.**
>
> These documents are generated when assessment runs with **Full analysis** coverage. Re-run the assessment and set `analysisCoverage: full` to enable them.

[Share feedback](https://aka.ms/ghcp-appmod/feedback)
