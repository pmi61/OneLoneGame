using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    [Header("Movement values:")]
    public float offset;        // расстояние, ближе которого не нужно приближаться к цели
    private Vector3 movement;    // направление движения
    public float startSpeed;     // начальная
    private float speed;         // скорость текущая (под модификаторами)
    private float lastDirChange; // время смены направления
    private int dir;             // ролл направления

    [Header("Enemies and their attributes:")]
    public List<string> enemyTags;      // список тегом противников
    public float aggroDistance;         // дистанция агра
    private bool isEnemySeen = false;   // флаг, видит ли моб противника

    [Header("Life attributes:")]
    public float maxHealth;
    public float maxStamina;
    public float staminaDelta;
    private LifeIndicators LI;

    [Header("BOdy attributes:")]
    public Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    [Header("Fight attributes:")]
    public GameObject arrowPrefab;

    public float arrowStrenght;
    private float last;

    // Start is called before the first frame update
    void Start()
    {
        speed = startSpeed;
        lastDirChange = Time.time;
        boxCollider = GetComponent<BoxCollider2D>();
        last = Time.time;
        LI = GetComponent<LifeIndicators>();
        LI.SetMaxValues(maxHealth, maxStamina, 10);
}

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameRunning || GameManager.instance.IsInMenu)
            return;
        float distance = aggroDistance;
        if (LI.Health <= 0)
        {
            GetComponent<DeathScript>().OnDeath(transform.position);
            Destroy(gameObject);
        }
        Transform nearestCreature = findClosestVisibleEnemy();

        if (nearestCreature != null)
        {
            distance = Vector2.Distance(nearestCreature.position, transform.position);
            isEnemySeen = true;
            float now = Time.time;
            // атака
            #region
            if (now - last > 2.0f)
            {
                TryToAttack(nearestCreature);
                last = now;
            }
            #endregion
            movement = new Vector3(nearestCreature.position.x - transform.position.x, nearestCreature.position.y - transform.position.y, 0).normalized;
        }
        else
            isEnemySeen = false;
        if (distance > offset)
            getMoveDir();
        else
            movement = new Vector3(0, 0, 0);
        rb.velocity = movement * speed;
    }

    private Transform findClosestVisibleEnemy()
    {
        Transform nearestCreature = null;
        if (GetComponent<FieldOfView>().visibleTargets.Count > 0)
        {          
            nearestCreature = GetComponent<FieldOfView>().visibleTargets[0];
            float distance = Vector2.Distance(transform.position, nearestCreature.position);
            foreach (Transform creature in GetComponent<FieldOfView>().visibleTargets)
            {
                if (creature != nearestCreature && distance > Vector2.Distance(transform.position, creature.position))
                {
                    nearestCreature = creature;
                    distance = Vector2.Distance(transform.position, creature.position);
                }
            }
        }
        return nearestCreature;
    }

    private void FireArrow(Transform creature)
    {
        GetComponent<Attack>().FireArrow(transform.position, creature.position, enemyTags, arrowStrenght, arrowPrefab);
    }

    private void TryToAttack(Transform creature)
    {
        GetComponent<Attack>().AttemptToAttack(transform.position, (creature.position - transform.position).normalized, enemyTags);
    }

    void getMoveDir()
    {
        if (!isEnemySeen)
        {
        float now = Time.time;
            if (now - lastDirChange > 1f) 
            {
                lastDirChange = Time.time;
                dir = Random.Range(0, 9);
                switch (dir)
                {
                    case 0:
                            movement = new Vector3(1, 0, 0);
                        break;
                    case 1:
                            movement = new Vector3(-1, 0, 0);
                        break;
                    case 2:
                            movement = new Vector3(0, 1, 0);
                        break;
                    case 3:
                            movement = new Vector3(0, -1, 0);
                        break;                
                    case 4:
                        movement = new Vector3(1, 1, 0);
                        break;
                    case 5:
                        movement = new Vector3(1, -1, 0);
                        break;
                    case 6:
                        movement = new Vector3(-1, -1, 0);
                        break;
                    case 7:
                        movement = new Vector3(-1, 1, 0);
                        break;
                    case 8:
                        movement = new Vector3(0, 0, 0);
                        break;
                }
                movement.Normalize();
            }
        }
    }
}

