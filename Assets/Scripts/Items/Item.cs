using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    [SerializeField] public Sprite itemIcon;
    [SerializeField] public GameObject itemPrefab;

    /*public void LoadResources()
    {
        itemIcon = Resources.Load<Sprite>(itemIconPath);
        itemPrefab = Resources.Load<GameObject>(itemPrefabPath);
    }*/
}
