# GuiDispatcher.Sharp.Avalonia

Avalonia 12 implementation of [GuiDispatcher.Sharp](https://www.nuget.org/packages/GuiDispatcher.Sharp) for **.NET 10**.

## Install

```xml
<PackageReference Include="GuiDispatcher.Sharp.Avalonia" Version="1.1.*" />
```

This package pulls in `GuiDispatcher.Sharp` (`1.1.2` or later, below `2.0.0`)
and `Avalonia` (`12.1.1`).

Or via CLI:

```bash
dotnet add package GuiDispatcher.Sharp.Avalonia
```

Versioning follows [Semantic Versioning](https://semver.org/). Releases are cut by pushing a `vX.Y.Z` git tag; see [CHANGELOG.md](CHANGELOG.md) for release history.

## Requirements

- .NET 10 (`net10.0`)
- Avalonia 12.1+

## NuGet publishing

Publishing uses NuGet Trusted Publishing from GitHub Actions, not a stored API key.

Configure a trusted publishing policy on nuget.org:

| Field | Value |
|-------|-------|
| Repository owner | `buchmiet` |
| Repository | `GuiDispatcher.Sharp.Avalonia` |
| Workflow file | `publish-nuget.yml` |
| Environment | `production` |

### Releasing

The `Publish NuGet` workflow only runs on `vX.Y.Z` tag pushes, and will fail unless the `.csproj` version and `CHANGELOG.md` are both updated first. To cut a release:

1. Bump `<Version>` in `GuiDispatcher.Sharp.Avalonia.csproj`.
2. Move the relevant `[Unreleased]` entries in `CHANGELOG.md` into a new `## [X.Y.Z] - YYYY-MM-DD` section.
3. Commit both changes.
4. Tag and push: `git tag vX.Y.Z && git push origin vX.Y.Z` — this triggers the publish workflow.

For a coordinated release of the complete package family, follow the
[family release guide](https://github.com/buchmiet/GuiDispatcher.Sharp/blob/main/RELEASING_FAMILY.md).

## Usage

```csharp
using GuiDispatcher.Sharp;
using GuiDispatcher.Sharp.Avalonia;
using GuiDispatcher.Sharp.Contracts;

services.AddSingleton<IGuiDispatcher, AvaloniaGuiDispatcher>();
```

The dispatcher is backed by `Avalonia.Threading.Dispatcher.UIThread`.

```csharp
await dispatcher.InvokeAsync(() =>
{
    viewModel.Apply(result);
});

using var timer = dispatcher.CreateTimer(TimeSpan.FromSeconds(1));
timer.Tick += (_, _) => viewModel.Refresh();
timer.Start();
```

For tests or headless hosts, use `ImmediateGuiDispatcher` from the core `GuiDispatcher.Sharp` package.
