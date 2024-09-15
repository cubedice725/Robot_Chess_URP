using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public abstract class MonsterMovement : AStar
{
    public virtual int AttackDistance { get; set; } = 1;
    public virtual int MovingDistance { get; set; } = 1;
    public virtual void Move()
    {
        

        if (FinalNodeList.Count != 0)
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

        Instance.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)map2DObject.moster;
    }
    public bool AttackNavigation()
    {
        // 자기 위치가 비어있어야 탐색 가능
        Instance.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)map2DObject.noting;
        PathFinding(
            new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z),
            new Vector3Int((int)player.transform.position.x, (int)player.transform.position.y, (int)player.transform.position.z),
            Vector3Int.zero,
            new Vector3Int(Instance.MapSizeX, 0, Instance.MapSizeZ)
            );

        if (FinalNodeList.Count < AttackDistance + 3 && FinalNodeList.Count != 0)
        {
            return true;
        }
        return false;
    }
    public virtual void Attack()
    {
        print("공격");
    }
}
