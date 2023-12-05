using System.Collections.Generic;
using UnityEngine.UI;

public static class Inventory
{
    public static List<Item> items = new List<Item>();
    public static Image[] itemSlots;

    public static void InitializeInventorySlots(Image[] slots)
    {
        itemSlots = slots;
        UpdateUI();
    }

    public static void AddToInventory(Item item)
    {
        items.Add(item);
        UpdateUI();
    }

    public static void UpdateUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < items.Count)
            {
                itemSlots[i].sprite = items[i].itemIcon;
                itemSlots[i].enabled = true;
            }
            else
            {
                itemSlots[i].enabled = false; // Скрываем иконку, если слот пуст
            }
        }
    }
}
