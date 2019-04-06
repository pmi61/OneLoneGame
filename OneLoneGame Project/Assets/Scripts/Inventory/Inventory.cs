using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, в котором хранится заданное количество ячеек инвентаря
/// </summary>
public class Inventory : MonoBehaviour
{
    public static Inventory instance = null;

    /// <summary>
    /// Функция-делегат, которая вызывается, когда нужно обновить
    /// визуальное содержимое инвентаря
    /// </summary>
    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallBack;

    /// <summary>
    /// Функция-делегат, которая вызывается, когда нужно обновить
    /// цвет выбранной ячейки
    /// </summary>
    /// <param name="prevIndex"> Индекс ячейки, которая была выбранна ранее </param>
    /// <param name="curIndex"> Индекс новой выбранной ячейки </param>
    public delegate void OnCurrentIndexChanged(int prevIndex, int curIndex);
    public OnCurrentIndexChanged onCurrentIndexChangedCallBack;

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
    public List<InventoryCell> cells;

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

            cells = new List<InventoryCell>();

            // Создаём пустые ячейки
            for (int i = 0; i < cellNumber; i++)
            {
                cells.Add(new InventoryCell());
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
            int prevIndex = currentCellIndex;

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

            onCurrentIndexChangedCallBack.Invoke(prevIndex, currentCellIndex);
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
            if (!cells[i].IsEmpty())
            {
                // Если название предмета в текущей ячейке совпадает с названием добавляемого предмета
                // И
                // Если количество предметов в текущей ячейке меньше максимального, т.е. есть место на ещё 1 такой же предмет
                if (cells[i].itemData.name == item.name &&
                    cells[i].number < cells[i].itemData.maxInInventoryCell)
                {
                    // Добавляем предмет, увеличивая количество предметов
                    cells[i].number++;

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
            cells[firstEmptyCellIndex].itemData = item;
            cells[firstEmptyCellIndex].number = 1;

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
        if (cells[index].IsEmpty())
        {
            return null;
        }
        else
        {
            cells[index].number--;

            GameObject item = Resources.Load("Prefabs/Items/" + cells[index].itemData.name) as GameObject;

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
        int prevIndex = currentCellIndex;

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

        onCurrentIndexChangedCallBack.Invoke(prevIndex, currentCellIndex);
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

            if (cells[i].IsEmpty())
            {
                cellString += "Empty";
            }
            else
            {
                cellString += cells[i].number.ToString() + " " + cells[i].itemData.name;
            }

            Debug.Log(cellString);
        }
    }
}
