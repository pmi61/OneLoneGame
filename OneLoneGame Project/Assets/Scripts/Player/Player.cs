using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Inventory inventory;

    public Slider hungerUI;
    public Image hungerUIcolor;
    public Slider healthUI;
    public Image healthUIcolor;
    public Slider staminaUI;
    public Image staminaUIcolor;

    public float startSpeed;
    public float speed;
    public float Speed
    {
        set { speed = value; }
        get { return speed; }
    }
    public float hunger;
    public float Hunger
    {
        set { hunger = value; }
        get { return hunger; }
    }
    public float hungerDelta;
    public float health;
    public float Health
    {
        set
        {
            health = value;
        }
        get
        {
            return health;
        }
    }
    public float stamina;
    public float staminaDelta;


    public float attackRadius;
    private Vector2 direct;
    public LayerMask layer;
    public Animator animator;

    /* для столкновений с объектами */
    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.

    // Start is called before the first frame update
    void Start()
    {
        // facing = new Vector2(0, -1);
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();
        speed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.instance.isGameRunning)
        {
            return;
        }




        // if (facing.x == -1 && facing.y == 0)
        //    gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("horse1_19") as Sprite;
        //if (facing.x == 1 && facing.y == 0)
        //    gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("horse1_31") as Sprite;
        //if (facing.y == -1 && facing.x == 0)
        //    gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("horse1_49") as Sprite;
        //if (facing.y == 1 && facing.x == 0)
        //    gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("horse1_37") as Sprite;


        if (healthUI.value <= 0)
        {
            GameManager.instance.GameOver();

        }
        // если голод
        if (hunger <= 0)
            health -= 10 * Time.deltaTime;

        if (hunger > 0)
            hunger -= hungerDelta * Time.deltaTime;
        hungerUI.value = hunger;
        hungerUIcolor.color = Color.Lerp(Color.black, Color.yellow, hungerUI.value / hungerUI.maxValue);

        healthUI.value = health;
        healthUIcolor.color = Color.Lerp(Color.red, Color.green, healthUI.value / healthUI.maxValue);

        staminaUI.value = stamina;
        staminaUIcolor.color = Color.Lerp(Color.black, new Color(0.2f, 0.75f, 1, 1), staminaUI.value / staminaUI.maxValue * 100);

        // Для определения направления движения
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        if (Input.GetKeyDown(KeyCode.Escape) && health > 0)
        {
            GameManager.instance.OnESC();
            healthUI.gameObject.SetActive(!healthUI.gameObject.activeSelf);
            staminaUI.gameObject.SetActive(!staminaUI.gameObject.activeSelf);
        }
        else
        {
            if (!GameManager.instance.IsInMenu)
            {

                if (Input.GetKeyDown(KeyCode.LeftShift) && stamina > 33.0f)
                    speed = startSpeed * 2;
                else
                if (Input.GetKeyUp(KeyCode.LeftShift) || stamina < 0.0f)
                    speed = startSpeed;
                else
                if (Input.GetKey(KeyCode.W))
                {
                    movement.y = 1;
                    direct = new Vector2(0.5f, 0);
                }
                else
                if (Input.GetKey(KeyCode.A))
                {
                    direct = new Vector2(0.5f, 0);
                    movement.x = -1;
                }
                else
                if (Input.GetKey(KeyCode.S))
                {
                    movement.y = -1;
                    direct = new Vector2(0.5f, 0);
                }
                else
                if (Input.GetKey(KeyCode.D))
                {
                    movement.x = 1;
                    direct = new Vector2(0.5f, 0);
                }

                if (stamina > -1 && speed != startSpeed)
                    stamina -= staminaDelta * Time.deltaTime;
                else
                if (stamina < 100)
                    stamina += staminaDelta / 4 * Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    AttemptAttack(movement);
                }
            }
        }

        /* проверка на столкновение */
        Vector2 start = transform.position;
        Vector2 end = transform.position + movement * speed * Time.deltaTime + new Vector3(boxCollider.size.x * movement.x, boxCollider.size.y * movement.y, 0);
        boxCollider.enabled = false;                                    // выключаем коллайдер, чтоб не врезаться в самих себя
        RaycastHit2D hit = Physics2D.Linecast(start, end, layer);       // пускаем луч(а мб и нет) на MaskLayer layer, чтоб проверить на столкновение
        boxCollider.enabled = true;                                     // включаем обратно

        // Для проверки работы инвентаря
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

                inventory.PrintDebug();
            }
        }

        if (hit.transform == null) // если нет столкновения
        {
            if (movement != Vector3.zero)
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
            }
            animator.SetFloat("Magnitude", movement.normalized.magnitude);

            transform.position += movement.normalized * speed * Time.deltaTime;
        }
        else
        {
            if (health > 0)
                health -= 10;
        }
    }

    public bool AttemptAdd(Item item)
    {
        return true;
    }

    private void AttemptAttack(Vector2 directionRange)
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        boxCollider.enabled = false;
        Vector2 size = boxCollider.size;
        //size += direction;
        Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position + directionRange, attackRadius);
        // RaycastHit2D hit = Physics2D.BoxCast(origin, boxCollider.size * 2, 0, direction);
        boxCollider.enabled = true;
        if (hit != null && hit.transform.tag == "Enemy")
            hit.GetComponent<enemyAI>().health -= 10;
    }

}
