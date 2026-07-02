# Repository Guidelines

## Project Structure & Module Organization

This repository contains a .NET solution for exposing Notion tasks as an iCalendar feed.

- `Notion2Ical/`: main library targeting `net10.0`.
- `Notion2Ical/NotionApi/`: DTOs that map responses from the Notion API.
- `Notion2Ical/ICalendar/`: output models used to generate iCalendar data.
- `Notion2Ical/Interfaces/`: public service and repository contracts.
- `Notion2Ical.Tests/`: xUnit unit tests.
- `.github/workflows/ci.yml`: CI workflow for restore, build, test, and NuGet packing.

## Build, Test, and Development Commands

Use .NET 10. If the local shell does not resolve it, run `source ~/.zshrc` first.

```bash
dotnet restore Notion2Ical.sln
dotnet build Notion2Ical.sln
dotnet test Notion2Ical.sln
dotnet pack Notion2Ical/Notion2Ical.csproj --configuration Release --output artifacts
```

`dotnet test` runs the full unit test suite. `dotnet pack` creates a local NuGet package in `artifacts/`.

## Coding Style & Naming Conventions

Use modern C# style: file-scoped namespaces, nullable reference types, implicit usings, and project-level global usings. Keep local `using` directives out of source files unless a file-specific import is genuinely clearer.

Use PascalCase for public types and members. Keep DTOs in `NotionApi` and iCalendar output types in `ICalendar`; do not mix input and output models in the same namespace.

Prefer `System.Text.Json` over `Newtonsoft.Json`.

## Testing Guidelines

Tests use xUnit. Place tests in `Notion2Ical.Tests/` and name files after the unit under test, for example `NotionServiceTests.cs`.

Test names should describe behavior, e.g. `GetVCalendarData_SkipsTasksWithoutDueDate`. Avoid tests that call the real Notion API; use fakes or custom `HttpMessageHandler` implementations.

## Commit & Pull Request Guidelines

Follow the existing commit style: short, descriptive, imperative or present-tense messages, such as `Refactor Notion calendar feed`.

Pull requests should include a concise summary, test results, and any configuration or migration notes. Link related issues when applicable. For behavior changes, mention the affected feed output or Notion API assumptions.

## Security & Configuration Tips

Never commit Notion tokens, database IDs for private workspaces, or NuGet API keys. Use application configuration for `NotionCalendarOptions` and GitHub Secrets for future package publishing.

TokenSave may be available, but verify its worktree before relying on results.
