using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
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

    [SerializeField] private Transform weaponInfo;
    [SerializeField] private TextMeshProUGUI abilityDescription;
    private Transform[] bullets = new Transform[4];
    private Image magicBullet;

    [SerializeField] private float damagedHealtBarPunchStrenght = 25f;
    [SerializeField] private float damagedHealthBarPunchDuration = 0.15f;
    [SerializeField] private float dmgFillUpdateDelayTime = 0.5f;
    [SerializeField] private float dmgFillChangeSpeed = 0.1f;

    private void Awake()
    {
        instance = this;
        for(int i = 1; i < weaponInfo.childCount; i++)
        {
            bullets[i - 1] = weaponInfo.GetChild(i).transform;
        }

        magicBullet = bullets[3].GetComponent<Image>();
    }

    private void Start()
    {
        OnGameStateChange(GameState.Instance.GetState());
    }

    private void OnEnable()
    {
        GameState.OnStateChange += OnGameStateChange;
    }

    private void OnDisable()
    {
        GameState.OnStateChange -= OnGameStateChange;
    }

    private void OnGameStateChange(GameState.Type obj)
    {
        hpBar.gameObject.SetActive(obj.Equals(GameState.Type.InGame));
        weaponInfo.gameObject.SetActive(obj.Equals(GameState.Type.InGame));
        
        if (obj.Equals(GameState.Type.GameOver))
        {
            Fade.instance.FadeOut(1, () => { });
        }
        else if (obj.Equals(GameState.Type.Menu) || obj.Equals(GameState.Type.InGame))
        {
            Fade.instance.FadeIn(10.0f, () => { });
        }
    }

    public void Damaged()
    {
        UpdateHealth();

        //hpBar.DOPunchPosition(Vector2.down * damagedHealtBarPunchStrenght, damagedHealthBarPunchDuration);
        DOVirtual.DelayedCall(dmgFillUpdateDelayTime, () => dmgFill.DOFillAmount(RunManager.Instance._player.currentHealth / RunManager.Instance._player.maxHealth, dmgFillChangeSpeed));
    }

    public void UpdateHealth()
    {
        float currentHealth = RunManager.Instance._player.currentHealth;
        float maxHealth = RunManager.Instance._player.maxHealth;

        hpFill.fillAmount = currentHealth / maxHealth;
        hpText.text = Mathf.Clamp(currentHealth, 0, maxHealth) + "/" + maxHealth;

        dmgFill.fillAmount = hpFill.fillAmount;
    }

    public void UpdateShot(int magicBullet, EffectType effect)
    {
        if(magicBullet < 0)
        {
            abilityDescription.text = effect + "";
            foreach(var b in bullets)
            {
                b.gameObject.SetActive(true);
            }

            return;
        }

        bullets[3 - magicBullet].gameObject.SetActive(false);
    }
}
