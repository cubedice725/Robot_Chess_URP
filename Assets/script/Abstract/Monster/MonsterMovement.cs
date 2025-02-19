using UnityEngine;
using static GameManager;

public abstract class MonsterMovement : AStar
{
    int count = 1;
    protected Monster monster;
    public bool start = true;
    protected override void Awake()
    {
        base.Awake(); 
        monster = GetComponent<Monster>();
    }
    // 실제 몬스터가 움직임
    public virtual bool UpdateMove()
    {
        // 몬스터 부터 플레이어 까지의 거리 확인
        if (FinalNodeList.Count != 0)
        {
            // MovingDistance을 통해 행동을 제약
            if (count <= monster.MovingDistance)
            {
                Instance.Map2D[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z)] = (int)map2DObject.noting;
                // 다음 위치를 바라보는게 끝나면
                if (!UpdateLooking(new Vector3(FinalNodeList[count].x, transform.position.y, FinalNodeList[count].z)))
                {
                    // 다음 위치까지 움직임
                    transform.Translate(Vector3.forward * 7f * Time.deltaTime);
                    // 다음 위치까지 몬스터의 움직임이 거리가 0.1 아래이면 도착이라 판단
                    if (Vector3.Distance(transform.position, new Vector3(FinalNodeList[count].x, transform.position.y, FinalNodeList[count].z)) <= 0.1)
                    {
                        Instance.Map2D[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z)] = (int)map2DObject.moster;

                        //만약 현 위치가 공격 사거리에 도달하면 움직임을 멈춤
                        if (0 == FinalNodeList.Count - (monster.AttackDistance + 2 + count))
                        {
                            count = 1;
                            return false;
                        }
                        count++;
                    }
                }
            }
            else
            {
                count = 1;
                return false;
            }
        }
        else
        {
            Debug.Log("플레이어를 찾을수 없음");
        }
        return true;
    }
    // 공격 사거리 확인을 위한 함수
    public bool AttackNavigation()
    {
        // 자기 위치가 비어있어야 탐색 가능
        Instance.Map2D[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z)] = (int)map2DObject.noting;
        
        // 탐색
        PathFinding(
            new Vector3Int((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), (int)Mathf.Round(transform.position.z)),
            new Vector3Int((int)Mathf.Round(player.transform.position.x), (int)Mathf.Round(player.transform.position.y), (int)Mathf.Round(player.transform.position.z)),
            Vector3Int.zero,
            new Vector3Int(Instance.MapSizeX, 0, Instance.MapSizeZ)
            );
        Instance.Map2D[(int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z)] = (int)map2DObject.moster;

        // 사거리 안에 있는지 확인
        if (FinalNodeList.Count < monster.AttackDistance + 3 && FinalNodeList.Count != 0)
        {
            return true;
        }
        return false;
    }
    // 실제로 몬스터가 회전을 함
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
