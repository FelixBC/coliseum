using System.Threading.Tasks;

namespace Coliseum.App.Extensions;

public static class TaskExtensions
{
    public static void FireAndForget(this Task task)
    {
        _ = task.ContinueWith(t =>
        {
            if (t.IsFaulted && t.Exception != null)
            {
                // Log the exception if needed
            }
        }, TaskContinuationOptions.OnlyOnFaulted);
    }
}
