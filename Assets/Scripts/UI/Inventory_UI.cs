using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{

    public string inventoryName;

    public List<Slot_UI> slots = new List<Slot_UI>();
    [SerializeField] private Canvas canvas;

    private Inventory inventory;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        
    }

    private void Start()
    {
        inventory = GameManager.instance.player.inventory.GetInventoryByName(inventoryName);
        SetupSlots();
        Refresh();
    }
    void Update()
    {
        
    }

    public void Refresh()
    {
        if (slots.Count == inventory.slots.Count)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (inventory.slots[i].itemName != "")
                {
                    slots[i].SetItem(inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    public void Remove()
    {
        //Item itemToDrop = GameManager.instance.itemManager.GetItemByName(inventory.slots[UI_Manager.draggedSlot.slotID].itemName);
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID].itemName);
        //UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID]
        Debug.Log("item to Drop: " + itemToDrop);


        if (itemToDrop != null)
        {
            if (UI_Manager.dragSingle)
            {
                GameManager.instance.player.DropItem(itemToDrop);
                UI_Manager.draggedSlot.inventory.Remove(UI_Manager.draggedSlot.slotID);
            }
            else
            {
                Debug.Log("Item is not null");
                GameManager.instance.player.DropItem(itemToDrop, UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID].count);
                UI_Manager.draggedSlot.inventory.Remove(UI_Manager.draggedSlot.slotID, UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID].count);
            }
            GameManager.instance.uiManager.RefreshAll();
        }
        UI_Manager.draggedSlot = null;
    }

    public void SlotBeginDrag(Slot_UI slot)
    {
        UI_Manager.draggedSlot = slot;
        UI_Manager.draggedIcon = Instantiate(UI_Manager.draggedSlot.itemIcon);
        UI_Manager.draggedIcon.transform.SetParent(canvas.transform); // makes icon the child of the canvas
        UI_Manager.draggedIcon.raycastTarget = false; // making sure icon does not block slot, disabling icon as a raycast target
        UI_Manager.draggedIcon.rectTransform.sizeDelta = new Vector2(75, 75); // size of dragged icon

        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
        Debug.Log("Stard Drag: " + UI_Manager.draggedSlot.name);
    }

    public void SlotDrag()
    {
        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
        Debug.Log("Dargging: " + UI_Manager.draggedSlot.name);
    }

    public void SlotEndDrag()
    {
        Destroy(UI_Manager.draggedIcon.gameObject);
        UI_Manager.draggedIcon = null;

        Debug.Log("Done Dragging: " + UI_Manager.draggedSlot);
    }

    public void SlotDrop(Slot_UI slot)
    {
        if (UI_Manager.dragSingle)
        {
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory);
        }
        else
        {
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory, UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID].count);
        }
        GameManager.instance.uiManager.RefreshAll();

    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if (canvas != null)
        {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    void SetupSlots()
    {
        int counter = 0;
        foreach (Slot_UI slot in slots)
        {
            slot.slotID = counter;
            counter++;
            slot.inventory = inventory;
        }
    }
}
