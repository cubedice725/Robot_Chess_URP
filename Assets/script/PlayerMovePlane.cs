using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerMovePlane : MonoBehaviour
{
    private IObjectPool<PlayerMovePlane> _ManagedPool;

    public void SetManagedPool(IObjectPool<PlayerMovePlane> pool)
    {
        _ManagedPool = pool;
    }
    protected void Destroy()
    {
        _ManagedPool.Release(this);
    }
}
