using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    void Update()
    {
        var dir = RotationTarget.Position - transform.position;
        dir.y = 0;
        transform.forward = dir.normalized;
    }
}
