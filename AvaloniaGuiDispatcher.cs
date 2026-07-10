using GuiDispatcher.Sharp.Contracts;
using AvaloniaDispatcher = Avalonia.Threading.Dispatcher;
using AvaloniaDispatcherTimer = Avalonia.Threading.DispatcherTimer;

namespace GuiDispatcher.Sharp.Avalonia;

/// <summary>
/// <see cref="IGuiDispatcher"/> implementation backed by Avalonia's UI thread.
/// </summary>
public class AvaloniaGuiDispatcher : IGuiDispatcher
{
    public bool CheckAccess() => AvaloniaDispatcher.UIThread.CheckAccess();

    public void Post(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);
        AvaloniaDispatcher.UIThread.Post(action);
    }

    public void Invoke(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (AvaloniaDispatcher.UIThread.CheckAccess())
            action();
        else
            AvaloniaDispatcher.UIThread.Invoke(action);
    }

    public async Task InvokeAsync(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (AvaloniaDispatcher.UIThread.CheckAccess())
        {
            action();
            return;
        }

        await AvaloniaDispatcher.UIThread.InvokeAsync(action);
    }

    public async Task InvokeAsync(Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (AvaloniaDispatcher.UIThread.CheckAccess())
        {
            await action().ConfigureAwait(false);
            return;
        }

        await AvaloniaDispatcher.UIThread.InvokeAsync(action);
    }

    public T Invoke<T>(Func<T> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return AvaloniaDispatcher.UIThread.CheckAccess()
            ? func()
            : AvaloniaDispatcher.UIThread.Invoke(func);
    }

    public IGuiTimer CreateTimer(TimeSpan interval) => new AvaloniaGuiTimer(interval);

    public IDisposable RunOnce(Action action, TimeSpan interval)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (interval < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be greater than or equal to zero.");

        return AvaloniaDispatcherTimer.RunOnce(action, interval);
    }
}
