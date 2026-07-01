using GuiDispatcher.Sharp;

namespace GuiDispatcher.Sharp.Avalonia;

/// <summary>Dispatcher timer backed by Avalonia's <see cref="DispatcherTimer"/>.</summary>
public sealed class AvaloniaGuiTimer : IGuiTimer
{
    private readonly global::Avalonia.Threading.DispatcherTimer _timer;

    public AvaloniaGuiTimer(TimeSpan interval)
    {
        if (interval < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be greater than or equal to zero.");

        _timer = new global::Avalonia.Threading.DispatcherTimer { Interval = interval };
        _timer.Tick += OnTick;
    }

    public event EventHandler? Tick;

    public TimeSpan Interval
    {
        get => _timer.Interval;
        set
        {
            if (value < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(value), "Interval must be greater than or equal to zero.");

            _timer.Interval = value;
        }
    }

    public bool IsEnabled
    {
        get => _timer.IsEnabled;
        set => _timer.IsEnabled = value;
    }

    public void Start() => _timer.Start();

    public void Stop() => _timer.Stop();

    public void Dispose()
    {
        _timer.Stop();
        _timer.Tick -= OnTick;
    }

    private void OnTick(object? sender, EventArgs e) => Tick?.Invoke(this, e);
}
