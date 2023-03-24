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

    private NavMeshAgent agent;

    private void Awake()
    {
        if(rotationOrigin == null)
        {
            rotationOrigin = transform.GetChild(0).transform;
        }

        agent = GetComponent<NavMeshAgent>();
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
                break;
        }
    }

    private void Die()
    {
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

    const float KNOCKBACK_MULT = 2;
    const float HEAL_MULT = 10;
    void Knockback(EffectContext context)
    {
        transform.position += context.direction * context.strength * KNOCKBACK_MULT;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + Vector3.up * 2, currentHealth / MaxHealth / 2);
    }
}

public enum EffectType
{
    KNOCKBACK,
    HEAL,
    SPEEDBOOST
}

public struct EffectContext
{
    public EffectType type;
    public Vector3 direction;
    public float strength;
    public float radius;
    public Vector3 point;
}
