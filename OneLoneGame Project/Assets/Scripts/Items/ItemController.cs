using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс-контроллер для предметов
public class ItemController : MonoBehaviour
{
    // Минимальное расстояние между игроком и предметом для попытки добавления в инвентарь
    private const float CAN_TAKE_RADIUS = 1.0f;

    // Растояние, на которое отбразывается предмет при выбрасывании из инвентаря
    private const float DROP_DISTANCE = 1.5f;

    private GameObject playerObject;

    private Item item;

    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (!playerObject) {
            Debug.Log("Player not found for Item \"" + name + "\"");
            return;
        }

        // Если предмет рядом с пользователем, пытаемся добавить его пользователю
        if (IsPlayerNear() == true)
        {
            if (AttemptAddToPlayer())
            {
                Destroy(gameObject);
            }
        }
    }

    // Функция, которая выполняет попытку добавить предмет в инвентарь игроку
    private bool AttemptAddToPlayer()
    {
        Debug.Log("Attempt to add Item \"" + name + "\" to Player...");

        Player player = playerObject.GetComponent("Player") as Player;

        return player.AttemptAdd(item);
    }

    // Функция, которая проверяет, находится ли предмет на достаточно близком к игроку расстоянии
    private bool IsPlayerNear()
    {
        float distance = Vector2.Distance(this.transform.position, playerObject.transform.position);

        //Debug.Log("Distance between Item \"" + name + "\" and Player = " + distance.ToString());

        if (distance <= CAN_TAKE_RADIUS)
        {
            return true;
        }
        else
        {
        return false;
        }
    }
}
