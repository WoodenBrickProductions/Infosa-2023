using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShootComponent : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float fireRate = 4;
    [SerializeField] private float damage = 2;
    [SerializeField] private int magicBulletMin = 2;
    [SerializeField] private int magicBulletMax = 4;
    
    private float timer = 0;
    private int magicBullet;
    private EffectType nextEffect;
    private int effectCount = Enum.GetNames(typeof(EffectType)).Length;

    private void Awake()
    {
        magicBullet = Random.Range(magicBulletMin, magicBulletMax);
        nextEffect = (EffectType) Random.Range(0, effectCount);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if(timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            timer = 1.0f / fireRate;
            Shoot();
        }
    }

    public void Shoot()
    {
        var ray = new Ray(RotationTarget.Position, RotationTarget.Transform.forward);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
            return;

        //Debug
        ShootPos = RotationTarget.Position;
        HitPos = hit.point;
        Debug.Log(hit.transform);
        magicBullet--;

        var enemy = hit.transform.GetComponent<Enemy>();
        
        if(magicBullet == 0)
        {
            var context = new EffectContext();
            var dir = ray.direction;
            dir.y = 0;
            context.type = nextEffect;
            context.direction = dir;
            context.strength = 2;
            context.radius = 3;
            context.point = hit.point;

            switch (nextEffect)
            {
                case EffectType.KNOCKBACK:
                    Knockback(context, enemy);
                    break;
                case EffectType.HEAL:
                    Heal(context);
                    break;
                case EffectType.SPEEDBOOST:
                    break;
            }

            magicBullet = Random.Range(magicBulletMin, magicBulletMax);
            nextEffect = (EffectType)Random.Range(0, effectCount);
            return;
        }

        if (enemy == null)
            return;

        enemy.TakeDamage(damage);

    }

    void Knockback(EffectContext context, Enemy enemy)
    {
        if (enemy == null)
            return;

        enemy.ApplyEffect(context);
    }

    void Heal(EffectContext context)
    {
        var colliders = Physics.OverlapSphere(context.point, context.radius);
        foreach(var collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy == null)
                return;

            enemy.ApplyEffect(context);
        }
    }

    Vector3 HitPos, ShootPos;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(ShootPos, HitPos);
        Gizmos.DrawSphere(HitPos, 0.1f);
    }
}
