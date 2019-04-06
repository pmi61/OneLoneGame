using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, который отвечает за графическую обработку содержимого мнвентаря
/// </summary>
class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// Цвет обычной ячейки инвентаря
    /// </summary>
    private Color32 DEFAULT_COLOR = new Color32(255, 255, 255, 255);
    /// <summary>
    /// Цвет выбранной ячейки инвентаря
    /// </summary>
    private Color32 SELECTED_COLOR = new Color32(200, 200, 200, 255);

    /// <summary>
    /// Панель, которая содержит ячейки инвентаря
    /// </summary>
    public Transform cellsParent;

    /// <summary>
    /// Набор обработчиков для каждой ячейки
    /// </summary>
    private InventoryCellUI[] cellsUI;

    /// <summary>
    /// Инвентарь
    /// </summary>
    Inventory inventory;

   void Start()
    {
        inventory = Inventory.instance;

        inventory.onInventoryChangedCallBack += UpdateContentUI;
        inventory.onCurrentIndexChangedCallBack += UpdateSelectedColor;

        cellsUI = cellsParent.GetComponentsInChildren<InventoryCellUI>();

        // Устанавлеваем ячейку выбранной
        cellsUI[inventory.CurrentCellIndex].gameObject.GetComponent<Image>().color = SELECTED_COLOR;
    }
    
    /// <summary>
    /// Функция обратного вызова для графического обновления содержимого
    /// </summary>
    public void UpdateContentUI()
    {
        Debug.Log("Updating inventory UI");
        for (int i = 0; i < cellsUI.Length; i++)
        {
            // Если ячейка в инвентаре пуста
            if (inventory.cells[i].IsEmpty())
            {
                // Очищаем графическое содержимое 
                cellsUI[i].Clear();
            }
            else
            {
                // Иначе, устанавливаем графическое содержимое
                cellsUI[i].Set(inventory.cells[i].itemData, inventory.cells[i].number);
            }
        }
    }

    /// <summary>
    /// Функция обратного вызова для обновления цвета выбранной ячейки
    /// </summary>
    /// <param name="prevIndex"> Индекс ячейки, которая была выбранна ранее </param>
    /// <param name="curIndex"> Индекс новой выбранной ячейки </param>
    public void UpdateSelectedColor(int prevIndex, int curIndex)
    {
        cellsUI[prevIndex].gameObject.GetComponent<Image>().color = DEFAULT_COLOR;
        cellsUI[curIndex].gameObject.GetComponent<Image>().color = SELECTED_COLOR;
    }
}