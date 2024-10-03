using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Base Stats")]
    public float health;
    public float damage;
    public float attackSpeed;


    public float damageLerpDuration;
    private float currentHealth;
    private float targetHealth;
    private Coroutine damageCouroutine;

    HealthUI healthUI;

    private void Awake()
    {
        healthUI = GetComponent<HealthUI>();
        currentHealth = health;
        targetHealth = health;


        healthUI.Start3DSlider(health);
        healthUI.Update2DSlider(health, currentHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDame(gameObject, damage);
        }
    }

    public void TakeDame(GameObject target, float damageAmount)
    {
        Stats targetStats = target.GetComponent<Stats>();
        targetStats.targetHealth -= damageAmount;

        if (targetStats.targetHealth <= 0)
        {
            Destroy(target.gameObject);
            CheckIfPlayerDead();
        }else if (targetStats.damageCouroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }


    private void CheckIfPlayerDead()
    {

        healthUI.Update2DSlider(health, 0);
    }
    private void StartLerpHealth()
    {
        if (damageCouroutine == null)
        {
            damageCouroutine = StartCoroutine(LerpHealth());
        }
    }

    private IEnumerator LerpHealth()
    {
        float elapsedTime = 0;
        float initialHealth = currentHealth;
        float target = targetHealth;

        while (elapsedTime < damageLerpDuration)
        {
            currentHealth = Mathf.Lerp(initialHealth, target, elapsedTime / damageLerpDuration);

            UpdateHealthUI();

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        currentHealth = target;
        UpdateHealthUI();

        damageCouroutine = null;
    }

    private void UpdateHealthUI()
    {
        healthUI.Update2DSlider(health, currentHealth);
        healthUI.Update3DSlider(currentHealth);
    }
}
