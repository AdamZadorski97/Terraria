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

    [SerializeField] private Transform playerInventoryVisualizationHoldPivot;
    [SerializeField] private Transform playerInventoryVisualizationPivot;
    [SerializeField] private SpriteRenderer playerInventoryVisualization;
    [SerializeField] private GameObject CraftingPanel;

    public int tempIDOnPickup;
    public int tempAmountOnPickup;
    public ItemType tempItemType;
    public Sprite tempSpriteOnPickup;
    public GameObject tempItemPrefab;
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
       



        if (InputController.Instance.Actions.openCraftingAction.WasPressed)
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

    private void LateUpdate()
    {
        if (inventorySlots[UserInterfaceController.Instance.GetCurrentSlotNumber()].itemType == ItemType.weapon)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            playerInventoryVisualizationPivot.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - playerInventoryVisualizationPivot.position);
        }
        else
        {
            playerInventoryVisualizationPivot.localEulerAngles = new Vector3(0, 0, -90);
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
            ClearRecipieSlot();
            userInterfaceController.eQBoxCraftingControllers[i].UpdateItemAmount(craftingSlots[i].itemAmount, craftingSlots[i].itemSprite);
        }

        userInterfaceController.EQBoxReadyRecipieController.UpdateItemAmount(readyRecipieSlot.itemAmount, readyRecipieSlot.itemSprite);


        ClearTempPick();
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
            ClearTempPick();
        }


        tempIDOnPickup = inventorySlots[slotNumber].itemID;
        tempItemType = inventorySlots[slotNumber].itemType;
        tempSpriteOnPickup = inventorySlots[slotNumber].itemSprite;
        tempItemPrefab = inventorySlots[slotNumber].itemPrefab;
        if (InputController.Instance.Actions.stackItemsAction.IsPressed)
        {
            tempAmountOnPickup += inventorySlots[slotNumber].itemAmount;
            GetItem(inventorySlots[slotNumber], slotNumber, inventorySlots[slotNumber].itemAmount);
        }
        else if (InputController.Instance.Actions.stackHalfItemsAction.IsPressed)
        {
            int half = (int)inventorySlots[slotNumber].itemAmount / 2;
            tempAmountOnPickup += half;
            GetItem(inventorySlots[slotNumber], slotNumber, half);
        }
        else
        {
            tempAmountOnPickup++;
            GetItem(inventorySlots[slotNumber], slotNumber, 1);
        }

        lastSlotGet = slotNumber;
        UpdateTempPickCanvas();


    }


    public void GetFromCraftingSLot(int slotNumber)
    {
        if (craftingSlots[slotNumber].itemAmount <= 0)
            return;

        if (tempIDOnPickup != craftingSlots[slotNumber].itemID && tempIDOnPickup != 0)
            return;

        tempIDOnPickup = craftingSlots[slotNumber].itemID;
        tempItemType = craftingSlots[slotNumber].itemType;
        tempSpriteOnPickup = craftingSlots[slotNumber].itemSprite;
        tempItemPrefab = craftingSlots[slotNumber].itemPrefab;
        if (InputController.Instance.Actions.stackItemsAction.IsPressed)
        {
            tempAmountOnPickup += craftingSlots[slotNumber].itemAmount;
            GetItem(craftingSlots[slotNumber], slotNumber, craftingSlots[slotNumber].itemAmount, true);

        }
        else if (InputController.Instance.Actions.stackHalfItemsAction.IsPressed)
        {
            int half = (int)inventorySlots[slotNumber].itemAmount / 2;
            tempAmountOnPickup += half;
            GetItem(craftingSlots[slotNumber], slotNumber, half, true);
        }
        else
        {
            tempAmountOnPickup++;
            GetItem(craftingSlots[slotNumber], slotNumber, 1, true);
        }
        userInterfaceController.eQBoxCraftingControllers[slotNumber].UpdateItemAmount(craftingSlots[slotNumber].itemAmount, craftingSlots[slotNumber].itemSprite);
        lastSlotGet = slotNumber;
        UpdateTempPickCanvas();
    }

    public void SetSlotDataOnClick(int slotNumber)
    {
        if (craftingSlots[slotNumber].itemID == 0 || craftingSlots[slotNumber].itemID == tempIDOnPickup)
        {
            craftingSlots[slotNumber].itemID = tempIDOnPickup;
            if (InputController.Instance.Actions.stackItemsAction.IsPressed)
            {
                craftingSlots[slotNumber].itemAmount += tempAmountOnPickup;
                tempAmountOnPickup = 0;

            }
            else if (InputController.Instance.Actions.stackHalfItemsAction.IsPressed)
            {
                int half = (int)tempAmountOnPickup / 2;
                tempAmountOnPickup -= half;
                craftingSlots[slotNumber].itemAmount += half;
                userInterfaceController.eQBoxCraftingControllers[slotNumber].UpdateItemAmount(craftingSlots[slotNumber].itemAmount, craftingSlots[slotNumber].itemSprite);
            }

            else
            {
                craftingSlots[slotNumber].itemAmount++;
                tempAmountOnPickup--;

            }
            craftingSlots[slotNumber].itemSprite = tempSpriteOnPickup;
            craftingSlots[slotNumber].itemPrefab = tempItemPrefab;
        }

        if (tempAmountOnPickup == 0)
        {
            ClearTempPick();
        }

        userInterfaceController.eQBoxCraftingControllers[slotNumber].UpdateItemAmount(craftingSlots[slotNumber].itemAmount, craftingSlots[slotNumber].itemSprite);

        UpdateTempPickCanvas();
        CheckRecipie();
    }

    public void GetReadyRecipie()
    {
        if (readyRecipieSlot.itemAmount == 0)
        {
            return;
        }

        AddNewItem(tempIDOnPickup, tempItemType, null, tempSpriteOnPickup, tempAmountOnPickup, tempItemPrefab);
        ClearTempPick();

        AddNewItem(readyRecipieSlot.itemID, readyRecipieSlot.itemType, null, readyRecipieSlot.itemSprite, readyRecipieSlot.itemAmount, readyRecipieSlot.itemPrefab);
      

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
        UpdateTempPickCanvas();
        ClearRecipieSlot();
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
                readyRecipieSlot.itemPrefab = craftingRecipie.itemPrefab;
                userInterfaceController.EQBoxReadyRecipieController.UpdateItemAmount(readyRecipieSlot.itemAmount, readyRecipieSlot.itemSprite);
                return;
            }
            else
            {
                userInterfaceController.EQBoxReadyRecipieController.UpdateItemAmount(0, null);
            }

        }
    }



    public void AddNewItem(int ID, ItemType itemType, Tile tile = null, Sprite sprite = null, int amount = 1, GameObject itemPrefab = null)
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
                if (itemPrefab != null)
                    slot.itemPrefab = itemPrefab;

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
                if (itemPrefab != null)
                    slot.itemPrefab = itemPrefab;
                slot.itemType = itemType;
                userInterfaceController.eQBoxControllers[slotNumber].UpdateItemAmount(slot.itemAmount, slot.itemSprite);
                return;
            }
            slotNumber++;
        }
    }

    public void GetItem(InventorySlot inventorySlot, int slot, int amount = 1, bool isCraftingSloot = false)
    {
        inventorySlot.itemAmount -= amount;
        if (inventorySlot.itemAmount <= 0)
        {
            inventorySlot.itemID = 0;
            inventorySlot.itemSprite = null;
        }

        if (isCraftingSloot)
            userInterfaceController.eQBoxCraftingControllers[slot].UpdateItemAmount(inventorySlot.itemAmount, inventorySlot.itemSprite);
        else
            userInterfaceController.eQBoxControllers[slot].UpdateItemAmount(inventorySlot.itemAmount, inventorySlot.itemSprite);
    }

    private void ClearRecipieSlot()
    {
        StartCoroutine(ClearRecipieSlotCoroutine());
    }
    IEnumerator ClearRecipieSlotCoroutine()
    {
        yield return new WaitForEndOfFrame();
        readyRecipieSlot.itemID = 0;
        readyRecipieSlot.itemAmount = 0;

        readyRecipieSlot.itemSprite = null;
        readyRecipieSlot.itemPrefab = null;
    }
    private void ClearTempPick()
    {
        tempIDOnPickup = 0;
        tempAmountOnPickup = 0;
        tempSpriteOnPickup = null;
        tempItemPrefab = null;
        FloatingCanvasController.Instance.HideTempPickVizualisation();
    }
    private void UpdateTempPickCanvas()
    {
        FloatingCanvasController.Instance.ShowTempPickVizualisation(tempSpriteOnPickup, tempAmountOnPickup);
    }

    private GameObject tempHoldingObjectPrefab;
    public void SetupPlayerInventory()
    {
        if (tempHoldingObjectPrefab!=null)
        {
            Destroy(tempHoldingObjectPrefab.gameObject);
        }
         

        if (inventorySlots[userInterfaceController.GetCurrentSlotNumber()].itemType == ItemType.ore)
        {
            if (inventorySlots[userInterfaceController.GetCurrentSlotNumber()].itemSprite = null)
            {
                playerInventoryVisualization.enabled = false;
            }
            else
            {
                playerInventoryVisualization.enabled = true;
                playerInventoryVisualization.sprite = inventorySlots[userInterfaceController.GetCurrentSlotNumber()].itemSprite;
            }
        }
        else if (inventorySlots[userInterfaceController.GetCurrentSlotNumber()].itemType == ItemType.weapon)
        {
            playerInventoryVisualization.enabled = false;
            tempHoldingObjectPrefab = Instantiate(inventorySlots[userInterfaceController.GetCurrentSlotNumber()].itemPrefab);
            {
                tempHoldingObjectPrefab.transform.SetParent(playerInventoryVisualizationHoldPivot);
                tempHoldingObjectPrefab.transform.position = playerInventoryVisualizationHoldPivot.position;
            }
        }
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
    public GameObject itemPrefab;

}

