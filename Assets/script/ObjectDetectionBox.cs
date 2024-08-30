using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class ObjectDetectionBox : MonoBehaviour
{
    private GameSupporter gameSupporter;
    private IObjectPool<ObjectDetectionBox> _ManagedPool;
    public void Awake()
    {
        gameSupporter = FindObjectOfType<GameSupporter>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.StartsWith("Map Block"))
        {
            gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2DObject.wall;
        }
        else if (collision.transform.name == "Monster")
        {
            gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2DObject.moster;
        }
        else if (collision.transform.name == "Player")
        {
            gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2DObject.player;
        }
        else
        {
            gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2DObject.noting;
        }
        collision = null;
        transform.position = new Vector3(0, -100, 0);
    }
    public void SetManagedPool(IObjectPool<ObjectDetectionBox> pool)
    {
        _ManagedPool = pool;
    }
    protected void Destroy()
    {
        _ManagedPool.Release(this);
    }
}
