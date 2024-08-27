using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATK : MonoBehaviour
{
    GameSupporter gameSupporter;
    GeneralSkills generalSkills;

    private void Awake()
    {
        gameSupporter = FindAnyObjectByType<GameSupporter>();
    }
    public void OnClickGeneralSkills()
    {
        gameSupporter.skillState = generalSkills;
    }

}
