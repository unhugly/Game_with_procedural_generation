using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public Item itemDetails;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.AddToInventory(itemDetails);
            Destroy(gameObject); 
        }
    }
}

