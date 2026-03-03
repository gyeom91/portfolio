using System;
using System.Collections.Generic;

public class NativePoolManager : Singleton<NativePoolManager>
{
    protected Dictionary<Type, Queue<Object>> _pools = new();

    public T Pop<T>(T instance, int defaultCreateCount = 10) where T : class, new()
    {
        return Pop<T>(defaultCreateCount);
    }

    public T Pop<T>(int defaultCreateCount = 10) where T : class, new()
    {
        var type = typeof(T);
        if (_pools.TryGetValue(type, out var queue) == false)
        {
            queue = new Queue<object>();

            _pools.Add(type, queue);
        }

        if (queue.Count == 0)
        {
            for (var i = 0; i < defaultCreateCount; ++i)
                queue.Enqueue(new T());
        }

        return queue.Dequeue() as T;
    }

    public void Push(Object instance)
    {
        var type = instance.GetType();
        if (_pools.TryGetValue(type, out var queue) == false)
        {
            queue = new Queue<Object>();

            _pools.Add(type, queue);
        }

        queue.Enqueue(instance);
    }
}
