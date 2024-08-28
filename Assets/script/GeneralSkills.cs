using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GeneralSkills : Skill
{
    private IObjectPool<Skill> _ManagedPool;

    public override void SkillCasting()
    {
        playerATK.RemoveSkillSelection();
        Invoke("Destroy", 5f);
    }
    public void SetManagedPool(IObjectPool<Skill> pool)
    {
        _ManagedPool = pool;
    }
    public void Destroy()
    {
        _ManagedPool.Release(this);
    }
}
