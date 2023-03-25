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
            switch (shootMode)
            {
                case ShootMode.HITSCAN:
                    Shoot();
                    break;
                case ShootMode.PROJECTILE:
                    ShootProjectile();
                    break;
            }
        }
    }

    public void Shoot()
    {
        var ray = new Ray(RotationTarget.Position, RotationTarget.Transform.forward);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
            return;

        magicBullet--;
        Hit(hit.point, hit.transform, ray.direction, magicBullet == 0);
    }

    public void ShootProjectile()
    {
        pool.ShootBullet(RotationTarget.Position, RotationTarget.Transform.forward, bulletSpeed);
    }

    private void OnHit(Collider obj, Vector3 point, Vector3 direction)
    {
        Hit(point, obj.transform, direction, false);
    }

    private void Hit(Vector3 point, Transform hitTransform, Vector3 direction, bool magic)
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
            context.type = nextEffect;
            context.direction = dir;
            context.strength = 2;
            context.radius = 3;
            context.point = point;

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

            magicBullet = randomBulletOrder ? Random.Range(magicBulletMin, magicBulletMax) : magicBulletMax;
            nextEffect = randomBulletType ? (EffectType)Random.Range(0, effectCount) : effectType;
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

    enum ShootMode
    {
        HITSCAN,
        PROJECTILE
    }
}
