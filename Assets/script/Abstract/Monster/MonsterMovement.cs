using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public abstract class MonsterMovement : AStar
{
    int i = 1;
    public virtual int AttackDistance { get; set; } = 1;
    public virtual int MovingDistance { get; set; } = 1;
    public bool start = true;
    public virtual bool Move()
    {
        if (FinalNodeList.Count != 0)
        {
            if (i <= MovingDistance)
            {
                Instance.Map2D[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z)] = (int)map2DObject.noting;
                if (!UpdateLooking(new Vector3(FinalNodeList[i].x, transform.position.y, FinalNodeList[i].z)))
                {
                    transform.Translate(Vector3.forward * 7f * Time.deltaTime);
                    if (Vector3.Distance(transform.position, new Vector3(FinalNodeList[i].x, transform.position.y, FinalNodeList[i].z)) <= 0.1)
                    {
                        Instance.Map2D[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z)] = (int)map2DObject.moster;
                        if (0 == FinalNodeList.Count - (AttackDistance + 2 + i))
                        {
                            i = 1;
                            return false;
                        }
                        i++;
                    }
                }
            }
            else
            {
                i = 1;
                return false;
            }
        }
        else
        {
            Debug.Log("플레이어를 찾을수 없음");
        }
        return true;
    }
    public bool AttackNavigation()
    {
        // 자기 위치가 비어있어야 탐색 가능
        Instance.Map2D[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z)] = (int)map2DObject.noting;
        PathFinding(
            new Vector3Int((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), (int)Mathf.Round(transform.position.z)),
            new Vector3Int((int)Mathf.Round(player.transform.position.x), (int)Mathf.Round(player.transform.position.y), (int)Mathf.Round(player.transform.position.z)),
            Vector3Int.zero,
            new Vector3Int(Instance.MapSizeX, 0, Instance.MapSizeZ)
            );
        Instance.Map2D[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z)] = (int)map2DObject.moster;

        if (FinalNodeList.Count < AttackDistance + 3 && FinalNodeList.Count != 0)
        {
            return true;
        }
        return false;
    }
    public bool UpdateLooking(Vector3 target)
    {
        float stopAngle = 1.0f;
        Vector3 direction = (target - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float angle = Quaternion.Angle(transform.rotation, targetRotation);
        // 목표를 바라보는 조건
        if (angle > stopAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.deltaTime);
        }
        else
        {
            return false;
        }
        return true;
    }
}
