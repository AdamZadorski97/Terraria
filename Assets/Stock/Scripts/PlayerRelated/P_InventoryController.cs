using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class P_InventoryController : MonoBehaviour
{
    public static P_InventoryController Instance { get; private set; }
    public List<InventorySlot> inventorySlots;
    private UserInterfaceController userInterfaceController;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        userInterfaceController = UserInterfaceController.Instance;
        int number = 0;
        foreach (InventorySlot slot in inventorySlots)
        {
            slot.slotNumber = number;
            number++;
        }
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        int slotNumber = 0;
        foreach (InventorySlot slot in inventorySlots)
        {
            userInterfaceController.eQBoxControllers[slotNumber].UpdateItemAmount(slot.itemAmount, slot.itemSprite);
            slotNumber++;
        }
    }

    public void AddNewItem(int ID, ItemType itemType, Tile tile = null, Sprite sprite = null)
    {
        int slotNumber = 0;
        foreach (InventorySlot slot in inventorySlots)
        {
            if (ID == slot.itemID)
            {
                slot.itemAmount++;
                if(tile!=null)
                slot.itemSprite = tile.sprite;
                if(sprite!=null)
                    slot.itemSprite = sprite;

                slot.itemType = itemType;
                userInterfaceController.eQBoxControllers[slotNumber].UpdateItemAmount(slot.itemAmount, slot.itemSprite);
                return;
            }
            slotNumber++;
        }
        slotNumber = 0;
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.itemAmount == 0)
            {
                slot.itemAmount++;
                slot.itemID = ID;
                slot.itemSprite = tile.sprite;
                userInterfaceController.eQBoxControllers[slotNumber].UpdateItemAmount(slot.itemAmount, slot.itemSprite);
                return;
            }
            slotNumber++;
        }
    }






    public void GetItem(int slot)
    {
        inventorySlots[slot].itemAmount--;
        if(inventorySlots[slot].itemAmount<=0)
        {
            inventorySlots[slot].itemSprite = null;
        }

        userInterfaceController.eQBoxControllers[slot].UpdateItemAmount(inventorySlots[slot].itemAmount, inventorySlots[slot].itemSprite);
    }



}

[Serializable]
public class InventorySlot
{
    public int slotNumber;
    public int itemID;
    public string itemName;
    public ItemType itemType;
    public int itemAmount;
    public Sprite itemSprite;

}

