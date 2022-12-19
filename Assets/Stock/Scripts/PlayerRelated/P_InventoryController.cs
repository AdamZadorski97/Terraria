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
    public Sprite tempSpriteOnPickup;

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
        CraftingPanel.SetActive(false);
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
        tempIDOnPickup = inventorySlots[slotNumber].itemID;
        tempAmountOnPickup++;
        tempSpriteOnPickup = inventorySlots[slotNumber].itemSprite;
        GetItem(slotNumber);
    }

    public void SetSlotDataOnClick(int slotNumber)
    {
        if (tempAmountOnPickup == 0)
        {
            return;
        }
            craftingSlots[slotNumber].itemID = tempIDOnPickup;
        craftingSlots[slotNumber].itemAmount++;
        craftingSlots[slotNumber].itemSprite = tempSpriteOnPickup;
        userInterfaceController.eQBoxCraftingControllers[slotNumber].UpdateItemAmount(craftingSlots[slotNumber].itemAmount, tempSpriteOnPickup);
      
        tempAmountOnPickup--;
        CheckRecipie();
    }

    public void GetReadyRecipie()
    {
        if (readyRecipieSlot.itemAmount == 0)
        {
            return;
        }

       
      

        AddNewItem(readyRecipieSlot.itemID, readyRecipieSlot.itemType, null, readyRecipieSlot.itemSprite);
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
                readyRecipieSlot.itemAmount = 1;
                userInterfaceController.EQBoxReadyRecipieController.UpdateItemAmount(1, readyRecipieSlot.itemSprite);
                Debug.Log("RecipieFound");
                return;
            }

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
                slot.itemAmount++;
                slot.itemID = ID;
                if (tile != null)
                    slot.itemSprite = tile.sprite;
                if (sprite != null)
                    slot.itemSprite = sprite;
                userInterfaceController.eQBoxControllers[slotNumber].UpdateItemAmount(slot.itemAmount, slot.itemSprite);
                return;
            }
            slotNumber++;
        }
    }

    public void GetItem(int slot)
    {
        inventorySlots[slot].itemAmount--;
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

