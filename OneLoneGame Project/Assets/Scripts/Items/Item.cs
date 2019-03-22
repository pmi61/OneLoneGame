using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Минимальное расстояние между игроком и предметом для попытки добавления в инвентарь
    private const float MIN_DISTANCE = 1.0f;

    private GameObject playerObject;

    public SpriteRenderer spriteRenderer;

    new public string name = "Some name";
    public string description = "Some item";
    public int maxInInvintoryCell = 1;

    // Start is called before the first frame update
    void Start()
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

        return player.AttemptAdd(this);
    }

    // Функция, которая проверяет, находится ли предмет на достаточно близком к игроку расстоянии
    private bool IsPlayerNear()
    {
        float distance = Vector3.Distance(this.transform.position, playerObject.transform.position);

        //Debug.Log("Distance between Item \"" + name + "\" and Player = " + distance.ToString());

        if (distance <= MIN_DISTANCE)
        {
            return true;
        }
        else
        {
        return false;
        }
    }
}
