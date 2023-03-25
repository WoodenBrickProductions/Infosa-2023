using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOrigin : MonoBehaviour
{
    private static BulletOrigin instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public static Vector3 Position
    {
        get
        {
            return instance.transform.position;
        }
    }

    public static Transform Transform
    {
        get
        {
            return instance.transform;
        }
    }
}
