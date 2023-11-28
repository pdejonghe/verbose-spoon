using System.Collections.Concurrent;

namespace ProceedixDemoApp.Services
{
    public class BackgroundQueue<T> : IBackgroundQueue<T> where T : class
    {
        private readonly ConcurrentQueue<T> _items = new();

        public void Enqueue(T item)
        {
            ArgumentNullException.ThrowIfNull(item);
            _items.Enqueue(item);
        }

        public T Dequeue()
        {
            var success = _items.TryDequeue(out var workItem);
            return success ? workItem : null;
        }
    }
}