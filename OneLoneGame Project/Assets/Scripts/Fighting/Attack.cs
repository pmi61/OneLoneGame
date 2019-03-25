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
                creature.GetComponent<LifeIndicators>().TakeDamage(10); ;
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

    public void FireArrow(Vector2 origin, Vector2 dst, List<string> enemyTags, float startSpeed, GameObject arrowPrefab)
    {
        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = origin + (dst - origin).normalized;
        arrow.GetComponent<projectileScript>().Movement = (dst - origin).normalized;
        arrow.GetComponent<projectileScript>().StartSpeed = 1;
        arrow.GetComponent<projectileScript>().enemyTags = enemyTags;
    }
}
