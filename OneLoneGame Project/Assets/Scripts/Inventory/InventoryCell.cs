﻿/// <summary>
/// Класс-контейнер для ячейки инвентаря
/// </summary>
public class InventoryCell
{
    /// <summary>
    /// Предмет в ячейке
    /// </summary>
    public ItemData itemData;

    /// <summary>
    /// Количество предметов в ячейке
    /// </summary>
    public int number;

    /// <summary>
    /// Проверяет, является ли ячейка пустой
    /// </summary>
    /// <returns> Возвращает true, если ячейка пустая, иначе false</returns>
    public bool IsEmpty()
    {
        if (number == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    ///  Функция, которая устанавливает ячейку пустой
    /// </summary>
    public void Clear()
    {
        number = 0;
        itemData = null;
    }
}

