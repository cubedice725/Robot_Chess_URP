using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerMovingState : IState
{
    private PlayerMovement _PlayerMovement;
    private RaycastHit hit;

    public PlayerMovingState(PlayerMovement playerMovement)
    {
        _PlayerMovement = playerMovement;
        Debug.Log(_PlayerMovement);
    }
    public void Enter()
    {
        _PlayerMovement.SetPlayerPlane();
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 메인 카메라를 통해 마우스 클릭한 곳의 ray 정보를 가져옴
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                _PlayerMovement.Hit = hit;

                if (hit.transform.name.StartsWith("PlayerMovePlane"))
                {
                    _PlayerMovement.Move();
                    GameManager.Instance.state = GameManager.State.Idle;
                }
            }
        }
    }
    public void Exit()
    {
        _PlayerMovement.RemovePlayerPlane();
    }
}
