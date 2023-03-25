using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float MaxHealth = 100;
    [SerializeField] private float currentHealth;
    [SerializeField] Transform rotationOrigin;
    [SerializeField] float movementSpeed = 3;
    [SerializeField] float collisionDamage = 3;
    [SerializeField] float collisionKnockback = 1;
    private NavMeshAgent agent;

    private Coroutine speedBoost;
    private EffectContext knockbackEffect = new EffectContext();

    private EnemySpawner _spawner;

    public void SetSpawner(EnemySpawner spawner)
    {
        _spawner = spawner;
    }

    private void Awake()
    {
        if(rotationOrigin == null)
        {
            rotationOrigin = transform.GetChild(0).transform;
        }

        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        currentHealth = MaxHealth;
    }

    void Update()
    {
        MoveToPlayer();
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

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Die();
        }

        if(currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
        }
    }

    public void ApplyEffect(EffectContext context)
    {
        switch (context.type)
        {
            case EffectType.KNOCKBACK:
                Knockback(context);
                break;
            case EffectType.HEAL:
                TakeDamage(-context.strength * HEAL_MULT);
                break;
            case EffectType.SPEEDBOOST:
                if (speedBoost != null)
                    StopCoroutine(speedBoost);

                speedBoost = StartCoroutine(Speedboost(context));
                break;
        }
    }

    private void Die()
    {
        _spawner.SendDeath(this);
        enabled = false;
        Destroy(gameObject);
    }

    void MoveToPlayer()
    {
        NavMeshHit hit;
        if(NavMesh.SamplePosition(RotationTarget.Position, out hit, 10, -1))
        {
            agent.SetDestination(hit.position);
        }
    }

    const float KNOCKBACK_MULT = 0.5f;
    const float HEAL_MULT = 10;
    const float SPEEDBOOST_MULT = 1;

    void Knockback(EffectContext context)
    {
        transform.position += context.direction * context.strength * KNOCKBACK_MULT;
    }

    IEnumerator Speedboost(EffectContext context)
    {
        agent.speed = movementSpeed * context.strength * SPEEDBOOST_MULT;

        yield return new WaitForSeconds(context.duration);

        agent.speed = movementSpeed;

        yield return 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player == null)
            return;

        player.TakeDamage(collisionDamage);

        knockbackEffect.strength = collisionKnockback;
        knockbackEffect.type = EffectType.KNOCKBACK;
        var dir = player.transform.position - transform.position;
        dir.y = 0;
        knockbackEffect.direction = dir.normalized;
        player.ApplyEffect(knockbackEffect);

        transform.position += -knockbackEffect.direction * knockbackEffect.strength * KNOCKBACK_MULT;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + Vector3.up * 2, currentHealth / MaxHealth / 2);
    }
}
