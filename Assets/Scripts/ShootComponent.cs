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
    [Tooltip("Randomize how many bullets need to be fired before the next magic bullet")]
    [SerializeField] private bool randomBulletOrder = true;
    [Tooltip("Randomize which will be the next magic bullet effect")]
    [SerializeField] private bool randomBulletType = true;

    [SerializeField] private EffectType effectType;

    [SerializeField] private int magicBulletMin = 2;
    [SerializeField] private int magicBulletMax = 4;
    
    [Header("Projectile")]
    [SerializeField] private ShootMode shootMode = ShootMode.HITSCAN;
    [SerializeField] private float bulletSpeed = 1;
    [SerializeField] private BulletPool pool;

    private float timer = 0;
    private int magicBullet;
    private EffectType nextEffect;
    private int effectCount = Enum.GetNames(typeof(EffectType)).Length;

    private void Awake()
    {
        magicBullet = randomBulletOrder ? Random.Range(magicBulletMin, magicBulletMax) : magicBulletMax;
        nextEffect = randomBulletType ? (EffectType) Random.Range(0, effectCount) : effectType;
        if(shootMode == ShootMode.PROJECTILE)
        {
            pool.OnHitCallback += OnHit;
        }
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
            magicBullet--;

            switch (shootMode)
            {
                case ShootMode.HITSCAN:
                    Shoot();
                    break;
                case ShootMode.PROJECTILE:
                    ShootProjectile();
                    break;
            }

            if(magicBullet == 0)
            {
                magicBullet = randomBulletOrder ? Random.Range(magicBulletMin, magicBulletMax) : magicBulletMax;
                nextEffect = randomBulletType ? (EffectType)Random.Range(0, effectCount) : effectType;
            }
        }
    }

    public void Shoot()
    {
        var ray = new Ray(RotationTarget.Position, RotationTarget.Transform.forward);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
            return;

        Hit(hit.point, hit.transform, ray.direction, magicBullet == 0, nextEffect);
    }

    public void ShootProjectile()
    {
        pool.ShootBullet(RotationTarget.Position, RotationTarget.Transform.forward, bulletSpeed, magicBullet == 0, nextEffect);
    }

    private void OnHit(Collider obj, Vector3 point, Vector3 direction, bool magic, EffectType effect)
    {
        Hit(point, obj.transform, direction, magic, effect);
    }

    private void Hit(Vector3 point, Transform hitTransform, Vector3 direction, bool magic, EffectType effect)
    {
        //Debug
        ShootPos = RotationTarget.Position;
        HitPos = point;
        Debug.Log(point);

        var enemy = hitTransform.GetComponent<Enemy>();

        if (magic)
        {
            var context = new EffectContext();
            var dir = direction;
            dir.y = 0;
            context.type = effect;
            context.direction = dir.normalized;
            context.strength = 2;
            context.radius = 3;
            context.point = point;

            switch (effect)
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

    Vector3 healPoint;
    void Heal(EffectContext context)
    {
        healPoint = context.point;
        var colliders = Physics.OverlapSphere(context.point, context.radius);
        foreach(var collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy == null)
                continue;

            enemy.ApplyEffect(context);
        }
    }

    Vector3 HitPos, ShootPos;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(ShootPos, HitPos);
        Gizmos.DrawSphere(HitPos, 0.1f);

        Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(healPoint, 3  );
    }

    enum ShootMode
    {
        HITSCAN,
        PROJECTILE
    }
}
