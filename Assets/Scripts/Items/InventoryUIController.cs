using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    public Image[] itemSlots;
    public Color selectedColor = Color.yellow;
    private Color defaultColor; 
    private int currentlySelected = -1; // Текущий выбранный слот. -1 означает, что ничего не выбрано
    private ItemData currentlyEquippedItem = null;

    private void Start()
    {
        Inventory.InitializeInventorySlots(itemSlots);
        if (itemSlots.Length > 0)
        {
            defaultColor = itemSlots[0].color;
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                SelectItem(i);
            }
        }
    }

    private void SelectItem(int index)
    {
        if (currentlySelected == index)
        {
            itemSlots[index].color = defaultColor;
            currentlySelected = -1;
            UnequipItem();
            return;
        }

        if (currentlySelected != -1)
        {
            itemSlots[currentlySelected].color = defaultColor;
        }

        itemSlots[index].color = selectedColor;
        currentlySelected = index;

        EquipItem(index);
    }

    private void EquipItem(int index)
    {
        if (index < Inventory.items.Count)
        {
            ItemData itemToEquip = Inventory.items[index];

            Transform player = PlayerReference.player.transform;
            Transform rightHand = player.Find("RightHand");

            if (rightHand)
            {
                if (currentlyEquippedItem != null && currentlyEquippedItem.itemName == itemToEquip.itemName)
                {
                    UnequipItem();
                    return;
                }

                UnequipItem();

                GameObject itemPrefab = Resources.Load<GameObject>(itemToEquip.itemPrefabPath);
                if (itemPrefab != null)
                {
                    GameObject equippedItem = Instantiate(itemPrefab, rightHand.position, Quaternion.identity, rightHand);
                    equippedItem.transform.localScale = itemPrefab.transform.localScale;
                    equippedItem.transform.localRotation = Quaternion.identity;
                    equippedItem.transform.Rotate(-60f, 0f, 0f);
                    equippedItem.transform.Rotate(0f, 90f, 0f, Space.Self);

                    currentlyEquippedItem = itemToEquip;
                    PlayerController.status = Walk_Mode.Attack;
                }
                else
                {
                    Debug.LogError("Prefab not found at path: " + itemToEquip.itemPrefabPath);
                }
            }
        }
    }


    private void UnequipItem()
    {
        PlayerController.status = Walk_Mode.Walking;
        Transform rightHand = PlayerReference.player.transform.Find("RightHand");

        if (rightHand)
        {
            foreach (Transform child in rightHand)
            {
                Destroy(child.gameObject);
            }
        }

        currentlyEquippedItem = null;
    }

}
