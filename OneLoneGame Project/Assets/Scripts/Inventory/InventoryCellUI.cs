using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Класс, который отвечает за графическое отображение ячейки инвентаря
/// </summary>
public class InventoryCellUI : MonoBehaviour
{
    /// <summary>
    /// Изабражение для вывода иконки предмета
    /// </summary>
    public Image icon;

    /// <summary>
    /// Поле с текстом количества предметов в инвентаре
    /// </summary>
    public TextMeshProUGUI numberText;

    /// <summary>
    /// Устанавливает содержимое ячейки
    /// </summary>
    /// <param name="itemData"> Информация о предмете </param>
    /// <param name="number"> Количество предметов </param>
    public void Set(ItemData itemData, int number)
    {
        // Устанавливаем изображение в ячейке
        icon.sprite = itemData.icon;
        icon.enabled = true;

        // Устанавливаем количество предметов
        numberText.text = number.ToString();
        numberText.enabled = true;
    }

    /// <summary>
    ///  Функция, которая устанавливает ячейку пустой
    /// </summary>
    public void Clear()
    {
        icon.sprite = null;
        icon.enabled = false;

        numberText.text = "0";
        numberText.enabled = false;
    }
}
