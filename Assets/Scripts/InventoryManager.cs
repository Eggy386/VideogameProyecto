using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();


    [Header("Backpack")]
    public Inventory backpack;
    public int backpackSlotCount;

    [Header("Toolbar")]
    public Inventory toolbar;
    public int toolbarSlotCount;

    private void Awake()
    {
        backpack = new Inventory(backpackSlotCount);
        toolbar = new Inventory(toolbarSlotCount);

        inventoryByName.Add("Backpack", backpack);
        inventoryByName.Add("Toolbar", toolbar);
    }

    public void Add(string inventoryName, Item item)
    {
        if(inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].Add(item);
        }
    }
    
    public Inventory GetInventoryByName(string inventoryName)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            return inventoryByName[inventoryName];
        }
        return null;
    }

    public int GetItemCount(string inventoryName, string itemName)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            Inventory inventory = inventoryByName[inventoryName];
            int totalCount = 0;

            foreach (var slot in inventory.slots)
            {
                if (slot.itemName == itemName)
                {
                    totalCount += slot.count;
                }
            }

            return totalCount;
        }

        return 0; // Si no encuentra el inventario o el ítem
    }

    public void RemoveItem(string inventoryName, string itemName, int quantity)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            Inventory inventory = inventoryByName[inventoryName];

            foreach (var slot in inventory.slots)
            {
                if (slot.itemName == itemName)
                {
                    int toRemove = Mathf.Min(quantity, slot.count); // Asegura que no elimines más de lo disponible
                    inventory.Remove(inventory.slots.IndexOf(slot), toRemove); // Usa el método `Remove` existente
                    quantity -= toRemove;

                    if (quantity <= 0) // Si ya hemos eliminado suficiente, salimos del bucle
                        break;
                }
            }
        }
    }

}
