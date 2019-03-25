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


    /// <summary>
    /// 
    /// </summary>
    /// <param name="_health" - максимальное здоровье, также устанавливается в качестве текущего значения></param> 
    /// <param name="_stamina" - максимальная стамина, также уст...></param>
    /// <param name="_staminaDelta" - скорость траты стамины></param>
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
    /// 
    /// </summary>
    /// <param name="damage" - урон, полученный по хп></param>
    /// <returns></returns>
    public float TakeDamage(float damage)
    {
        return health -= damage;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="heal" - значения полученного лечения></param>
    /// <returns></returns>
    public float GainHealth(float heal)
    {
        return health += heal;
    }
    /// <summary>
    ///  Используется в случаях, если стамина вычитается действиями (не бегом)
    /// </summary>
    /// <param name="staminaUsed" ></param>
    /// <returns></returns>
    public float StaminaDrain(float staminaUsed)
    {
        return stamina -= staminaUsed;
    }
    /// <summary>
    ///  Бег
    /// </summary>
    /// <param name="time" - пихать дельта тайм></param>
    /// <returns></returns>
    public float Run(float time)
    {
        return stamina -= staminaDelta * time;
    }
    /// <summary>
    /// По аналогии с получением хп
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
    ///  Естественное восстановление стамины
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public float StaminaRecover(float time)
    {
        return stamina += staminaDelta / 2.0f * time;
    }
}
