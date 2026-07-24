using Avalonia.Headless.XUnit;
using GuiDispatcher.Sharp.Avalonia;
using Xunit;

namespace GuiDispatcher.Sharp.Avalonia.Tests;

public class AvaloniaGuiTimerTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(3);

    [AvaloniaFact]
    public async Task Start_FiresTick()
    {
        var ticked = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var timer = new AvaloniaGuiTimer(TimeSpan.FromMilliseconds(50));
        timer.Tick += (_, _) => ticked.TrySetResult();
        timer.Start();
        await ticked.Task.WaitAsync(Timeout);
    }

    [AvaloniaFact]
    public async Task Stop_PreventsFurtherTicks()
    {
        var tickCount = 0;
        using var timer = new AvaloniaGuiTimer(TimeSpan.FromMilliseconds(30));
        timer.Tick += (_, _) => Interlocked.Increment(ref tickCount);
        timer.Start();
        await Task.Delay(80);
        timer.Stop();
        var countAfterStop = tickCount;
        await Task.Delay(80);
        Assert.True(tickCount >= 1);
        Assert.Equal(countAfterStop, tickCount);
    }
}
