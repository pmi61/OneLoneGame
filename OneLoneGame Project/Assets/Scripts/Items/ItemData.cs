using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Класс для хранения информации о предмете
/// </summary>

[CreateAssetMenu(fileName = "New Item", menuName = "Item Data", order = 51)]
public class ItemData : ScriptableObject
{
    /// <summary>
    /// Иконка для инвентаря
    /// </summary>
    public Sprite icon = null;

    /// <summary>
    /// Название предмета
    /// </summary>
    new public string name = "New Item";

    /// <summary>
    /// Описание предмета
    /// </summary>
    public string description = "Some Item";

    /// <summary>
    /// Максимальное количество предметов в одной ячейке инвентаря
    /// </summary>
    public int maxInInventoryCell = 1;
}