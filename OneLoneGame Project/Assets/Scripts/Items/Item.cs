using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

// Класс-контейнер для хранения свойств предмета
public class Item : MonoBehaviour
{
    public static LayerMask layerMask;

    // Название предмета
    new public string name;

    // Описание
    public string description;

    // Максимальное количество предметов в одной ячейке инвентаря
    public int maxInInventoryCell;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Items");
    }
}