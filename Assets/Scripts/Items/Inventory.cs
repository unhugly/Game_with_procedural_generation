using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public static class Inventory
{
    public static List<ItemData> items = new List<ItemData>();
    public static Image[] itemSlots;

    public static void InitializeInventorySlots(Image[] slots)
    {
        itemSlots = slots;
        UpdateUI();
    }

    public static void AddToInventory(ItemData item)
    {
        if (!ContainsItem(item))
        {
            items.Add(item);
            UpdateUI();
        }
    }

    public static void UpdateUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < items.Count)
            {
                string itemIconPath = items[i].itemIconPath;
                Sprite itemSprite = Resources.Load<Sprite>(itemIconPath);

                if (itemSprite != null)
                {
                    itemSlots[i].sprite = itemSprite;
                    itemSlots[i].enabled = true;
                }
                else
                {
                    // Обработка ошибок, если спрайт не найден
                    Debug.LogError("Sprite not found at path: " + itemIconPath);
                    itemSlots[i].enabled = false;
                }
            }
            else
            {
                itemSlots[i].enabled = false;
            }
        }
    }

    public static bool ContainsItem(ItemData item)
    {
        return items.Any(i => i.itemName == item.itemName);
    }

    public static void SaveInventory()
    {
        InventoryData inventoryData = new InventoryData(Inventory.items);

        string fileName = "inventory.json";
#if UNITY_EDITOR
        fileName = "inventory_editor.json";
#endif

        string savePath = Path.Combine(Application.persistentDataPath, fileName);
        string inventoryJson = JsonUtility.ToJson(inventoryData);

        File.WriteAllText(savePath, inventoryJson);
        Debug.Log("Inventory saved to " + savePath);
    }

    public static void LoadInventory()
    {
        string fileName = "inventory.json";
#if UNITY_EDITOR
        fileName = "inventory_editor.json";
#endif

        string savePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(savePath))
        {
            string inventoryJson = File.ReadAllText(savePath);
            InventoryData loadedInventoryData = JsonUtility.FromJson<InventoryData>(inventoryJson);

            if (loadedInventoryData != null && loadedInventoryData.items != null)
            {
                items = loadedInventoryData.items;
                UpdateUI();
            }
        }
        else
        {
            Debug.Log("No inventory save file found at " + savePath);
        }
    }
}

[System.Serializable]
public class InventoryData
{
    public List<ItemData> items;

    public InventoryData(List<ItemData> items)
    {
        this.items = items;
    }
}

