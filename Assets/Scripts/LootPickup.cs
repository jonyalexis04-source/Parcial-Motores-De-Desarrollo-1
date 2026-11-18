using UnityEngine;

public class LootPickup : MonoBehaviour
{
    public enum LootType { HealthPotion, ManaPotion, Key, AxeUpgrade }
    public LootType lootType;
    public float amountValue = 25f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo tocó el trigger: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            HotbarManager inventory = other.GetComponent<HotbarManager>();
            if (inventory != null)
            {
                inventory.AddLoot(lootType, amountValue);
                Destroy(gameObject); 
            }
            else
            {
                Debug.Log("El jugador no tiene un componente HotbarManager.");
            }
        }
    }
}