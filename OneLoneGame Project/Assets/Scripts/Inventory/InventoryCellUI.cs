using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Вложенный класс с информацией о ячейке инвентаря
/// </summary>
public class InventoryCellUI : MonoBehaviour
{
    public Image icon;

    /// <summary>
    /// Предмет в ячейке
    /// </summary>
    public ItemData itemData;

    /// <summary>
    /// Количество предметов в ячейке
    /// </summary>
    public int number;

    /// <summary>
    ///  Функция, которая устанавливает ячейку пустой
    /// </summary>
    public void Clear()
    {
        number = 0;
        icon.sprite = null;
        icon.enabled = false;
        itemData = null;
    }
}
