using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GeneralSkills : Skill
{
    private Vector3 _Direction = Vector3.up;
    private float _Speed = 3f;

    private IObjectPool<Skill> _ManagedPool;
    void Update()
    {
        transform.Translate(_Direction * Time.deltaTime * _Speed);
    }
    public override void SkillCasting()
    {
        playerATK.RemoveSkillSelection();
        transform.gameObject.SetActive(true);
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Destroy();
            collision = null;
        }
    }
    public override void SetManagedPool(IObjectPool<Skill> pool)
    {
        _ManagedPool = pool;
    }
    public override void Destroy()
    {
        _ManagedPool.Release(this);
    }
}
