using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory
{
    // Список ячеек инвентаря
    private List<Cell> inventoryCells;

    // Количество ячеек
    protected int cellNumber;

    public Inventory(int cellNumber)
    {
        this.cellNumber = cellNumber;
        inventoryCells = new List<Cell>();

        for (int i = 0; i < cellNumber; i++)
        {
            inventoryCells.Add(new Cell());
        }
    }

    // Функция для добавления предмета в инвентарь
    public bool AttemptAdd(Item item)
    {
        // Индекс первой пустой ячейки в инвентаре
        int firstEmptyCellIndex = -1;

        for (int i = 0; i < cellNumber; i++)
        {
            // Если ячейка не пустая
            if (!inventoryCells[i].IsEmpty())
            {
                // Если название предмета в текущей ячейке совпадает с названием добавляемого предмета
                // И
                // Если количество предметов в текущей ячейке меньше максимального
                if (inventoryCells[i].item.name == item.name &&
                    inventoryCells[i].number < item.maxInInvintoryCell)
                {
                    // Добавляем предмет, увеличивая количество предметов
                    inventoryCells[i].number++;
                    // Выходим из функции
                    return true;
                }
            }
            // Иначе, получается, что ячейка пустая, и, если мы ни разу не находили пустую ячеку, записываем индекс текущей
            else if (firstEmptyCellIndex == -1)
            {
                firstEmptyCellIndex = i;
            }
        }
        // Дальнейший код выполняется, если не нашли незаполненную ячейку с таким же именем

        // Если находили пустую ячейку
        if (firstEmptyCellIndex != -1)
        {
            // Добавляем предмет в неё
            inventoryCells[firstEmptyCellIndex].item = item;
            inventoryCells[firstEmptyCellIndex].number = 1;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Вывод содержимого в окно отладки
    public void PrintDebug()
    {
        Debug.Log("Inventory contains:");

        string cellString;
        for (int i = 0; i < cellNumber; i++)
        {
            cellString = "Cell №" + i + ": ";

            if (inventoryCells[i].IsEmpty())
            {
                cellString += "Empty";
            }
            else
            {
                cellString += inventoryCells[i].number.ToString() + " " + inventoryCells[i].item.name;
            }

            Debug.Log(cellString);
        }
    }

    // Ячейка инвентаря
    private class Cell
    {
        // Количество предметов в ячейке
        public int number;

        // Предмет в ячейке
        public Item item;

        public Cell()
        {
            SetEmpty();
        }


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

        public void SetEmpty()
        {
            number = 0;
        }
    }
}
