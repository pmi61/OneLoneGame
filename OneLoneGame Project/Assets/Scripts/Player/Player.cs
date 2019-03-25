using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Inventory inventory;
    public Camera cam;

    [Header("UI")]
    public Slider hungerUI;
    public Image hungerUIcolor;
    public Slider healthUI;
    public Image healthUIcolor;
    public Slider staminaUI;
    public Image staminaUIcolor;

    [Space]
    [Header("Movement properties")]
    public Rigidbody2D rb;
    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.
    public float startSpeed;
    public float speed;
    public float Speed
    {
        set { speed = value; }
        get { return speed; }
    }

    [Space]
    [Header("Life values")]
    public float maxHealth;
    public float maxStamina;
    public float staminaDelta;
    public float hunger;
    public float Hunger
    {
        set { hunger = value; }
        get { return hunger; }
    }
    public float hungerDelta;

    [Space]
    [Header("Attack properties")]
    public GameObject slashPrefab;
    public float attackRadius;
    private Vector2 direct;
    public LayerMask layer;
    public Animator animator;
    public List<string> enemyTags;

    private LifeIndicators LI;

    public GameObject arrowPrefab;
    
    

    // Start is called before the first frame update
    void Start()
    {       
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();
        speed = startSpeed;
        LI = GetComponent<LifeIndicators>();
       LI.SetMaxValues(maxHealth, maxStamina, staminaDelta);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameRunning)
        {
            return;
        }
        // проверка жизненых показателей
        #region
        if (LI.Health <= 0)
        {
            GameManager.instance.GameOver();
        }
        if (hunger <= 0)
        {
            hungerUIcolor.enabled = false;
            LI.TakeDamage(10 * Time.deltaTime);
        }
        else
        {
            hungerUIcolor.enabled = true;
            hunger -= hungerDelta * Time.deltaTime;
        }
        #endregion

        // обновление показателей интерфейса
        #region
        hungerUI.value = hunger;
        hungerUIcolor.color = Color.Lerp(Color.black, Color.yellow, hungerUI.value / hungerUI.maxValue);

        healthUI.value =LI.Health;
        healthUIcolor.color = Color.Lerp(Color.red, Color.green, healthUI.value / healthUI.maxValue);

        staminaUI.value = LI.Stamina;
        staminaUIcolor.color = Color.Lerp(Color.black, new Color(0.2f, 0.75f, 1, 1), staminaUI.value / staminaUI.maxValue * 100);
        #endregion

        // движение
        #region
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        if (movement != Vector3.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);

        }
        animator.SetFloat("Magnitude", movement.normalized.magnitude);
        rb.velocity = movement.normalized * speed;
        #endregion

        if (Input.GetKeyDown(KeyCode.Escape) && LI.Health > 0)
        {
            GameManager.instance.OnESC();
            healthUI.gameObject.SetActive(!healthUI.gameObject.activeSelf);
            staminaUI.gameObject.SetActive(!staminaUI.gameObject.activeSelf);
        }
        else
        {
            if (!GameManager.instance.IsInMenu)
            {

                if (Input.GetKeyDown(KeyCode.LeftShift) && LI.Stamina > 33.0f)
                    speed = startSpeed * 2;
                else
                if (Input.GetKeyUp(KeyCode.LeftShift) || LI.Stamina < 0.0f)
                    speed = startSpeed;
                else
                    movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

                if (LI.Stamina > -1 && speed != startSpeed)
                    LI.Run(Time.deltaTime);
                else
                if (LI.Stamina < 100)
                    LI.GainStamina(Time.deltaTime);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pz.z = 0;                    
                    GetComponent<Attack>().AttemptToAttack(transform.position, (pz - transform.position).normalized);
                }
                else
                    if(Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pz.z = 0;
                   GameObject arrow = Instantiate(arrowPrefab);
                    arrow.transform.position = transform.position + (pz - transform.position).normalized;
                    arrow.GetComponent<projectileScript>().Movement = (pz - transform.position).normalized;
                    arrow.GetComponent<projectileScript>().StartSpeed = 1;
                    arrow.GetComponent<projectileScript>().enemyTags = enemyTags;

                }
            }
        }

        // работа инвентаря
        #region
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
        #endregion  

    }

    public bool AttemptAdd(Item item)
    {
        return true;
    }
}
