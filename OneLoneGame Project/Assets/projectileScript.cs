using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{

    public float StartSpeed;
    public float speed;//= .5f; выставляется через юнити, не здесь  
    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.
    public Vector3 movement;
    public LayerMask layer;

    // Start is called before the first frame update
    void Start()
    {
        speed = StartSpeed;        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 start = transform.position;
        Vector2 end = transform.position + movement * speed * Time.deltaTime;
        //RaycastHit2D hit = Physics2D.Linecast(start, end, layer);       // пускаем луч(а мб и нет) на MaskLayer layer, чтоб проверить на столкновение
        transform.position += movement * speed * Time.deltaTime;
       // if (hit.transform != null)
    //        Destroy(this);
    }
}
