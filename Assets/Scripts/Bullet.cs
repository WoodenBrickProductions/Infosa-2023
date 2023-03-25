using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public BulletPool pool;
    public int index;
    public bool magic;
    public EffectType effect;

    public float timer;
    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        enabled = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        enabled = false;
        Debug.Log("Projectile hit " + other.name);

        var point = other.ClosestPoint(transform.position);
        pool.OnHit(other, point, direction, magic, effect);
        gameObject.SetActive(false);
    }
}
