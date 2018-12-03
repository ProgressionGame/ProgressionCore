using System.Collections.Concurrent;

namespace Progression.Util.Threading
{
    public abstract class ThreadingInterface<TInterface, TUpdate>
        where TInterface : ThreadingInterface<TInterface, TUpdate>
        where TUpdate : UpdateBase<TInterface, TUpdate>
    {
        protected readonly ConcurrentQueue<TUpdate> UpdatesQueue = new ConcurrentQueue<TUpdate>();
        

        protected abstract bool ThreadWaiting { get; }


        public void ScheduleUpdate(TUpdate update)
        {
            UpdatesQueue.Enqueue(update);
            if (ThreadWaiting) Notify();
        }

        protected abstract void Notify();


        public virtual bool Execute()
        {
            if (UpdatesQueue.Count == 0) return false;
            TUpdate update;
            while (UpdatesQueue.TryDequeue(out update)) {
                update.Execute((TInterface) this);
            }
            return true;
        }
    }
}