# GuiDispatcher.Sharp.Avalonia

Avalonia implementation for `GuiDispatcher.Sharp`.

## Install

```xml
<PackageReference Include="GuiDispatcher.Sharp.Avalonia" Version="1.0.*" />
```

Or via CLI:

```bash
dotnet add package GuiDispatcher.Sharp.Avalonia
```

Versioning follows [Semantic Versioning](https://semver.org/). Releases are cut by pushing a `vX.Y.Z` git tag; see [CHANGELOG.md](CHANGELOG.md) for release history.

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

## Usage

```csharp
using GuiDispatcher.Sharp;
using GuiDispatcher.Sharp.Avalonia;

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
