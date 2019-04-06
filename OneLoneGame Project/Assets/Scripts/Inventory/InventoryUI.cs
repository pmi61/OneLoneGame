using UnityEngine;

class InventoryUI : MonoBehaviour
{
    public Transform cellsParent;

    private InventoryCellUI[] cells;

    Inventory inventory;

   void Start()
    {
        inventory = Inventory.instance;
        inventory.onInventoryChangedCallBack += UpdateUI;

        cells = cellsParent.GetComponentsInChildren<InventoryCellUI>();
    }

    public void UpdateUI()
    {
        Debug.Log("Updating inventory UI");
        for (int i = 0; i < cells.Length; i++)
        {
            if (inventory.inventoryCells[i].IsEmpty())
            {
                cells[i].Clear();
            }
            else
            {
                cells[i].icon.sprite = inventory.inventoryCells[i].itemData.icon;
                cells[i].icon.enabled = true;
            }
        }
    }
}