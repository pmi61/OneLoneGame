using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{

    private float startSpeed;
    public float StartSpeed
    {
        set
        {
            startSpeed = value;
        }
        get
        {
            return startSpeed;
        }
    }
     private float speed;
    public GameObject player;
    private Vector3 movement;
    public Vector3 Movement
    {
        set { movement = value; }
    }
    public LayerMask layer;
    private float travelledDistance;
    private float maxDistance = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        travelledDistance = 0;
        speed = StartSpeed;        
    }

    // Update is called once per frame
    void Update()
    {
        travelledDistance += speed * Time.deltaTime;
        transform.position += movement * speed * Time.deltaTime;
        Vector2 start = transform.position;
        Vector2 end = transform.position + movement * speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Linecast(start, end, layer);       // пускаем луч(а мб и нет) на MaskLayer layer, чтоб проверить на столкновение
        if (hit.transform != null && hit.transform.tag == "Player")
        {
            player.GetComponent<Player>().Health -= 10;
            Destroy(gameObject);
        }
        else
        if (travelledDistance > maxDistance)
            Destroy(gameObject);
    }
}
