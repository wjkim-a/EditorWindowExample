using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IObjectPool where T : ObjectBase
{
    private Queue<T> _pool = new Queue<T>();

    public int PoolCount => _pool.Count;

    public void Push(T obj)
    {
        _pool.Enqueue(obj);
    }

    public T Pop()
    {
        return _pool.Count > 0 ? _pool.Dequeue() : null;
    }   
}
