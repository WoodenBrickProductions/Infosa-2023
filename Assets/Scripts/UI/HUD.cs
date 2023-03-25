using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    [SerializeField] private Transform hpBar;
    [SerializeField] private Image hpFill;
    [SerializeField] private Image dmgFill;
    [SerializeField] private TextMeshProUGUI hpText;

    [SerializeField] private float damagedHealtBarPunchStrenght = 25f;
    [SerializeField] private float damagedHealthBarPunchDuration = 0.15f;
    [SerializeField] private float dmgFillUpdateDelayTime = 0.5f;
    [SerializeField] private float dmgFillChangeSpeed = 0.1f;

    private void Awake()
    {
        instance = this;
    }

    public void Damaged()
    {
        UpdateHealth();

        hpBar.DOPunchPosition(Vector2.down * damagedHealtBarPunchStrenght, damagedHealthBarPunchDuration);
        DOVirtual.DelayedCall(dmgFillUpdateDelayTime, () => dmgFill.DOFillAmount(RunManager.Instance._player.currentHealth / RunManager.Instance._player.maxHealth, dmgFillChangeSpeed));
    }

    private void UpdateHealth()
    {
        float currentHealth = RunManager.Instance._player.currentHealth;
        float maxHealth = RunManager.Instance._player.maxHealth;

        hpFill.fillAmount = currentHealth / maxHealth;
        hpText.text = currentHealth + "/" + maxHealth;

        dmgFill.fillAmount = hpFill.fillAmount;
    }
}
