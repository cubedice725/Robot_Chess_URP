using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Skill : MonoBehaviour
{
    protected abstract void OnCollisionEnter(Collision collision);
    public abstract void SkillCasting();
    public abstract void SetManagedPool(IObjectPool<Skill> pool);
    public abstract void Destroy();
}
