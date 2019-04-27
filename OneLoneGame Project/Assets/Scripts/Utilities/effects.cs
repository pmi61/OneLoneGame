using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effects : MonoBehaviour
{
    public float lifeTime;
    public List<string> enemyTags;
    public float slashAttackRadius;
    public float slashDamage;
    public LayerMask damageLayer;
    /// <summary>
    /// Функция-корутина для ожидания 0.2 и нанесения урона
    /// </summary>
    /// <param name="origin"></param> место, откуда производится атака
    /// <param name="direction"></param> - направление атаки 
    /// Также <param name="enemyAI"></param> нужно изменить на тот скрипт, который отвечает за хп (LifeIndicators?)
    IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(0.15f);
    Collider2D[] hit = Physics2D.OverlapCircleAll(this.transform.position, slashAttackRadius, damageLayer);
        foreach (Collider2D creature in hit)
            if (creature != null && enemyTags.Contains(creature.transform.tag))
                creature.GetComponent<Entity>().TakeDamage(slashDamage);
}
// Start is called before the first frame update
void Start()
    {

        StartCoroutine(DealDamage());

    }


    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(gameObject);
    }


}
