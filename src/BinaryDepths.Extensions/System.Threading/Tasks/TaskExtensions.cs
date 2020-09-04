using System;
using System.Threading;
using System.Threading.Tasks;

namespace BinaryDepths.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Allows the await call itself to be cancelled. Useful for timing out the wait for a potentially misbehaving task
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="OperationCanceledException">Thrown when the CancellationToken is canceled</exception>
        /// <returns></returns>
        public static async Task<T> WithWaitCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            void OnCancelledCallback(object state)
            {
                var source = (TaskCompletionSource<bool>)state;
                source.TrySetResult(true);
            }
            
            var completionSource = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(OnCancelledCallback, completionSource))
            {
                var completedTask = await Task.WhenAny(task, completionSource.Task).ConfigureAwait(false);
                if (completedTask == completionSource.Task)
                {
                    throw new OperationCanceledException(cancellationToken);
                }
            }
            
            return await task;
        }
    }
}
