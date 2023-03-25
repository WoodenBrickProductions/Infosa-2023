using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private MovementTest movement;
    private ShootComponent shoot;

    [Header("Stats")]
    [SerializeField] private float movementSpeed = 10;
    public float maxHealth = 3;
    public float currentHealth;

    private Coroutine speedBoost;

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
                if (speedBoost != null)
                    StopCoroutine(speedBoost);

                speedBoost = StartCoroutine(Speedboost(context));
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

        HUD.instance.Damaged();

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void Die()
    {
        //RunManager.Instance.RestartRun();
    }
}
