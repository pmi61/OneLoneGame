using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, в котором хранится заданное количество ячеек инвентаря
/// </summary>
public class Inventory : MonoBehaviour
{
    public static Inventory instance = null;

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallBack;

    /// <summary>
    /// Код для сдвига текущей ячейки назад на одну позицию
    /// </summary>
    public const int SHIFT_LEFT = -1;

    /// <summary>
    /// Код для сдвига текущей ячейки вперёд на одну позицию
    /// </summary>
    public const int SHIFT_RIGHT = 1;

    /// <summary>
    /// Список ячеек инвентаря
    /// </summary>
    /// <seealso cref="Inventory.Cell"/>
    public List<InventoryCell> inventoryCells;

    /// <summary>
    /// Индекс текущей ячейки инвентаря
    /// </summary>
    private int currentCellIndex = 0;

    /// <summary>
    /// Количество ячеек в инвентаре
    /// </summary> 
    
    public int cellNumber;

    void Awake()
    {
        // Проверяем существование instance
        if (instance == null)
        {
            Debug.Log("Create instance of GameManager");
            // Если не существует, инициализируем
            instance = this;

            inventoryCells = new List<InventoryCell>();

            // Создаём пустые ячейки
            for (int i = 0; i < cellNumber; i++)
            {
                inventoryCells.Add(new InventoryCell());
            }
        }
        // Если instance уже существует, и он не ссылается на текущий экземпляр:
        else if (instance != this)
        {
            // Унитожаем его. Это соответствует концепции синглетного класса,
            // которая значит, что во всём приложении может быть только один экземпляр такого класса
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Свойство для задания значения индекса текущей ячейки инвентаря
    /// </summary>
    public int CurrentCellIndex
    {
        get
        {
            return currentCellIndex;
        }
        set
        {
            if (value < 0)
            {
                currentCellIndex = 0;
            }
            else if (value >= cellNumber)
            {
                currentCellIndex = cellNumber - 1;
            }
            else
            {
                currentCellIndex = value;
            }
        }
    }

    /// <summary>
    ///  Функция для добавления предмета в инвентарь.
    /// </summary>
    /// <param name="item"> Предмет, который мы хотим добавить. </param>
    /// <returns> Возвращает true, если добавить предмет получилось, иначе возвращает false</returns>
    /// <remarks> Ищет либо ячейку с таким же предметом, либо пустую ячейку</remarks>
    public bool AttemptAdd(ItemData item)
    {
        // Индекс первой пустой ячейки в инвентаре
        // -1 сигнализирует о том, что пустых ячеек нет
        int firstEmptyCellIndex = -1;

        for (int i = 0; i < cellNumber; i++)
        {
            // Если ячейка не пустая
            if (!inventoryCells[i].IsEmpty())
            {
                // Если название предмета в текущей ячейке совпадает с названием добавляемого предмета
                // И
                // Если количество предметов в текущей ячейке меньше максимального, т.е. есть место на ещё 1 такой же предмет
                if (inventoryCells[i].itemData.name == item.name &&
                    inventoryCells[i].number < inventoryCells[i].itemData.maxInInventoryCell)
                {
                    // Добавляем предмет, увеличивая количество предметов
                    inventoryCells[i].number++;

                    if (onInventoryChangedCallBack != null)
                    {
                        onInventoryChangedCallBack.Invoke();
                    }
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
            inventoryCells[firstEmptyCellIndex].itemData = item;
            inventoryCells[firstEmptyCellIndex].number = 1;

            if (onInventoryChangedCallBack != null)
            {
                onInventoryChangedCallBack.Invoke();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Функция, которая удаляет из инвенторя один элемент по текущему индексу
    /// </summary>
    /// <returns> Возвращает удалённый предмет </returns>
    public GameObject RemoveOne()
    {
        return RemoveOne(currentCellIndex);
    }

    /// <summary>
    /// Функция, которая удаляет из инвентаря один элемент по указанному индексу
    /// </summary>
    /// <param name="index"> Индекс ячейки, из которой удаляем предмет </param>
    /// <returns> Возвращает удалённый предмет. Если ячейка была пустой, возвращает null </returns>
    public GameObject RemoveOne(int index)
    {
        if (inventoryCells[index].IsEmpty())
        {
            return null;
        }
        else
        {
            inventoryCells[index].number--;

            GameObject item = Resources.Load("Prefabs/Items/" + inventoryCells[index].itemData.name) as GameObject;

            if (onInventoryChangedCallBack != null)
            {
                onInventoryChangedCallBack.Invoke();
            }
            return item;
        }
    }

    /// <summary>
    /// Функция для сдвига индекса текущей ячейки инвентаря
    /// </summary>
    /// <param name="shiftDirection"> Велчина, на которую необходимо сдвинуть влево/вправо индекс текущей ячейки инвентаря</param>
    public void ShiftCurrentIndex(int shiftDirection)
    {
        switch (shiftDirection)
        {
            case SHIFT_LEFT:
                if (currentCellIndex == 0)
                {
                    currentCellIndex = cellNumber - 1;
                }
                else
                {
                    currentCellIndex--;
                }
                break;

            case SHIFT_RIGHT:
                if (currentCellIndex == cellNumber - 1)
                {
                    currentCellIndex = 0;
                }
                else
                {
                    currentCellIndex++;
                }
                break;
        }

    }

    /// <summary>
    /// Вывод содержимого инвентаря в окно отладки
    /// </summary>
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
                cellString += inventoryCells[i].number.ToString() + " " + inventoryCells[i].itemData.name;
            }

            Debug.Log(cellString);
        }
    }
}
