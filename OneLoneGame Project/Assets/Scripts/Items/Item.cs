using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Класс для хранения информации о предмете
/// </summary>
public class Item : MonoBehaviour
{
    /// <summary>
    /// Маска слоя предметов
    /// </summary>
    public static LayerMask layerMask;

    /// <summary>
    /// Название предмета
    /// </summary>
    new public string name;

    /// <summary>
    /// Описание предмета
    /// </summary>
    public string description;

    /// <summary>
    /// Максимальное количество предметов в одной ячейке инвентаря
    /// </summary>
    public int maxInInventoryCell;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Items");
    }
}