using System;
using System.Threading;
using System.Threading.Tasks;

namespace Panzerdog.Test.Assignment.Utils
{
    public class TaskQueue
    {
        private readonly SemaphoreSlim _semaphore = new(1);

        public async Task Enqueue(Func<Task> taskGenerator)
        {           
            await _semaphore.WaitAsync();
            try
            {
                await taskGenerator();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}