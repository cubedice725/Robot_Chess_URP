using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class GeneralSkills : Skill
{
    private IObjectPool<Skill> _ManagedPool;
    Transform _monster = null;
    private void Update()
    {
        if(_monster != null)
        {
            playerLookAt.gameObject.transform.LookAt(_monster);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(_monster.position-player.transform.position), 1f);
            print(player.transform.rotation);
            print(playerLookAt.transform.rotation);
            if(player.transform.rotation == playerLookAt.transform.rotation)
            {
                gameObject.transform.position = new Vector3(
                                player.gameObject.transform.position.x,
                                player.gameObject.transform.position.y,
                                player.gameObject.transform.position.z + 1
                                );
                gameObject.SetActive(true);
                transform.Translate(Vector3.forward * Time.deltaTime * 3f);
            }
        }
    }
    public override void SkillCasting(Transform monster)
    {
        _monster = monster;
        
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
