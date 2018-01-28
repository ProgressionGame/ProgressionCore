using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Progression.Util.Async
{
    public class AsyncResult<T> : INotifyCompletion, IResult<T>
    {
        private T _result;

        public T Result {
            get => _result;
            set { 
            _result = value;
                IsCompleted = true;
            }
        }

        public AsyncResult(T result)
        {
            Result = result;
            IsCompleted = true;
        }


        public AsyncResult()
        {
            
        }

        public AsyncResult<T> GetAwaiter() { return this; }

        public bool IsCompleted { get; private set; }
        object IResult.Result => Result;

        public T GetResult() => Result;
        
        

        public void OnCompleted(Action continuation)
        {
            new Task(continuation).Start();
        }
    }
}