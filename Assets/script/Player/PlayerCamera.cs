using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.VisionOS;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private Player player;

    private float scrollSpeed = 2000.0f;
    private float mouseSpeed = 100f;
    private float scrollWheel;
    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        // ȭ�� ���󰡱�
        if (Input.GetKeyDown("space"))
        {
            cam.Follow = player.transform;
        }
    }
    private void LateUpdate()
    {
        CameraMove();

        if (0f != Input.GetAxis("Mouse ScrollWheel"))
        {
            ZoomInOut();
        }
    }
    // ī�޶� ������
    private void CameraMove()
    {
        Vector3 mousePosition = Input.mousePosition;

        // ī�޶� �������� õõ��
        if (mousePosition.x <= 0)
        {
            CamFollowNull();
            cam.transform.Translate(-Time.deltaTime * mouseSpeed, 0, 0);
        }
        // ī�޶� �������� ����
        else if (mousePosition.x <= 100)
        {
            CamFollowNull();
            cam.transform.Translate(-Time.deltaTime * mouseSpeed / 10, 0, 0);
        }

        // ī�޶� ���������� õõ��
        else if (mousePosition.x >= Screen.width - 1)
        {
            CamFollowNull();
            cam.transform.Translate(Time.deltaTime * mouseSpeed, 0, 0);
        }
        // ī�޶� ���������� ����
        else if (mousePosition.x >= Screen.width - 100)
        {
            CamFollowNull();
            cam.transform.Translate(Time.deltaTime * mouseSpeed / 10, 0, 0);
        }

        // ī�޶� ���� õõ��
        if (mousePosition.y <= 0)
        {
            CamFollowNull();
            cam.transform.Translate(0, -Time.deltaTime * mouseSpeed, 0);
        }
        // ī�޶� ���� ����
        else if (mousePosition.y <= 100)
        {
            CamFollowNull();
            cam.transform.Translate(0, -Time.deltaTime * mouseSpeed/10, 0);
        }

        // ī�޶� �Ʒ��� õõ��
        else if (mousePosition.y >= Screen.height - 1)
        {
            CamFollowNull();
            cam.transform.Translate(0, Time.deltaTime * mouseSpeed, 0);
        }
        // ī�޶� �Ʒ��� ����
        else if (mousePosition.y >= Screen.height - 100)
        {
            CamFollowNull();
            cam.transform.Translate(0, Time.deltaTime * mouseSpeed/10, 0);
        }
    }

    // ī�޶� ����, �ܾƿ�
    private void ZoomInOut()
    {
        CamFollowNull();
        scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        cam.transform.Translate(0, 0, scrollWheel * Time.deltaTime * scrollSpeed);

        if (cam.transform.position.y <= 3)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, 3, cam.transform.position.z);
        }
        if (cam.transform.position.y >= 25)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, 25, cam.transform.position.z);
        }
    }

    // ī�޶� ���󰡴°� ����
    private void CamFollowNull()
    {
        if (cam.Follow != null)
        {
            cam.Follow = null;
        }
    }
}
