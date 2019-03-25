using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeIndicators : MonoBehaviour
{
    [Header("Health values:")]
    private float maxHealth;
    private float health;
    public float Health
    {
        get
        {
            return health;
        }
    }
    [Header("Stamina values:")]
    private float maxStamina;
    private float stamina;
    public float Stamina
    {
        get
        {
            return stamina;
        }
    }
    private float staminaDelta;
    [Header("Bool values")]
    private bool isSet = false;

    public void SetMaxValues(float _health, float _stamina, float _staminaDelta)
    {
        if (isSet)
            return;
        maxHealth = _health;
        maxStamina = _stamina;
        staminaDelta = _staminaDelta;

        health = maxHealth;
        stamina = maxStamina;
        isSet = true;
    }

    public float TakeDamage(float damage)
    {
        return health -= damage;
    }

    public float GainHealth(float heal)
    {
        return health += heal;
    }

    public float StaminaDrain(float staminaUsed)
    {
        return stamina -= staminaUsed;
    }

    public float Run(float time)
    {
        return stamina -= staminaDelta * time;
    }

    public float GainStamina(float staminaGain)
    {
        stamina += staminaGain;
        if (stamina > maxStamina)
            stamina = maxStamina;
        return stamina;
    }

    public float StaminaRecover(float time)
    {
        return stamina += staminaDelta / 2.0f * time;
    }


}
