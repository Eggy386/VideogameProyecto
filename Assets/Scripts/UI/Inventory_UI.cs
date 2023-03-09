using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;

    public Player player;

    public List<Slot_UI> slots = new List<Slot_UI>();
    [SerializeField] private Canvas canvas;

    private Slot_UI draggedSlot; //holds a reference to slot when dragging
    private Image draggedIcon;
    private bool dragSingle;


    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        inventoryPanel.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        } 

        if(Input.GetKey(KeyCode.LeftShift))
        {
            dragSingle = true;
            Debug.Log("drag single: " + dragSingle);
        }
        else
        {
            dragSingle = false;
        }

    }

    public void ToggleInventory()
    {
        if(!inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(true);
            Refresh();
        }
        else
        {
            inventoryPanel.SetActive(false);
        }
    }

    void Refresh()
    {
        if(slots.Count == player.inventory.slots.Count) 
        {
            for(int i = 0; i< slots.Count; i++) 
            {
                if (player.inventory.slots[i].itemName != "")
                {
                    slots[i].SetItem(player.inventory.slots[i]);
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
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(player.inventory.slots[draggedSlot.slotID].itemName);
        Debug.Log(itemToDrop);
        

        if (itemToDrop != null)
        {
            if (dragSingle)
            {
                player.DropItem(itemToDrop);
                player.inventory.Remove(draggedSlot.slotID);
            }
            else
            {
                Debug.Log("Item is not null");
                player.DropItem(itemToDrop, player.inventory.slots[draggedSlot.slotID].count);
                player.inventory.Remove(draggedSlot.slotID, player.inventory.slots[draggedSlot.slotID].count);
            }
            Refresh();
        }
        draggedSlot = null;
    }

    public void SlotBeginDrag(Slot_UI slot)
    {
        draggedSlot = slot;
        draggedIcon = Instantiate(draggedSlot.itemIcon);
        draggedIcon.transform.SetParent(canvas.transform); // makes icon the child of the canvas
        draggedIcon.raycastTarget = false; // making sure icon does not block slot, disabling icon as a raycast target
        draggedIcon.rectTransform.sizeDelta = new Vector2(75, 75); // size of dragged icon

        MoveToMousePosition(draggedIcon.gameObject);
        Debug.Log("Stard Drag: " + draggedSlot.name);
    }

    public void SlotDrag()
    {
        MoveToMousePosition(draggedIcon.gameObject);
        Debug.Log("Dargging: " + draggedSlot.name);
    }

    public void SlotEndDrag()
    {
        Destroy(draggedIcon.gameObject);
        draggedIcon = null;

        Debug.Log("Done Dragging: " + draggedSlot);
    }

    public void SlotDrop(Slot_UI slot)
    {
        
        Debug.Log("Dropped: " + draggedSlot.name + " on " + slot.name);
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if(canvas != null)
        {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }
}
