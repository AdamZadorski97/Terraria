using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class P_InventoryController : MonoBehaviour
{
    public static P_InventoryController Instance { get; private set; }
    public List<InventorySlot> inventorySlots;
    public List<InventorySlot> craftingSlots;

    public InventorySlot readyRecipieSlot;


    private CraftingProperties craftingProperties;

    private UserInterfaceController userInterfaceController;



    [SerializeField] private GameObject CraftingPanel;

    public int tempIDOnPickup;
    public int tempAmountOnPickup;
    public ItemType tempItemType;
    public Sprite tempSpriteOnPickup;
    private int lastSlotGet;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (InputController.Instance.Actions.openCrafting.WasPressed)
        {
            if (!CraftingPanel.activeSelf)
            {
                TurnOnCraftingPanel();
            }
            else
            {
                TurnOffCraftingPanel();
            }
        }
    }
    private void TurnOnCraftingPanel()
    {
        CraftingPanel.SetActive(true);
    }

    private void TurnOffCraftingPanel()
    {


        AddNewItem(tempIDOnPickup, tempItemType, null, tempSpriteOnPickup, tempAmountOnPickup);
        for (int i = 0; i < 9; i++)
        {
            if (craftingSlots[i].itemAmount > 0)
            {

                AddNewItem(craftingSlots[i].itemID, craftingSlots[i].itemType, null, craftingSlots[i].itemSprite, craftingSlots[i].itemAmount);
                craftingSlots[i].itemAmount = 0;

                if (craftingSlots[i].itemAmount == 0)
                {
                    craftingSlots[i].itemID = 0;
                }
            }

            userInterfaceController.eQBoxCraftingControllers[i].UpdateItemAmount(craftingSlots[i].itemAmount, craftingSlots[i].itemSprite);
        }
        readyRecipieSlot.itemAmount = 0;
        readyRecipieSlot.itemID = 0;
        readyRecipieSlot.itemSprite = null;
        userInterfaceController.EQBoxReadyRecipieController.UpdateItemAmount(readyRecipieSlot.itemAmount, readyRecipieSlot.itemSprite);


        tempIDOnPickup = 0;
        tempSpriteOnPickup = null;
        tempAmountOnPickup = 0;
        CraftingPanel.SetActive(false);
    }


    public bool CheckCraftingPanelOpen()
    {
        return CraftingPanel.activeSelf;
    }

    private void Start()
    {
        craftingProperties = ScriptableManager.Instance.craftingProperties;
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

    public void GetSlotDataOnClick(int slotNumber)
    {
        if (!CheckCraftingPanelOpen())
            return;
        if (inventorySlots[slotNumber].itemAmount <= 0)
            return;

        if (slotNumber != lastSlotGet)
        {
            AddNewItem(tempIDOnPickup, tempItemType, null, tempSpriteOnPickup, tempAmountOnPickup);
            tempIDOnPickup = 0;
            tempSpriteOnPickup = null;
            tempAmountOnPickup = 0;
        }

        lastSlotGet = slotNumber;

        tempIDOnPickup = inventorySlots[slotNumber].itemID;
        tempItemType = inventorySlots[slotNumber].itemType;
        tempSpriteOnPickup = inventorySlots[slotNumber].itemSprite;
        if (InputController.Instance.Actions.stackItems.IsPressed)
        {
            tempAmountOnPickup = inventorySlots[slotNumber].itemAmount;
            GetItem(slotNumber, tempAmountOnPickup);
        }
        else
        {
            tempAmountOnPickup++;
            GetItem(slotNumber, 1);
        }

        
        
    }

    public void SetSlotDataOnClick(int slotNumber)
    {
        if(tempIDOnPickup == 0)
        {
            if (InputController.Instance.Actions.stackItems.IsPressed)
            {
                AddNewItem(craftingSlots[slotNumber].itemID, craftingSlots[slotNumber].itemType, null, craftingSlots[slotNumber].itemSprite, craftingSlots[slotNumber].itemAmount);
                craftingSlots[slotNumber].itemSprite = null;
                craftingSlots[slotNumber].itemAmount = 0;
                craftingSlots[slotNumber].itemID = 0;
                userInterfaceController.eQBoxCraftingControllers[slotNumber].UpdateItemAmount(craftingSlots[slotNumber].itemAmount, craftingSlots[slotNumber].itemSprite);
            }
            else
            {
                AddNewItem(craftingSlots[slotNumber].itemID, craftingSlots[slotNumber].itemType, null, craftingSlots[slotNumber].itemSprite, 1);
            
                craftingSlots[slotNumber].itemAmount--;

                if(craftingSlots[slotNumber].itemAmount == 0)
                {
                    craftingSlots[slotNumber].itemID = 0;
                    craftingSlots[slotNumber].itemSprite = null;
                }
                userInterfaceController.eQBoxCraftingControllers[slotNumber].UpdateItemAmount(craftingSlots[slotNumber].itemAmount, craftingSlots[slotNumber].itemSprite);
            }
            CheckRecipie();
            return;
        }


        if (craftingSlots[slotNumber].itemID == 0 || craftingSlots[slotNumber].itemID == tempIDOnPickup)
        {
            craftingSlots[slotNumber].itemID = tempIDOnPickup;
            if (InputController.Instance.Actions.stackItems.IsPressed)
            {
                craftingSlots[slotNumber].itemAmount += tempAmountOnPickup;
                tempAmountOnPickup = 0;
             
            }
            else
            {
                craftingSlots[slotNumber].itemAmount++;
                tempAmountOnPickup--;
            }
            craftingSlots[slotNumber].itemSprite = tempSpriteOnPickup;
        }

        else if (craftingSlots[slotNumber].itemID != tempIDOnPickup)
        {
            AddNewItem(craftingSlots[slotNumber].itemID, craftingSlots[slotNumber].itemType, null, craftingSlots[slotNumber].itemSprite, craftingSlots[slotNumber].itemAmount);
            craftingSlots[slotNumber].itemSprite = tempSpriteOnPickup;
            craftingSlots[slotNumber].itemAmount = tempAmountOnPickup;
            craftingSlots[slotNumber].itemID = tempIDOnPickup;
        }

        if (tempAmountOnPickup == 0)
        {
            tempIDOnPickup = 0;
        }

        userInterfaceController.eQBoxCraftingControllers[slotNumber].UpdateItemAmount(craftingSlots[slotNumber].itemAmount, craftingSlots[slotNumber].itemSprite);


        CheckRecipie();
    }

    public void GetReadyRecipie()
    {
        if (readyRecipieSlot.itemAmount == 0)
        {
            return;
        }

        AddNewItem(tempIDOnPickup, tempItemType, null, tempSpriteOnPickup, tempAmountOnPickup);
        tempIDOnPickup = 0;
        tempSpriteOnPickup = null;
        tempAmountOnPickup = 0;

        AddNewItem(readyRecipieSlot.itemID, readyRecipieSlot.itemType, null, readyRecipieSlot.itemSprite, readyRecipieSlot.itemAmount);
        readyRecipieSlot.itemID = 0;
        readyRecipieSlot.itemAmount = 0;
        readyRecipieSlot.itemSprite = null;

        for (int i = 0; i < 9; i++)
        {
            if (craftingSlots[i].itemAmount > 0)
            {

                craftingSlots[i].itemAmount--;
                if (craftingSlots[i].itemAmount <= 0)
                {
                    craftingSlots[i].itemID = 0;
                }

                userInterfaceController.eQBoxCraftingControllers[i].UpdateItemAmount(craftingSlots[i].itemAmount, craftingSlots[i].itemSprite);
            }
        }

        userInterfaceController.EQBoxReadyRecipieController.UpdateItemAmount(0, null);
        CheckRecipie();

    }

    public void CheckRecipie()
    {
        foreach (CraftingRecipie craftingRecipie in craftingProperties.recipies)
        {
            int correctCount = 0;
            for (int i = 0; i < 9; i++)
            {
                if (craftingRecipie.itemID[i] == craftingSlots[i].itemID)
                {
                    correctCount++;
                }
            }
            if (correctCount == 9)
            {
                readyRecipieSlot.itemID = craftingRecipie.recipieID;
                readyRecipieSlot.itemType = craftingRecipie.itemType;
                readyRecipieSlot.itemSprite = craftingRecipie.sprite;
                readyRecipieSlot.itemAmount = craftingRecipie.amount;
                userInterfaceController.EQBoxReadyRecipieController.UpdateItemAmount(readyRecipieSlot.itemAmount, readyRecipieSlot.itemSprite);
                return;
            }
            else
            {
                userInterfaceController.EQBoxReadyRecipieController.UpdateItemAmount(0, null);
            }

        }
    }



    public void AddNewItem(int ID, ItemType itemType, Tile tile = null, Sprite sprite = null, int amount = 1)
    {
        int slotNumber = 0;
        foreach (InventorySlot slot in inventorySlots)
        {
            if (ID == slot.itemID)
            {

                slot.itemAmount += amount;
                if (tile != null)
                    slot.itemSprite = tile.sprite;
                if (sprite != null)
                    slot.itemSprite = sprite;

                Debug.Log(itemType);
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
                slot.itemAmount += amount;
                slot.itemID = ID;
                if (tile != null)
                    slot.itemSprite = tile.sprite;
                if (sprite != null)
                    slot.itemSprite = sprite;
                slot.itemType = itemType;
                userInterfaceController.eQBoxControllers[slotNumber].UpdateItemAmount(slot.itemAmount, slot.itemSprite);
                return;
            }
            slotNumber++;
        }
    }

    public void GetItem(int slot, int amount = 1)
    {
        inventorySlots[slot].itemAmount -= amount;
        if (inventorySlots[slot].itemAmount <= 0)
        {
            inventorySlots[slot].itemID = 0;
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

