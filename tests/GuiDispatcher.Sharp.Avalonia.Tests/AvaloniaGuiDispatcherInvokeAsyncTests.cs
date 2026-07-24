using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using GuiDispatcher.Sharp.Avalonia;
using Xunit;

namespace GuiDispatcher.Sharp.Avalonia.Tests;

public class AvaloniaGuiDispatcherInvokeAsyncTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

    [AvaloniaFact]
    public async Task InvokeAsync_FuncTask_DoesNotBlockUiThread_AndAwaitsInnerTask()
    {
        var dispatcher = new AvaloniaGuiDispatcher();
        var callbackStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var callbackGate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var postExecuted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var invokeAsyncCompleted = false;

        var invokeTask = Task.Run(async () =>
        {
            await dispatcher.InvokeAsync(async () =>
            {
                Assert.True(Dispatcher.UIThread.CheckAccess());

                callbackStarted.SetResult();

                await callbackGate.Task.ConfigureAwait(true);

                Assert.True(Dispatcher.UIThread.CheckAccess());
            });

            invokeAsyncCompleted = true;
        });

        await callbackStarted.Task.WaitAsync(Timeout);

        dispatcher.Post(() => postExecuted.SetResult());

        await postExecuted.Task.WaitAsync(Timeout);

        Assert.False(invokeTask.IsCompleted);
        Assert.False(invokeAsyncCompleted);

        callbackGate.SetResult();

        await invokeTask.WaitAsync(Timeout);

        Assert.True(invokeAsyncCompleted);
    }

    [AvaloniaFact]
    public async Task InvokeAsync_FuncTask_OnUiThread_DoesNotBlockUiThread_AndAwaitsInnerTask()
    {
        var dispatcher = new AvaloniaGuiDispatcher();
        var callbackStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var callbackGate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var postExecuted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var invokeTask = dispatcher.InvokeAsync(async () =>
        {
            Assert.True(Dispatcher.UIThread.CheckAccess());

            callbackStarted.SetResult();

            await callbackGate.Task.ConfigureAwait(true);

            Assert.True(Dispatcher.UIThread.CheckAccess());
        });

        await callbackStarted.Task.WaitAsync(Timeout);

        dispatcher.Post(() => postExecuted.SetResult());

        await postExecuted.Task.WaitAsync(Timeout);

        Assert.False(invokeTask.IsCompleted);

        callbackGate.SetResult();

        await invokeTask.WaitAsync(Timeout);
    }
}
