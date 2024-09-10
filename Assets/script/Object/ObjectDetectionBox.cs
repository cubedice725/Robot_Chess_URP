using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using static GameManager;

public class ObjectDetectionBox : MonoBehaviour
{
    private IObjectPool<ObjectDetectionBox> _ManagedPool;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.StartsWith("Map Block"))
        {
            Instance.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)map2DObject.wall;
        }
        else if (collision.transform.name == "Monster")
        {
            Instance.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)map2DObject.moster;
        }
        else if (collision.transform.name == "Player")
        {
            Instance.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)map2DObject.player;
        }
        else
        {
            Instance.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)map2DObject.noting;
        }
        collision = null;
        transform.position = new Vector3(0, -100, 0);
    }
    public void SetManagedPool(IObjectPool<ObjectDetectionBox> pool)
    {
        _ManagedPool = pool;
    }
    public void Destroy()
    {
        _ManagedPool.Release(this);
    }
}
