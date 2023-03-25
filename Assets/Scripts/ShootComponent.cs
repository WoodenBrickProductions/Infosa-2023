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
        var ray = new Ray(BulletOrigin.Position, RotationTarget.Transform.forward);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
            return;

        Hit(hit.point, hit.transform, ray.direction, magicBullet == 0, nextEffect);
    }

    public void ShootProjectile()
    {
        pool.ShootBullet(BulletOrigin.Position, RotationTarget.Transform.forward, bulletSpeed, magicBullet == 0, nextEffect);
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
            context.duration = 4;

            switch (effect)
            {
                case EffectType.KNOCKBACK:
                    Knockback(context, enemy);
                    break;
                case EffectType.HEAL:
                    ApplyEffect(context);
                    break;
                case EffectType.SPEEDBOOST:
                    ApplyEffect(context);
                    break;
                case EffectType.LASER:
                    Laser(context);
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
    void ApplyEffect(EffectContext context)
    {
        healPoint = context.point;
        var colliders = Physics.OverlapSphere(context.point, context.radius);
        foreach(var collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplyEffect(context);
                continue;
            }

            var player = collider.GetComponent<Player>();
            if (player != null)
                player.ApplyEffect(context);
        }
    }

    private Vector3 start, end;
    void Laser(EffectContext context)
    {
        start = context.point;
        end = context.point + context.direction * 5;
        var direction = end - start;
        Physics.OverlapBox(start + (direction) / 2, new Vector3(Mathf.Min(direction.x, 1), 1, Mathf.Min(direction.z, 1)));
    }

    Vector3 HitPos, ShootPos;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(ShootPos, HitPos);
        Gizmos.DrawSphere(HitPos, 0.1f);

        Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(healPoint, 3  );

        Gizmos.color = new Color(1, 0, 1);
        //Gizmos.DrawLine(start, end);
        //Gizmos.DrawSphere(start, 0.2f);
        //Gizmos.DrawSphere(end, 0.2f);
        var direction = end - start;
        Gizmos.DrawWireCube(start + (direction) / 2, new Vector3(Mathf.Max(Mathf.Abs(direction.x), 1), 1, Mathf.Max(Mathf.Abs(direction.z), 1)));
    }

    enum ShootMode
    {
        HITSCAN,
        PROJECTILE
    }
}

public enum EffectType
{
    KNOCKBACK,
    HEAL,
    SPEEDBOOST,
    LASER
}

public struct EffectContext
{
    public EffectType type;
    public Vector3 direction;
    public float strength;
    public float radius;
    public Vector3 point;
    public float duration;
}