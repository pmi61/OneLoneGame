using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Player : Entity
{
    /// <summary>
    /// Расстояние, начиная с которого игрок пытается автоматически поднять предметы
    /// </summary>
    private const float ATTEMPT_ADD_ITEM_RADIUS = 0.6f;

    /// <summary>
    /// Расстояние, на которое предмет отлетает от игрока при выбрасывании
    /// </summary>
    private const float DROP_ITEM_RADIUS = 1.8f;

    Inventory inventory;

    [Header("UI")]
    public TextMeshProUGUI clockUI;
    public Slider hungerUI;
    public Image hungerUIcolor;
    public Slider healthUI;
    public Image healthUIcolor;
    public Slider staminaUI;
    public Image staminaUIcolor;

    [Header("Hunger:")]
    [SerializeField] protected float hunger;
    [SerializeField] protected float hungerDelta;
    [SerializeField] protected float hungerDamage;

    [Header("Step sounds:")]
    UnityEngine.Object[] steps;
    float startStepTime = 0.3f;
    float stepTime;
    int stepNum;
    float lastStepTime;
    void Start()
    {
        stepNum = 0;
        stepTime = startStepTime;
        steps = new UnityEngine.Object[4];
        for (int i = 1; i < 5; i++)
        {
            string l = "Prefabs/Audio/Step";
            l += i;
           steps[i-1] = Resources.Load(l);
        }
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();
        speed = startSpeed;
        SetMaxValues(maxHealth, maxStamina, staminaDelta);
        // Создаём инвентарь на 5 слотов
        inventory = new Inventory(5);
        lastStepTime = 0;
    }

    void Update()
    {
        if (!GameManager.instance.isGameRunning)
        {
            rb.velocity = new Vector2(0,0);
            return;
        }
        now = Time.time;
        // проверка жизненых показателей
        if (!GameManager.instance.IsInMenu)
        {
            #region LifeCheck
            if (health <= 0)
            {
                GameManager.instance.GameOver();
                return;
            }
            if (hunger <= 0)
            {
                hungerUIcolor.enabled = false;
                TakeDamage(hungerDamage * Time.deltaTime);
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

            healthUI.value = health;
            healthUIcolor.color = Color.Lerp(Color.red, Color.green, healthUI.value / healthUI.maxValue);

            staminaUI.value = stamina;
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
                inventory.ShiftCurrentIndex(Inventory.SHIFT_LEFT);
                print("Current inventory index " + inventory.CurrentCellIndex);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                inventory.ShiftCurrentIndex(Inventory.SHIFT_RIGHT);
                print("Current inventory index " + inventory.CurrentCellIndex);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                DropItem();
            }
            AttemptAddItems();
            #endregion

        }
        else
            rb.velocity = new Vector2(0, 0);
        #region Input
        if (Input.GetKeyDown(KeyCode.Escape) && health > 0)
        {
            GameManager.instance.OnESC();
            healthUI.gameObject.SetActive(!healthUI.gameObject.activeSelf);
            staminaUI.gameObject.SetActive(!staminaUI.gameObject.activeSelf);
            hungerUI.gameObject.SetActive(!hungerUI.gameObject.activeSelf);
            clockUI.gameObject.SetActive(!clockUI.gameObject.activeSelf);
        }
        else
        {
            if (!GameManager.instance.IsInMenu)
            {
                #region stamina
                if (Input.GetKeyDown(KeyCode.LeftShift) && stamina > 33.0f)
                {
                    speed = startSpeed * 2;
                    stepTime = startStepTime * 0.5f;
                }
                else
                if (Input.GetKeyUp(KeyCode.LeftShift) || stamina < 0.0f)
                {
                    speed = startSpeed;
                    stepTime = startStepTime;
                }
            
            else
                movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

                if (stamina > -1 && speed != startSpeed)
                    Run(Time.deltaTime);
                else
                if (stamina < 100)
                    GainStamina(Time.deltaTime);
                #endregion
                if (Input.GetKeyDown(KeyCode.Mouse0) && (now - lastAttackTime > 1.5f))
                {
                    lastAttackTime = now;
                    Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pz.z = 0;
                    AttemptToAttack(transform.position, (pz - transform.position));
                }
                else
                    if (Input.GetKeyDown(KeyCode.Mouse1) && (now - lastAttackTime > 1.5f))
                {
                    lastAttackTime = now;
                    Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pz.z = 0;
                    FireArrow(transform.position, pz);
                }
            }
        }

        if (now - lastStepTime > stepTime && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            lastStepTime = now;
            GameObject t = Instantiate(steps[stepNum] as GameObject);
            stepNum = (stepNum + 2) % 3;
            t.transform.position = transform.position;
        }

        #endregion


    }

    /// <summary>
    /// Функция, которая пытается добавить предметы рядом с игроком в инвентарь
    /// </summary>
    public void AttemptAddItems()
    {
        Collider2D[] itemsToAdd = Physics2D.OverlapCircleAll(transform.position, ATTEMPT_ADD_ITEM_RADIUS, ItemController.layerMask);

        ItemData itemData;
        ItemController itemController;

        for (int i = 0; i < itemsToAdd.Length; i++)
        {
            itemController = itemsToAdd[i].transform.GetComponent<ItemController>();
            itemData = itemController.itemData;

            if (inventory.AttemptAdd(itemData))
            {
                Debug.Log("Item \"" + itemData.name + "\" was added");
                inventory.PrintDebug();

                Destroy(itemController.gameObject);
            }
        }
    }

    /// <summary>
    /// Функция, которая пытается извлечь предмет из текущей ячейки инвентаря и,
    /// если получилось, выбрасывает предмет
    /// </summary>
    public void DropItem()
    {
        // Достаём предмет из текущей ячейки инвентаря
        GameObject item = inventory.RemoveOne();

        // Если ячейка не пустая
        if (item != null)
        {
            // Определяем положение для повления предмета
            Vector2 offset = new Vector2(animator.GetFloat("Horizontal") * DROP_ITEM_RADIUS,
                                         animator.GetFloat("Vertical") * DROP_ITEM_RADIUS);

            Vector3 dropPosition = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, 0);

            inventory.PrintDebug();

            // Создаём предмет
            Instantiate(item.gameObject, dropPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("Current inventory cell is empty!");
        }
    }
}
