using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    [SerializeField] public Sprite itemIcon;
    [SerializeField] public GameObject itemPrefab;
}
