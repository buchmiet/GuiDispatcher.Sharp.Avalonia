# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.1.1] - 2026-07-24

### Added

- Dispatcher and timer integration tests running on Avalonia's headless UI thread.
- Regression coverage ensuring `InvokeAsync(Func<Task>)` remains asynchronous and awaits the inner task.
- Test execution as a required step of the NuGet publishing workflow.

### Changed

- Excluded test sources, assets, and Avalonia XAML from the adapter project and package.
- Raised the `GuiDispatcher.Sharp` dependency floor to `[1.1.1,2.0.0)` for
  coordinated family releases.

## [1.1.0] - 2026-07-10

### Changed

- Target framework is now `net10.0` (dropped `net8.0`).
- Avalonia dependency updated to `12.1.0`.
- `GuiDispatcher.Sharp` dependency floor raised to `[1.1.0,2.0.0)`.
- Removed `sealed` from public types.
- Argument validation uses `ArgumentNullException.ThrowIfNull`.

## [1.0.3] - 2026-07-01

### Changed

- Switched releases to a git-tag-triggered flow (`vX.Y.Z`) instead of publishing on every push to `main`. The `Publish NuGet` workflow now validates that the `.csproj` version and the tag match, and that `CHANGELOG.md` has a corresponding entry, before packing and pushing to NuGet.
- Raised the `GuiDispatcher.Sharp` dependency floor to `[1.0.2,2.0.0)`, matching the tag-triggered release of the core package.

## [1.0.2] - 2026-07-01

### Changed

- Raised the `GuiDispatcher.Sharp` dependency floor to `[1.0.1,2.0.0)`.

## [1.0.1] - 2026-07-01

### Added

- Initial release of `GuiDispatcher.Sharp.Avalonia`.
- `AvaloniaGuiDispatcher`: `IGuiDispatcher` backed by `Avalonia.Threading.Dispatcher.UIThread`.
- `AvaloniaGuiTimer`: `IGuiTimer` implementation for Avalonia.
