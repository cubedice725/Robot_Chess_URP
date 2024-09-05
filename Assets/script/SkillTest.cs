using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SkillTest : Skill
{
    RaycastHit hit;
    protected override void Awake()
    {
        prefabObject = "Prefab/Skill/SkillTestObject";
        base.Awake();
    }
    public override void Enter()
    {
        playerMovement.SetSkillSelection();
    }
    public override void IStateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���� ī�޶� ���� ���콺 Ŭ���� ���� ray ������ ������
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.transform.name.StartsWith("SkillSelection"))
                {
                    playerMovement.RemoveSkillSelection();
                    skillObjectPool.Get();
                    GameManager.Instance.playerState = GameManager.PlayerState.Idle;
                }
            }
        }
    }
    public override void Exit()
    {
        playerMovement.RemoveSkillSelection();
    }
}