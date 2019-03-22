using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /* Заберите это отсюда в GameManager */
    public GameObject canvas;
    public GameObject deathScreen;
    /*                                    */
    public Slider healthUI;
    public Image healthUIcolor;
    public Slider staminaUI;
    public Image staminaUIcolor;

    public float StartSpeed;
    public float speed;//= .5f; выставляется через юнити, не здесь
    public float health;
    public float stamina;
    public float staminaDelta;
    public LayerMask layer;
    public Animator animator;

    /* для столкновений с объектами */
    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.

    // Start is called before the first frame update
    void Start()
    {
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();
        StartSpeed = speed;      
        
    }

    // Update is called once per frame
    void Update()   
    {
        if (healthUI.value == 0)
        {
            deathScreen.SetActive(true);
            canvas.GetComponent<Canvas>().enabled = true;
        }
        healthUIcolor.color = Color.Lerp(Color.red, Color.green, healthUI.value / healthUI.maxValue);
        staminaUI.value = stamina;
        staminaUIcolor.color = Color.Lerp(Color.black, new Color(0.2f,0.75f,1,1), staminaUI.value / staminaUI.maxValue * 100);
        Vector3 movement = new Vector3();
        if (Input.GetKeyDown(KeyCode.Escape) && healthUI.value > 0)
        {
            canvas.GetComponent<Canvas>().enabled = !canvas.GetComponent<Canvas>().enabled;
            canvas.transform.Find("ESCMenu").gameObject.SetActive(true);
            healthUI.gameObject.SetActive(!healthUI.gameObject.activeSelf);
            staminaUI.gameObject.SetActive(!staminaUI.gameObject.activeSelf);
        }
        else
        {
            if (!canvas.GetComponent<Canvas>().enabled)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && stamina > 33.0)
                    speed = StartSpeed * 2;
                if (Input.GetKeyUp(KeyCode.LeftShift))
                    speed = StartSpeed;
                if (Input.GetKey(KeyCode.W))
                    movement.y = 1;
                if (Input.GetKey(KeyCode.A))
                    movement.x = -1;
                if (Input.GetKey(KeyCode.S))
                    movement.y = -1;
                if (Input.GetKey(KeyCode.D))
                    movement.x = 1;

                if (stamina > -1 && speed != StartSpeed)
                    stamina -= staminaDelta * Time.deltaTime;
                else
                if (stamina < 100)
                    stamina += staminaDelta / 4 * Time.deltaTime;
            }
        }
        

        /* проверка на столкновение */
        Vector2 start = transform.position;
        Vector2 end = transform.position + movement * speed * Time.deltaTime;
        boxCollider.enabled = false;                                    // выключаем коллайдер, чтоб не врезаться в самих себя
        RaycastHit2D hit = Physics2D.Linecast(start, end, layer);       // пускаем луч(а мб и нет) на MaskLayer layer, чтоб проверить на столкновение
        boxCollider.enabled = true;                                     // включаем обратно

        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventory.shiftCurrentIndex(Inventory.SHIFT_LEFT);
            print("Current inventory index " + inventory.getCurrentIndex());
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.shiftCurrentIndex(Inventory.SHIFT_RIGHT);
            print("Current inventory index " + inventory.getCurrentIndex());
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Item item = inventory.removeOne();
            if (item != null)
            {
                item.drop();
                inventory.PrintDebug();
            }
        }

        if (hit.transform == null) // если нет столкновения
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Magnitude", movement.magnitude);

            transform.position += movement * speed * Time.deltaTime;
        }
        else
        {
            if (healthUI.value > 0)
                healthUI.value -= 10;
        }
    }
}
