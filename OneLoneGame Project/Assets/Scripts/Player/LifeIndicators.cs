using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeIndicators : MonoBehaviour
{
    [Header("Health values:")]
    private float maxHealth;
   public  float health;
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
    /// <summary>
    /// Получение урона
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public float TakeDamage(float damage)
    {
        return health -= damage;
    }
    /// <summary>
    /// Получение лечения
    /// </summary>
    /// <param name="heal"></param>
    /// <returns></returns>
    public float GainHealth(float heal)
    {
        health += heal;
        if (health > maxHealth)
            health = maxHealth;
        return health;
    }
    /// <summary>
    /// Трата стамины не 
    /// </summary>
    /// <param name="staminaUsed"></param>
    /// <returns></returns>
    public float StaminaDrain(float staminaUsed)
    {
        return stamina -= staminaUsed;
    }
    /// <summary>
    /// Трата стамины от прошедшего времени
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public float Run(float time)
    {
        stamina -= staminaDelta * time;
            if (stamina < 0)
                stamina = 0;
        return stamina;
    }
    /// <summary>
    /// Получение стамины от событий (не бега)
    /// </summary>
    /// <param name="staminaGain"></param>
    /// <returns></returns>
    public float GainStamina(float staminaGain)
    {
        stamina += staminaGain;
        if (stamina > maxStamina)
            stamina = maxStamina;
        return stamina;
    }
    /// <summary>
    /// Естественное восстановление стамины по времени
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public float StaminaRecover(float time)
    {
        stamina += staminaDelta / 2.0f * time;
        if (stamina > maxStamina)
            stamina = maxStamina;
        return stamina; ;
    }


}
