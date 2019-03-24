using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public float offset;
    public Vector3 movement;
    public float startSpeed;
    public float speed;
    public float dirChange; // время смены направления
    public int dir;
    public GameObject player;
    public float aggroDistance;
    private bool isPlayerSeen = false;

    public float health;
    public float Health
    { set
        {
            health = Health;
        }
        get
        { return health; }
    }
    public float stamina;
    public float staminaDelta;
    public LayerMask layer;

    public Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public GameObject arrowPrefab;

    public float arrowStrenght;
    private float last;

    public FieldOfView FoW;

    // Start is called before the first frame update
    void Start()
    {
        FoW = GetComponent<FieldOfView>();
        startSpeed = speed;
        dirChange = Time.time;
        boxCollider = GetComponent<BoxCollider2D>();
        last = Time.time;    
}

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameRunning)
            return;
        if (health <= 0)
            Destroy(gameObject);
         float distance = Vector2.Distance(transform.position, player.transform.position);
        if (FoW.visibleTargets.Contains(player.transform))
        {
            isPlayerSeen = true;
            float now = Time.time;
            Debug.Log(now - last);
            // пуск стрел
            #region
            if (now - last > 2.0f)
            {
                var arrow = Instantiate(arrowPrefab);
                arrow.transform.position = (player.transform.position - transform.position).normalized*0.75f + transform.position;
                arrow.GetComponent<projectileScript>().Movement = (player.transform.position - transform.position).normalized;
                arrow.GetComponent<projectileScript>().StartSpeed = arrowStrenght;
                last = now;
            }
            #endregion

            movement = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y, 0).normalized;
        }
        else
            isPlayerSeen = false;
        if (distance > offset)
            getMoveDir();
        else
            movement = new Vector3(0, 0, 0);
        rb.velocity = movement * speed;
    }

    void getMoveDir()
    {
        if (!isPlayerSeen)
        {
        float now = Time.time;
            if (now - dirChange > 1f) 
            {
                dirChange = Time.time;
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

