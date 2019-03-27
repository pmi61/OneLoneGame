using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : Entity
{
    /// <summary>
    /// Расстояние, начиная с которого игрок пытается автоматически поднять предметы
    /// </summary>
    private const float ATTEMPT_ADD_ITEM_RADIUS = 1.0f;

    Inventory inventory;

    [Header("UI")]
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

    void Start()
    {
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();
        speed = startSpeed;
        SetMaxValues(maxHealth, maxStamina, staminaDelta);
        // Создаём инвентарь на 5 слотов
        inventory = new Inventory(5);
    }

    void Update()
    {
        if (!GameManager.instance.isGameRunning)
        {
            return;
        }
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

            staminaUI.value =stamina;
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
        #region Input
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
                #region stamina
                if (Input.GetKeyDown(KeyCode.LeftShift) && stamina > 33.0f)
                    speed = startSpeed * 2;
                else
                if (Input.GetKeyUp(KeyCode.LeftShift) || stamina < 0.0f)
                    speed = startSpeed;
                else
                    movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

                if (stamina > -1 && speed != startSpeed)
                    Run(Time.deltaTime);
                else
                if (stamina < 100)
                    GainStamina(Time.deltaTime);
                #endregion
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pz.z = 0;
                    AttemptToAttack(transform.position, (pz - transform.position));
                }
                else
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pz.z = 0;
                   FireArrow(transform.position, pz);
                }
            }
        }
        #endregion


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
