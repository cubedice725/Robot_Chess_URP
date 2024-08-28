using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected PlayerATK playerATK;
    protected virtual void Awake()
    {
        playerATK = FindAnyObjectByType<PlayerATK>();
    }
    public abstract void SkillCasting();
}
