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

Versioning follows CI run numbers (`1.0.{run}`) on pushes to `main`.

## NuGet publishing

Publishing uses NuGet Trusted Publishing from GitHub Actions, not a stored API key.

Configure a trusted publishing policy on nuget.org:

| Field | Value |
|-------|-------|
| Repository owner | `buchmiet` |
| Repository | `GuiDispatcher.Sharp.Avalonia` |
| Workflow file | `publish-nuget.yml` |
| Environment | `production` |

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
