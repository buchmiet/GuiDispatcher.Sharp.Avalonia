using Avalonia.Markup.Xaml;

namespace GuiDispatcher.Sharp.Avalonia.Tests;

public class App : global::Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
