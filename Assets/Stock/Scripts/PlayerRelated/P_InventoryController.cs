using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void AddNewItem(int ID, Sprite sprite)
    {
        int slotNumber = 0;
        foreach (InventorySlot slot in inventorySlots)
        {
            if (ID == slot.itemID)
            {
                slot.itemAmount++;
                slot.itemSprite = sprite;
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
                slot.itemSprite = sprite;
                userInterfaceController.eQBoxControllers[slotNumber].UpdateItemAmount(slot.itemAmount, slot.itemSprite);
                return;
            }
            slotNumber++;
        }
    }
}

[Serializable]
public class InventorySlot
{
    public int itemID;
    public string itemName;
    public int itemAmount;
    public Sprite itemSprite;
}