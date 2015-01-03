using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public interface IResetable
{
    void Reset();
}

// Broken DO NOT USE
public class ObjectPooler<T> where T : Component, new()
{
    private List<T> objects;
    T obj;

    public ObjectPooler(int initialBufferSize)
    {
        objects = new List<T>(initialBufferSize);
        obj = new T();
    }

    public virtual T Retrieve()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects.Last<T>().active)
            {
                objects.Add(obj);
                return objects.Last<T>();
            }
                
            if (!objects[i])
            {
                return objects[i];
            }
        }
        return null;
    }

    public int Count()
    {
        return objects.Count;
    }
    public void Store(T obj)
    {
        
    }
}
