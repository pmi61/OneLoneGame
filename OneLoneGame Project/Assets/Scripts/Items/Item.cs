using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

// Класс-контейнер для хранения свойств предмета
public class Item
{
    public Item(Item item)
    {
        Name = item.Name;
        Description = item.Description;
        MaxInInvintoryCell = item.MaxInInvintoryCell;
    }

    public Item(string name, string description, int maxInInventoryCell)
    {
        Name = name;
        Description = description;
        MaxInInvintoryCell = maxInInventoryCell;
    }

    public Item()
    {
        Name = "-";
        Description = "-";
        MaxInInvintoryCell = 1;
    }

    // Название предмета
    public string Name { get; set; }

    // Описание
    public string Description { get; set; }

    // Максимальное количество предметов в одной ячейке инвентаря
    public int MaxInInvintoryCell
    {
        get
        {
            return MaxInInvintoryCell;
        }
        set
        {
            if (value < 1)
            {
                MaxInInvintoryCell = 1;
            }
            else
            {
                MaxInInvintoryCell = value;
            }
        }
    }
}