using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory
{
    public const int SHIFT_LEFT = -1;
    public const int SHIFT_RIGHT = 1;

    // Список ячеек инвентаря
    private List<Cell> inventoryCells;

    private int currentIndex = 0;

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
                if (inventoryCells[i].item.Name == item.Name &&
                    inventoryCells[i].number < item.MaxInInvintoryCell)
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

    // Функция, которая удаляет из инвенторя один элемент по текущему индексу
    public Item removeOne()
    {
        return removeOne(currentIndex);
    }

    // Функция, которая удаляет из инвентаря один элемент по указанному индексу
    public Item removeOne(int index)
    {
        if (inventoryCells[index].IsEmpty())
        {
            return null;
        }
        else
        {
            inventoryCells[index].number--;
           
            return inventoryCells[index].item;
        }
    }

    public int getCurrentIndex()
    {
        return currentIndex;
    }

    // Функция для установки индекса текущей ячйки инвентаря
    public void setCurrentIndex(int index)
    {
        if (index < 0)
        {
            currentIndex = 0;
        }
        else if (index >= cellNumber)
        {
            currentIndex = cellNumber - 1;
        }
        else
        {
            currentIndex = index;
        }
    }

    // Функция для сдвига индекса текущей ячейки инвентаря
    public void shiftCurrentIndex(int shiftDirection)
    {
        switch (shiftDirection)
        {
            case SHIFT_LEFT:
                if (currentIndex == 0)
                {
                    currentIndex = cellNumber - 1;
                }
                else
                {
                    currentIndex--;
                }
                break;

            case SHIFT_RIGHT:
                if (currentIndex == cellNumber - 1)
                {
                    currentIndex = 0;
                }
                else
                {
                    currentIndex++;
                }
                break;
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
                cellString += inventoryCells[i].number.ToString() + " " + inventoryCells[i].item.Name;
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
