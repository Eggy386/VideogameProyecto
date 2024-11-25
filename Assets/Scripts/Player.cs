using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryManager inventory;
    public float detectionRadius = 1.0f; // Radio para detectar NPCs
    public LayerMask npcLayer; // Capa asignada a los NPCs

    private void Awake()
    {
        inventory = GetComponent<InventoryManager>();
    }

    private void Update()
    {
        // Interactuar con NPC
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckForNPC();
        }

        // Otras funcionalidades
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }

        // Labranza
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
            if (GameManager.instance.tileManager.isInteractable(position))
            {
                Debug.Log("Tile is interactable");
                GameManager.instance.tileManager.SetPlowed(position);
            }
        }

        // Plantar
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
            if (GameManager.instance.tileManager.IsPlowed(position))
            {
                Debug.Log("Player wants to plant seeds");
                int selectedSlotID = GameManager.instance.uiManager.toolBarUI.GetSelectedSlot().slotID;
                string selectedItemName = inventory.toolbar.slots[selectedSlotID].itemName;
                Debug.Log("Selected Item: " + selectedItemName);
                Item itemToPlant = GameManager.instance.itemManager.GetItemByName(selectedItemName);
                GameManager.instance.tileManager.PlantSeed(position, itemToPlant);

                if (itemToPlant != null && itemToPlant.data.isPlantable)
                    inventory.toolbar.slots[selectedSlotID].RemoveItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
            GameManager.instance.tileManager.CollectGrownPlant(position);
        }
    }

    private void CheckForNPC()
    {
        Collider2D npc = Physics2D.OverlapCircle(transform.position, detectionRadius, npcLayer);

        if (npc != null)
        {
            Debug.Log("Hay un NPC cerca: " + npc.name);
        }
        else
        {
            Debug.Log("No hay NPC cerca.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el radio de detección en el editor para facilitar el ajuste
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void DropItem(Item item)
    {
        Vector2 spawnLocation = transform.position;
        Vector2 spawnOffset = Random.insideUnitCircle.normalized * Random.Range(1.0f, 1.5f);

        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for (int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }

    private void Interact()
    {
        Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        Item plantedItemNearby = GameManager.instance.tileManager.GetPlantedItem(position);
        Debug.Log("Planted Item Nearby: " + plantedItemNearby.name);
        int growthStage = GameManager.instance.tileManager.GetPlantedItemGrowthStage(position);
        int selectedSlotID = GameManager.instance.uiManager.toolBarUI.GetSelectedSlot().slotID;
        string selectedItemName = inventory.toolbar.slots[selectedSlotID].itemName;
        Item itemInHand = GameManager.instance.itemManager.GetItemByName(selectedItemName);

        if (plantedItemNearby != null)
        {
            GameManager.instance.tileManager.CollectGrownPlant(position);
        }

        if (GameManager.instance.tileManager.isInteractable(position))
        {
            Debug.Log("Tile is interactable");
            GameManager.instance.tileManager.SetPlowed(position);
        }

        if (itemInHand.data.isPlantable && GameManager.instance.tileManager.IsPlowed(position))
        {
            GameManager.instance.tileManager.PlantSeed(position, itemInHand);
            inventory.toolbar.slots[selectedSlotID].RemoveItem();
        }
    }
}
