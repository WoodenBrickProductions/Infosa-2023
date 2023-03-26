using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ShootComponent : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnShoot;

    [SerializeField] private VisualEffect muzzleVisualEffect;

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
    [SerializeField] private BulletPool lazerPool;

    private float timer = 0;
    private int magicBullet;
    private EffectType nextEffect;
    private int effectCount = activeTypeCount;

    private void Awake()
    {
        magicBullet = randomBulletOrder ? Random.Range(magicBulletMin, magicBulletMax) : magicBulletMax;
        nextEffect = randomBulletType ? (EffectType) Random.Range(0, effectCount) : effectType;
        if(shootMode == ShootMode.PROJECTILE)
        {
            pool.OnHitCallback += OnHit;
            lazerPool.OnHitCallback += OnHit;
        }
    }

    private void Start()
    {
        HUD.instance.UpdateShot(-1, nextEffect);
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
            HUD.instance.UpdateShot(magicBullet, nextEffect);

            switch (shootMode)
            {
                case ShootMode.HITSCAN:
                    Shoot();
                    break;
                case ShootMode.PROJECTILE:
                    ShootProjectile();
                    break;
            }
            
            SoundSystem.Instance.PlaySound("fx-shoot");

            if(magicBullet == 0)
            {
                magicBullet = randomBulletOrder ? Random.Range(magicBulletMin, magicBulletMax) : magicBulletMax;
                nextEffect = randomBulletType ? (EffectType)Random.Range(0, effectCount) : effectType;
                HUD.instance.UpdateShot(-1, nextEffect);
            }
        }
    }

    public void Shoot()
    {
        var ray = new Ray(BulletOrigin.Position, ((RotationTarget.Transform.forward * 2) - BulletOrigin.Position).normalized);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
            return;

        Hit(hit.point, hit.transform, ray.direction, magicBullet == 0, nextEffect);
    }

    public void ShootProjectile()
    {
        pool.ShootBullet(BulletOrigin.Position, RotationTarget.Transform.forward, bulletSpeed, magicBullet == 0, nextEffect);

        var muzzleEffect = Instantiate(muzzleVisualEffect, BulletOrigin.Position, default, null);

        OnShoot?.Invoke();
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

        if(enemy != null)
        {
            Debug.Log("Hit enemy");
        }

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
                    ApplyToEnemy(context, enemy);
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
                case EffectType.LASER_BOUNCE:
                    ApplyToEnemy(context, enemy);
                    break;
                case EffectType.NULL:
                    break;
            }
            return;
        }

        if (enemy == null)
            return;

        enemy.TakeDamage(damage);
    }

    void ApplyToEnemy(EffectContext context, Enemy enemy)
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
        //var col1 = Physics.OverlapBox(start + (direction) / 2, new Vector3(Mathf.Min(direction.x, 1), 1, Mathf.Min(direction.z, 1)));
        //var col2 = Physics.OverlapBox(start - (direction) / 2, new Vector3(Mathf.Min(direction.x, 1), 1, Mathf.Min(direction.z, 1)));
        
        //foreach(var col in col1)
        //{
        //    var enemy = col.GetComponent<Enemy>();
        //    if (enemy == null)
        //        continue;

        //    enemy.ApplyEffect(context);
        //}

        //foreach(var col in col2)
        //{
        //    var enemy = col.GetComponent<Enemy>();
        //    if (enemy == null)
        //        continue;

        //    enemy.ApplyEffect(context);
        //}

        lazerPool.ShootBullet(start + direction / 4, direction.normalized, bulletSpeed * 2, true, EffectType.LASER_BOUNCE);
        lazerPool.ShootBullet(start - direction / 4, -direction.normalized, bulletSpeed * 2, true, EffectType.LASER_BOUNCE);

        alpha = 1;
    }

    Vector3 HitPos, ShootPos;
    float alpha = 0;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(ShootPos, HitPos);
        Gizmos.DrawSphere(HitPos, 0.1f);

        Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(healPoint, 3  );

        Gizmos.color = new Color(1, 0, 1, alpha);
        //Gizmos.DrawLine(start, end);
        //Gizmos.DrawSphere(start, 0.2f);
        //Gizmos.DrawSphere(end, 0.2f);
        var direction = end - start;
        //Gizmos.DrawCube(start + (direction) / 2, new Vector3(Mathf.Max(Mathf.Abs(direction.x), 1), 1, Mathf.Max(Mathf.Abs(direction.z), 1)));
        //Gizmos.DrawCube(start - (direction) / 2, new Vector3(Mathf.Max(Mathf.Abs(direction.x), 1), 1, Mathf.Max(Mathf.Abs(direction.z), 1)));
        if(alpha > 0)
        {
            alpha -= Time.deltaTime;
        }
    }

    enum ShootMode
    {
        HITSCAN,
        PROJECTILE
    }
    
    const int activeTypeCount = 4;
}

public enum EffectType
{
    KNOCKBACK,
    HEAL,
    SPEEDBOOST,
    LASER,

    LASER_BOUNCE,
    NULL
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