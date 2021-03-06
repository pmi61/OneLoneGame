﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    #region LifeIndicators
    [Header("Health values:")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    [Header("Stamina values:")]
    [SerializeField] protected float maxStamina;
    [SerializeField] protected float stamina;
    [SerializeField] protected float staminaDelta;

    public void SetMaxValues(float _health, float _stamina, float _staminaDelta)
    {
        maxHealth = _health;
        maxStamina = _stamina;
        staminaDelta = _staminaDelta;

        health = maxHealth;
        stamina = maxStamina;
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
        return stamina;
    }
    #endregion
    #region Movement
    [Header("Movement properties")]
    protected  Vector3 movement;
    public Rigidbody2D rb;
    protected BoxCollider2D boxCollider;
    [SerializeField] protected float startSpeed;
    protected float speed;
    [SerializeField] protected Animator animator;
    #endregion
    #region AttackMethods
    [Header("Enemy tags:")]
    [SerializeField] protected List<string> enemyTags;
    [Header("Attack properties")]
    [SerializeField] protected GameObject attackEffectPrefab;
    [SerializeField] protected GameObject arrowPrefab;
    [SerializeField] protected float arrowSpeed;
    [SerializeField] protected float arrowDamage;
   [SerializeField] protected float spreadAngle;
    [SerializeField] protected float slashAttackRadius;
    [SerializeField] protected float slashDamage;
    [SerializeField] protected LayerMask damageLayer;
    // Созданная атака
    protected GameObject attack;
    protected float now;
    protected float lastAttackTime;


    /// <summary>
    /// Функция-корутина для ожидания 0.2 и нанесения урона
    /// </summary>
    /// <param name="origin"></param> место, откуда производится атака
    /// <param name="direction"></param> - направление атаки 
    /// Также <param name="enemyAI"></param> нужно изменить на тот скрипт, который отвечает за хп (LifeIndicators?)
    IEnumerator DealDamage(Vector2 origin, Vector2 direction)
    {
        yield return new WaitForSeconds(0.1f);
        Collider2D[] hit = Physics2D.OverlapCircleAll(attack.transform.position, slashAttackRadius, damageLayer);
        foreach (Collider2D creature in hit)
            if (creature != null && enemyTags.Contains(creature.transform.tag))
                creature.GetComponent<Entity>().TakeDamage(slashDamage);
    }
    /// <summary>
    /// Функция для попытки атаковать
    /// </summary>
    /// <param name="origin"></param> - место, откуда производится атака
    /// <param name="direction"></param> - направление атаки 
    protected void AttemptToAttack(Vector2 origin, Vector2 direction)
    {
        direction.Normalize();
        attack = Instantiate(attackEffectPrefab);
        attack.transform.position = origin + direction;
        float angle = Vector3.SignedAngle(new Vector2(1, 0), (Vector2)attack.transform.position - origin, new Vector3(0, 0, 1));
        attack.transform.Rotate(0, 0, angle + 40);
        StartCoroutine(DealDamage(origin, direction));
    }
    /// <summary>
    /// Выстрелить снаряда
    /// </summary>
    /// <param name="origin" - место, откуда производится выстрел></param>
    /// <param name="dst" - место назначения ></param>
    /// <param name="enemyTags" - теги тех, кто будет поражен стрелой></param>
    /// <param name="startSpeed" - стартовая скорость></param>
    /// <param name="arrowPrefab" - префаб снаряда></param>
    protected void FireArrow(Vector2 origin, Vector2 dst)
    {
        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = origin + (dst - origin).normalized;
        Vector2 tmp = (dst - origin).normalized;
        float t = tmp.x;
        float angle = UnityEngine.Random.Range(-spreadAngle/2.0f, spreadAngle/2.0f);
        angle = angle * Mathf.PI / 180.0f;
        tmp.x = t * Mathf.Cos(angle) - tmp.y * Mathf.Sin(angle);
        tmp.y = t * Mathf.Sin(angle) + tmp.y * Mathf.Cos(angle);
        arrow.GetComponent<projectileScript>().Movement = tmp;
        arrow.GetComponent<projectileScript>().StartSpeed = arrowSpeed;
        arrow.GetComponent<projectileScript>().damage = arrowDamage;
        arrow.GetComponent<projectileScript>().enemyTags = enemyTags;
    }
    #endregion
    #region Death
    [SerializeField] protected List<GameObject> Loot;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position" - место спавна вещей></param>
    protected void Die()
    {
        foreach (GameObject item in Loot)
        {
            GameObject i = Instantiate(item);
            i.transform.position = transform.position;
        }
        Instantiate(Resources.Load<AudioSource>("Prefabs/Audio/DeathVoice"));
        Destroy(gameObject);
    }
    #endregion
}
