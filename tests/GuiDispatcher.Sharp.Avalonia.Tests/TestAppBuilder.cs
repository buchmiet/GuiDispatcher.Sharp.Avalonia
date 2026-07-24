using Avalonia;
using Avalonia.Headless;

[assembly: AvaloniaTestApplication(typeof(GuiDispatcher.Sharp.Avalonia.Tests.TestAppBuilder))]

namespace GuiDispatcher.Sharp.Avalonia.Tests;

public static class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}
