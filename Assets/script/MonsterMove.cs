using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MonsterMove : AStar
{
    public int AttackDistance { get; set; }
    public int MovingDistance { get; set; }

    protected override void Awake()
    {
        base.Awake();
        AttackDistance = 1;
        MovingDistance = 1;
    }
    
    public void Move()
    {
        GameManager.Instance.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameManager.map2DObject.noting;
        
        PathFinding(
            new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z),
            new Vector3Int((int)player.transform.position.x, (int)player.transform.position.y, (int)player.transform.position.z),
            Vector3Int.zero,
            new Vector3Int(GameManager.Instance.MapSizeX, 0, GameManager.Instance.MapSizeZ)
            );

        if (FinalNodeList.Count < AttackDistance + 3 && FinalNodeList.Count != 0)
        {
            attack();
        }
        else if (FinalNodeList.Count != 0)
        {
            for (int i = 1; i <= MovingDistance; i++)
            {
                transform.position = new Vector3(FinalNodeList[i].x, transform.position.y, FinalNodeList[i].z);
            }
        }
        else
        {
            Debug.Log("플레이어를 찾을수 없음");
        }
        GameManager.Instance.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameManager.map2DObject.moster;
    }
    public virtual void attack()
    {
        print("공격");
    }

}
