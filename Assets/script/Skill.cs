using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Skill : MonoBehaviour
{
    protected PlayerATK playerATK;
    protected virtual void Awake()
    {
        playerATK = FindAnyObjectByType<PlayerATK>();
    }
    protected abstract void OnCollisionEnter(Collision collision);
    public abstract void SkillCasting();
    public abstract void SetManagedPool(IObjectPool<Skill> pool);
    protected abstract void Destroy();
}
