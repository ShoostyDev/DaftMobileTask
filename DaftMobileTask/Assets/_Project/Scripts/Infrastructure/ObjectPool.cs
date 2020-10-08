using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool
{
    public GameObject PoolObjectsHolder;

    private Dictionary<PoolTypeEnum, List<IPoolable>> AvailableObjects = new Dictionary<PoolTypeEnum, List<IPoolable>>();

    public void Add(PoolTypeEnum type, IPoolable poolabelObject)
    {
        if (!AvailableObjects.ContainsKey(type))
        {
            AvailableObjects.Add(type, new List<IPoolable>());
        }
        poolabelObject.Deactivate();
        AvailableObjects[type].Add(poolabelObject);
    }

    public IPoolable Fetch(PoolTypeEnum type, Func<PoolTypeEnum, IPoolable> instantiateCallback)
    {
        var firstAvailable = AvailableObjects[type].FirstOrDefault(po => po.IsAvailable());
        if (firstAvailable == null && instantiateCallback != null)
        {
            var instance = instantiateCallback(type);
            Add(type, instance);
            firstAvailable = instance;
        }
        return firstAvailable;
    }

    public List<IPoolable> GetAll(PoolTypeEnum type)
    {
        return AvailableObjects[type].ToList();
    }

    public void ClearType(PoolTypeEnum type)
    {
        AvailableObjects[type] = new List<IPoolable>();
    }

    public List<IPoolable> GetAllActive(PoolTypeEnum type)
    {
        return AvailableObjects[type].Where(po => !po.IsAvailable()).ToList();
    }
}

public enum PoolTypeEnum
{
    ContinueButton,
    GameOverButton,
}
