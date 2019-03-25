using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Enemy tags:")]
    public List<string> enemyTags;
    [Header("Attack properties")]
    public GameObject attackEffectPrefab;
    public float attackRadius;
    public float damage;
    public LayerMask layer;
    
    private GameObject attack;


    /// <summary>
    /// Функция-корутина для ожидания 0.2 и нанесения урона
    /// </summary>
    /// <param name="origin"></param> место, откуда производится атака
    /// <param name="direction"></param> - направление атаки 
    /// Также <param name="enemyAI"></param> нужно изменить на тот скрипт, который отвечает за хп (LifeIndicators?)
    IEnumerator DealDamage(Vector2 origin, Vector2 direction)
    {
        yield return new WaitForSeconds(0.2f);
        Collider2D[] hit = Physics2D.OverlapCircleAll(attack.transform.position, attackRadius, layer);
        foreach (Collider2D creature in hit)
            if (creature != null && enemyTags.Contains(creature.transform.tag))
                creature.GetComponent<LifeIndicators>().TakeDamage(damage); ;
    }
    /// <summary>
    /// Функция для попытки атаковать
    /// </summary>
    /// <param name="origin"></param> - место, откуда производится атака
    /// <param name="direction"></param> - направление атаки 
    public void AttemptToAttack(Vector2 origin, Vector2 direction)
    {
        attack = Instantiate(attackEffectPrefab);
        attack.transform.position = origin + direction*1.5f;
        float angle = Vector3.SignedAngle(new Vector2(1, 0), (Vector2)attack.transform.position - origin, new Vector3(0,0,1));
          attack.transform.Rotate(0,0,angle + 40);
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
    public void FireArrow(Vector2 origin, Vector2 dst, List<string> enemyTags, float startSpeed, GameObject arrowPrefab)
    {
        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = origin + (dst - origin).normalized;
        arrow.GetComponent<projectileScript>().Movement = (dst - origin).normalized;
        arrow.GetComponent<projectileScript>().StartSpeed = startSpeed;
        arrow.GetComponent<projectileScript>().damage = startSpeed * 5;
        arrow.GetComponent<projectileScript>().enemyTags = enemyTags;
    }
}
