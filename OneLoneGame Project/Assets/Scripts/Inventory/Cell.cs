using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Вложенный класс с информацией о ячейке инвентаря
/// </summary>
public class Cell
{
    /// <summary>
    /// Количество предметов в ячейке
    /// </summary>
    public int number;

    /// <summary>
    /// Предмет в ячейке
    /// </summary>
    public string itemName;

    /// <summary>
    /// Конструктор класса, устанавливает для ячейки значение, которое обозначает, что ячейка пустая
    /// </summary>
    public Cell()
    {
        SetEmpty();
    }

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
    public void SetEmpty()
    {
        number = 0;
    }
}
