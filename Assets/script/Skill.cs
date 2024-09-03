using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Skill : MonoBehaviour
{
    protected Player player;
    protected PlayerLookAt playerLookAt;

    protected void Awake()
    {
        player = FindAnyObjectByType<Player>();
        playerLookAt = FindAnyObjectByType<PlayerLookAt>();
    }
    protected abstract void OnCollisionEnter(Collision collision);
    public abstract void SkillCasting(Transform monster);
    public abstract void SetManagedPool(IObjectPool<Skill> pool);
    public abstract void Destroy();
}
