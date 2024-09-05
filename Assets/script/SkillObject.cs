using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class SkillObject : MonoBehaviour
{
    private IObjectPool<SkillObject> _ManagedPool;

    public  void SetManagedPool(IObjectPool<SkillObject> pool)
    {
        _ManagedPool = pool;
    }
    public void Destroy()
    {
        _ManagedPool.Release(this);
    }
}
