using System;   
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : SingletonMono<ObjectManager>
{
    private Player _player;
    public Player Player
    { 
        get 
        {
            return _player; 
        } 
        set
        {
            if(value != null)
                _player = value;
        }
    }

    private Dictionary<Type, IObjectPool> _pools = new Dictionary<Type, IObjectPool>();
    private List<ObjectBase> _objects = new List<ObjectBase>();    

    public void RegistObj(ObjectBase obj)
    {
        _objects.Add(obj);
    }

    public T CreateObj<T>(T obj, Transform parents = null) where T : ObjectBase
    {
        T newT_Obj = parents == null ? Instantiate(obj) : Instantiate(obj, parents);
        newT_Obj.name = $"{typeof(T)}{obj.ID}";

        RegistObj(newT_Obj);

        return newT_Obj;
    }    

    public void RemoveObj<T>(T obj) where T : ObjectBase
    {
        _objects.Remove(obj);
        Destroy(obj);
    }

    public T CreatObjWithUsePool<T>(T obj, Transform parents = null) where T : ObjectBase
    {
        T newT_Obj = GetObjByPool<T>();

        if (newT_Obj == null)
        {
            newT_Obj = parents == null ? Instantiate(obj) : Instantiate(obj, parents);
            newT_Obj.name = $"{typeof(T)}{obj.ID}";
        }

        return newT_Obj;
    }

    public void PushToPool<T>(T obj) where T : ObjectBase
    {             
        CreatePool<T>();
        ObjectPool<T> objectPool = GetPool<T>();

        obj.gameObject.SetActive(false);
        objectPool.Push(obj);
    }

    public T GetObjByPool<T>() where T : ObjectBase
    {
        ObjectPool<T> objectPool = GetPool<T>();
        if (objectPool != null && objectPool.PoolCount > 0)
        {
            T obj = objectPool.Pop();
            obj.gameObject.SetActive(true);
            return obj;
        }

        return null;
    }

    private void CreatePool<T>() where T : ObjectBase
    {
        if(_pools.ContainsKey(typeof(T)) == false)
            _pools[typeof(T)] = new ObjectPool<T>();
    }

    private ObjectPool<T> GetPool<T>() where T : ObjectBase
    {
        if (_pools.ContainsKey(typeof(T)) == false)
            return null;

        return _pools[typeof(T)] as ObjectPool<T>;
    }

    public int PoolCount<T>() where T : ObjectBase
    {
        if (_pools.ContainsKey(typeof(T)) == false)
            return 0;

        ObjectPool<T> objectPool = GetPool<T>();
        return objectPool.PoolCount;
    }
}
