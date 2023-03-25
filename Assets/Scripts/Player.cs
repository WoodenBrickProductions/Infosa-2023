using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private MovementTest movement;
    private ShootComponent shoot;

    [Header("Stats")]
    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private float maxHealth = 100;

    [SerializeField] private float currentHealth;

    private void Awake()
    {
        movement = GetComponent<MovementTest>();
        shoot = GetComponent<ShootComponent>();

        currentHealth = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        movement.currentSpeed = movementSpeed;
    }

    const float KNOCKBACK_MULT = 10;
    const float HEAL_MULT = 10;
    const float SPEEDBOOST_MULT = 1;

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
                StartCoroutine(Speedboost(context));
                break;
        }
    }

    void Knockback(EffectContext context)
    {
        movement.Knockback(context.direction, context.strength * KNOCKBACK_MULT);
    }

    IEnumerator Speedboost(EffectContext context)
    {
        movement.currentSpeed = movementSpeed * context.strength * SPEEDBOOST_MULT;

        yield return new WaitForSeconds(context.duration);

        movement.currentSpeed = movementSpeed;

        yield return 0;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void Die()
    {

    }
}
