using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public BulletPool pool;
    public int index;

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        enabled = false;
        Debug.Log("Projectile hit " + other.name);

        var point = other.ClosestPoint(transform.position);
        pool.OnHit(other, point, point - transform.position);
        gameObject.SetActive(false);
    }
}
