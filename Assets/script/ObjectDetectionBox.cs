using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectDetectionBox : MonoBehaviour
{
    protected GameSupporter gameSupporter;

    public void Awake()
    {
        gameSupporter = FindObjectOfType<GameSupporter>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.StartsWith("Map Block"))
        {
            gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.wall;
        }
        else if (collision.transform.name == "Monster")
        {
            gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.moster;
        }
        else if (collision.transform.name == "Player")
        {
            gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.player;
        }
        else
        {
            gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.noting;
        }
        collision = null;
        transform.position = new Vector3(0, -100, 0);
    }
}
