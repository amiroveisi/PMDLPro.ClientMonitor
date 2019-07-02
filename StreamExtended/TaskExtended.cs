using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace StreamExtended
{
    /// <summary>
    /// Mimic a Task but you can set AsyncState
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaskResult : IAsyncResult
    {
        Task Task;
        object mAsyncState;

        public TaskResult(Task pTask, object state)
        {
            Task = pTask;
            mAsyncState = state;
        }

        public object AsyncState => mAsyncState;
        public WaitHandle AsyncWaitHandle => ((IAsyncResult)Task).AsyncWaitHandle;
        public bool CompletedSynchronously => ((IAsyncResult)Task).CompletedSynchronously;
        public bool IsCompleted => Task.IsCompleted;
        public void GetResult()
        {

            try
            {
                Task.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }

    /// <summary>
    /// Mimic a Task<T> but you can set AsyncState
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaskResult<T> : IAsyncResult
    {
        Task<T> Task;
        object mAsyncState;

        public TaskResult(Task<T> pTask, object state)
        {
            Task = pTask;
            mAsyncState = state;
        }

        public object AsyncState => mAsyncState;
        public WaitHandle AsyncWaitHandle => ((IAsyncResult)Task).AsyncWaitHandle;
        public bool CompletedSynchronously => ((IAsyncResult)Task).CompletedSynchronously;
        public bool IsCompleted => Task.IsCompleted;
        public T Result
        {
            get
            {
                try
                {
                    return Task.Result;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return default(T);
                }
            }
        }
    }


}
