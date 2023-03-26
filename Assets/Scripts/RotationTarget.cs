using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTarget : MonoBehaviour
{
    private static RotationTarget instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
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
