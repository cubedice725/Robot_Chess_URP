using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SkillCasting : MonoBehaviour
{
    public void SkillGet(Skill skill)
    {
        skill.SkillCasting();
    }
}
