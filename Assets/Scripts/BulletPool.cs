using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] int startSize;
    [SerializeField] GameObject bulletGO;
    [SerializeField] Bullet[] bullets;
    int current;

    public event Action<Collider, Vector3, Vector3> OnHitCallback;

    private void Awake()
    {
        if (startSize <= 0)
            startSize = 20;

        bullets = new Bullet[startSize];
        for(int i = 0; i < bullets.Length; i++)
        {
            var go = Instantiate(bulletGO, gameObject.transform);
            go.SetActive(false);
            var bullet = go.GetComponent<Bullet>();
            bullet.pool = this;
            bullets[i] = bullet;
        }
    }

    public void ShootBullet(Vector3 position, Vector3 direction, float speed)
    {
        var bullet = bullets[current++];
        bullet.index = current - 1;
        if (current == bullets.Length)
            current = 0;

        bullet.direction = direction;
        bullet.speed = speed;
        bullet.transform.position = position;
        bullet.gameObject.SetActive(true);
        bullet.enabled = true;
    }

    public void OnHit(Collider other, Vector3 point, Vector3 direction)
    {
        OnHitCallback?.Invoke(other, point, direction);
    }
}
