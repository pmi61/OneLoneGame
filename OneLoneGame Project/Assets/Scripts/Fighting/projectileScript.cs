using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    [Header("Movement values")]
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
    private Vector3 movement;
    public Vector3 Movement
    {
        set { movement = value; }
    }
    private float travelledDistance;
    private float maxDistance = 5.0f;
    [Space]
    [Header("Damage values")]
    public float damage;
    public List<string> enemyTags;
    public LayerMask layer;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        travelledDistance = 0;
        speed = StartSpeed;
        float angle = Vector3.SignedAngle(new Vector2(1, 0), movement, new Vector3(0, 0, 1)) - 45 ;
        transform.Rotate(0, 0, angle);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsInMenu)
        {
            travelledDistance += speed * Time.deltaTime;
            transform.position += movement * speed * Time.deltaTime;
            if (travelledDistance > maxDistance)
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      
            //Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyTags.Contains(collision.gameObject.tag))
        {
            collision.gameObject.GetComponent<LifeIndicators>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }


}

