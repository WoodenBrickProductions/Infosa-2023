using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform rotationOrigin;

    private void Awake()
    {
        if(rotationOrigin == null)
        {
            rotationOrigin = transform.GetChild(0).transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        var newForward = (RotationTarget.Position - rotationOrigin.transform.position);
        newForward.y = 0;
        newForward = newForward.normalized;

        if (Mathf.Abs(Vector3.Angle(rotationOrigin.transform.forward, newForward)) > 22.5f)
        {
            rotationOrigin.transform.Rotate(new Vector3(0, 45, 0));
        }
        else if(Mathf.Abs(Vector3.Angle(rotationOrigin.transform.forward, newForward)) < -22.5f)
        {
            rotationOrigin.transform.Rotate(new Vector3(0, -45, 0));
        }
    }
}
