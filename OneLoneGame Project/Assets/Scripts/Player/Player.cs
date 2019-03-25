using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Расстояние, начиная с которого игрок пытается автоматически поднять предметы
    /// </summary>
    private const float ATTEMPT_ADD_ITEM_RADIUS = 1.0f;

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
    public GameObject arrowPrefab;
    public GameObject slashPrefab;
    public float arrowStrength;
    public float slashDamage;
    public float attackRadius;
    private Vector2 direct;
    public LayerMask layer;
    public Animator animator;
    public List<string> enemyTags;

    private LifeIndicators LI;
    void Start()
    {
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();
        speed = startSpeed;
        LI = GetComponent<LifeIndicators>();
        LI.SetMaxValues(maxHealth, maxStamina, staminaDelta);

        // Создаём инвентарь на 5 слотов
        inventory = new Inventory(5);
    }

    void Update()
    {
        if (!GameManager.instance.isGameRunning)
        {
            return;
        }
        Vector3 movement;
        // проверка жизненых показателей
        if (!GameManager.instance.IsInMenu)
        {
            #region LifeCheck
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
            #region Interface
            hungerUI.value = hunger;
            hungerUIcolor.color = Color.Lerp(Color.black, Color.yellow, hungerUI.value / hungerUI.maxValue);

            healthUI.value = LI.Health;
            healthUIcolor.color = Color.Lerp(Color.red, Color.green, healthUI.value / healthUI.maxValue);

            staminaUI.value = LI.Stamina;
            staminaUIcolor.color = Color.Lerp(Color.black, new Color(0.2f, 0.75f, 1, 1), staminaUI.value / staminaUI.maxValue * 100);
            #endregion

            // движение
            #region Movement
            movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
            if (movement != Vector3.zero)
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);

            }
            animator.SetFloat("Magnitude", movement.normalized.magnitude);
            rb.velocity = movement.normalized * speed;
            #endregion

            // Для отладки работы инвентаря
            #region Inventory
            if (Input.GetKeyDown(KeyCode.Q))
            {
                inventory.shiftCurrentIndex(Inventory.SHIFT_LEFT);
                print("Current inventory index " + inventory.CurrentCellIndex);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                inventory.shiftCurrentIndex(Inventory.SHIFT_RIGHT);
                print("Current inventory index " + inventory.CurrentCellIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Item item = inventory.removeOne();
                if (item != null)
                {

                    inventory.PrintDebug();
                }
            }
            AttemptAddItems();
            #endregion

        }
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
                #region stamina
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
                #endregion
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pz.z = 0;
                    GetComponent<Attack>().AttemptToAttack(transform.position, (pz - transform.position).normalized);
                }
                else
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pz.z = 0;
                    GetComponent<Attack>().FireArrow(transform.position, pz, enemyTags, arrowStrength, arrowPrefab);
                }
            }
        }
        

    }

    /// <summary>
    /// Функция, которая пытается добавить предметы рядом с игроком в инвентарь
    /// </summary>
    public void AttemptAddItems()
    {
        Collider2D[] itemsToAdd = Physics2D.OverlapCircleAll(transform.position, ATTEMPT_ADD_ITEM_RADIUS, Item.layerMask);

        Item item;
        for (int i = 0; i < itemsToAdd.Length; i++)
        {
            item = itemsToAdd[i].transform.GetComponent<Item>();
            Debug.Log("Item \"" + item.name + "\" was added");

            if (inventory.AttemptAdd(item))
            {
                inventory.PrintDebug();
                Destroy(item.gameObject);
            }
        }
    }
}
