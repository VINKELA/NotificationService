using System.Threading.Tasks;

namespace NotificationService.Extensions
{
    public static class AsyncExtension
    {
        public static void Forget(this Task task)
        {
            if (!task.IsCompleted || task.IsFaulted) _ = ForgetAwaited(task);
            async static Task ForgetAwaited(Task task) => await task.ConfigureAwait(false);
        }

    }
}
