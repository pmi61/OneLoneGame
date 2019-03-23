using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public Vector3 movement;
    public float startSpeed;
    public float speed;
    public float dirChange; // время смены направления
    public int dir;
    public GameObject player;
    public float aggroDistance;
    private bool isPlayerSeen = false;

    public float health;
    public float stamina;
    public float staminaDelta;
    public LayerMask layer;

    private BoxCollider2D boxCollider;
    public GameObject arrowPrefab;

    public float arrowStrenght;
    private float last;

   struct arrow
    {
       public GameObject arr;
        public Vector3 direction;
        public float str;
        public float arrowDistance;
        public float maxDistance;
        public void Update()
        {
            arr.transform.position += direction * str * Time.deltaTime;
            arrowDistance += str * Time.deltaTime;
        }
    }
    private List<arrow> arrows;

    // Start is called before the first frame update
    void Start()
    {
        startSpeed = speed;
        dirChange = Time.time;
        boxCollider = GetComponent<BoxCollider2D>();
        last = Time.time;
        arrows = new List<arrow>();
      
}

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Destroy(this);
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < aggroDistance)
        {
            float now = Time.time;
            Debug.Log( now - last);
            if (now - last > 2.0f)
            {
                float rot = 0;
                arrow ar = new arrow
                {
                    maxDistance = 1.0f,
                    arr = Instantiate(arrowPrefab),
                    str = arrowStrenght
                };
                ar.arr.transform.position = transform.position;
                if (ar.arr.transform.position.x > player.transform.position.x)
                {
                    rot = Mathf.Asin(player.transform.position.y - (int)player.transform.position.y - ar.arr.transform.rotation.z);
                }
                else
                {
                    if (ar.arr.transform.position.y > player.transform.position.y)
                        rot = -Mathf.Acos(player.transform.position.x - (int)player.transform.position.x - ar.arr.transform.rotation.z);
                }
                ar.arr.transform.Rotate(0,0, rot*180.0f/Mathf.PI);
                ar.direction = player.transform.position - transform.position;
                arrows.Add(ar);
                last = now;
            }
           

            
            Vector2 start = transform.position;
            Vector2 end = player.transform.position - (transform.position + new Vector3(boxCollider.size.x * movement.x, boxCollider.size.y * movement.y, 0));
            boxCollider.enabled = false;                                    // выключаем коллайдер, чтоб не врезаться в самих себя
            RaycastHit2D hit = Physics2D.Linecast(start, end, layer);       // пускаем луч(а мб и нет) на MaskLayer layer, чтоб проверить на столкновение
            boxCollider.enabled = true;                                     // включаем обратно
            if (hit.transform == null)
            {
                isPlayerSeen = true;
                movement = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y, 0).normalized;
            }
            else
            {
                isPlayerSeen = false;
            }
        }
            getMoveDir();
        transform.position += movement * speed * Time.deltaTime;
        foreach (arrow arr in arrows)
        {
            arr.Update();            
            if (arr.arrowDistance > arr.maxDistance)
            {
                Destroy(arr.arr);
                arrows.Remove(arr);
            }
        }
    }

    void getMoveDir()
    {
        if (!isPlayerSeen)
        {
        float now = Time.time;
            if (now - dirChange > 0.5f) // если прошло больше 2х секунд
            {
                dirChange = Time.time;
                dir = Random.Range(0, 4);
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
                }
            }
        }
    }
}

