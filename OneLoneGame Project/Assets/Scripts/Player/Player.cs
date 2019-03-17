using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;//= .5f; выставляется через юнити, не здесь
    public LayerMask layer;
    public Animator animator;

    /* для столкновений с объектами */
    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.

    // Start is called before the first frame update
    void Start()
    {
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        /* проверка на столкновение */
        Vector2 start = transform.position;
        Vector2 end = transform.position + movement * speed * Time.deltaTime;
        boxCollider.enabled = false;                                    // выключаем коллайдер, чтоб не врезаться в самих себя
        RaycastHit2D hit = Physics2D.Linecast(start, end, layer);       // пускаем луч(а мб и нет) на MaskLayer layer, чтоб проверить на столкновение
        boxCollider.enabled = true;                                     // включаем обратно

        if (hit.transform == null) // если нет столкновения
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Magnitude", movement.magnitude);

            transform.position += movement * speed * Time.deltaTime;
        }
    }
}
