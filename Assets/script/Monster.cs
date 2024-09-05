using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MonsterMove))]
public class Monster : MonoBehaviour
{
    protected int HP { get; set; }
    protected int ATK { get; set; }
    protected int Defense { get; set; }
   

    public void Awake()
    {
        HP = 4;
        ATK = 1;
        Defense = 1;
    }
}
