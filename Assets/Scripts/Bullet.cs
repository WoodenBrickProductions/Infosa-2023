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

    private new Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        enabled = true;
        transform.parent = null;

        if (!magic)
        {
            renderer.material.color = Color.white;
            return;
        }

        switch (effect)
        {
            case EffectType.KNOCKBACK:
                renderer.material.color = Color.black;
                break;
            case EffectType.HEAL:
                renderer.material.color = Color.yellow;
                break;
            case EffectType.SPEEDBOOST:
                renderer.material.color = Color.blue;
                break;
        }
    }

    void Deactivate()
    {
        enabled = false;
        gameObject.SetActive(false);
        transform.parent = pool.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        Deactivate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Deactivate();
        Debug.Log("Projectile hit " + collision.gameObject.name);

        var point = collision.collider.ClosestPoint(transform.position);
        
        if(magic && effect == EffectType.LASER)
        {
            direction = collision.GetContact(0).normal;
        }

        pool.OnHit(collision.collider, point, direction, magic, effect);
    }
}
