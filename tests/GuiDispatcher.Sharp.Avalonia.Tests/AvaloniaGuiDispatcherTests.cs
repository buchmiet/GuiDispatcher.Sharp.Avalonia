using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using GuiDispatcher.Sharp.Avalonia;
using Xunit;

namespace GuiDispatcher.Sharp.Avalonia.Tests;

public class AvaloniaGuiDispatcherTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

    [AvaloniaFact]
    public void CheckAccess_OnUiThread_ReturnsTrue()
    {
        var dispatcher = new AvaloniaGuiDispatcher();
        Assert.True(dispatcher.CheckAccess());
    }

    [AvaloniaFact]
    public async Task Post_FromBackgroundThread_RunsOnUiThread()
    {
        var dispatcher = new AvaloniaGuiDispatcher();
        var posted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        await Task.Run(() =>
        {
            dispatcher.Post(() =>
            {
                Assert.True(Dispatcher.UIThread.CheckAccess());
                posted.SetResult();
            });
        });

        await posted.Task.WaitAsync(Timeout);
    }

    [AvaloniaFact]
    public async Task InvokeAsync_Action_FromBackgroundThread_CompletesOnUiThread()
    {
        var dispatcher = new AvaloniaGuiDispatcher();
        var onUiThread = false;

        await Task.Run(async () =>
        {
            await dispatcher.InvokeAsync(() => onUiThread = Dispatcher.UIThread.CheckAccess());
        });

        Assert.True(onUiThread);
    }

    [AvaloniaFact]
    public void Post_NullAction_Throws() =>
        Assert.Throws<ArgumentNullException>(() => new AvaloniaGuiDispatcher().Post(null!));

    [AvaloniaFact]
    public async Task Invoke_FromBackgroundThread_RunsOnUiThread()
    {
        var dispatcher = new AvaloniaGuiDispatcher();
        var onUiThread = false;

        await Task.Run(() => dispatcher.Invoke(() => onUiThread = Dispatcher.UIThread.CheckAccess()));

        Assert.True(onUiThread);
    }

    [AvaloniaFact]
    public async Task Invoke_ReturnsValue_FromBackgroundThread()
    {
        var dispatcher = new AvaloniaGuiDispatcher();
        var value = await Task.Run(() => dispatcher.Invoke(() => 42));
        Assert.Equal(42, value);
    }

    [AvaloniaFact]
    public async Task RunOnce_ExecutesOnceAfterDelay()
    {
        var dispatcher = new AvaloniaGuiDispatcher();
        var executed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var executeCount = 0;
        using var _ = dispatcher.RunOnce(() =>
        {
            Interlocked.Increment(ref executeCount);
            executed.TrySetResult();
        }, TimeSpan.FromMilliseconds(50));

        await executed.Task.WaitAsync(Timeout);
        await Task.Delay(100);
        Assert.Equal(1, executeCount);
    }

    [AvaloniaFact]
    public void RunOnce_NegativeInterval_Throws()
    {
        var dispatcher = new AvaloniaGuiDispatcher();
        Assert.Throws<ArgumentOutOfRangeException>(() => dispatcher.RunOnce(() => { }, TimeSpan.FromMilliseconds(-1)));
    }

    [AvaloniaFact]
    public void CreateTimer_NegativeInterval_Throws()
    {
        var dispatcher = new AvaloniaGuiDispatcher();
        Assert.Throws<ArgumentOutOfRangeException>(() => dispatcher.CreateTimer(TimeSpan.FromMilliseconds(-1)));
    }
}
