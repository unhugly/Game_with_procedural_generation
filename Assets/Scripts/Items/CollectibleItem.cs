using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public ItemData itemDetails;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.AddToInventory(itemDetails);
            Destroy(gameObject); 
        }
    }
}

