using Avalonia.Threading;
using GuiDispatcher.Sharp.Contracts;

namespace GuiDispatcher.Sharp.Avalonia;

/// <summary>Dispatcher timer backed by Avalonia's <see cref="DispatcherTimer"/>.</summary>
public class AvaloniaGuiTimer : IGuiTimer
{
    private readonly DispatcherTimer _timer;

    /// <summary>Creates a timer on the UI thread with the given interval.</summary>
    /// <param name="interval">Tick interval. Must be greater than or equal to zero.</param>
    public AvaloniaGuiTimer(TimeSpan interval)
    {
        if (interval < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be greater than or equal to zero.");

        _timer = new DispatcherTimer { Interval = interval };
        _timer.Tick += OnTick;
    }

    /// <inheritdoc />
    public event EventHandler? Tick;

    /// <inheritdoc />
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

    /// <inheritdoc />
    public bool IsEnabled
    {
        get => _timer.IsEnabled;
        set => _timer.IsEnabled = value;
    }

    /// <inheritdoc />
    public void Start() => _timer.Start();

    /// <inheritdoc />
    public void Stop() => _timer.Stop();

    /// <inheritdoc />
    public void Dispose()
    {
        _timer.Stop();
        _timer.Tick -= OnTick;
    }

    private void OnTick(object? sender, EventArgs e) => Tick?.Invoke(this, e);
}
