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

    public ItemProperties tempItemProperties;
    public int tempAmountOnPickup;
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
        if (inventorySlots[UserInterfaceController.Instance.GetCurrentSlotNumber()].itemProperties.itemType != ItemType.none)
        if (inventorySlots[UserInterfaceController.Instance.GetCurrentSlotNumber()].itemProperties.itemType == ItemType.weapon)
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
        AddNewItem(tempItemProperties, tempAmountOnPickup);
        for (int i = 0; i < 9; i++)
        {
            if (craftingSlots[i].itemAmount > 0)
            {

                AddNewItem(craftingSlots[i].itemProperties, craftingSlots[i].itemAmount);
                craftingSlots[i].itemAmount = 0;

                if (craftingSlots[i].itemAmount == 0)
                {
                    craftingSlots[i].itemProperties = ScriptableManager.Instance.itemList.item[0];
                }
            }
            ClearRecipieSlot();
            userInterfaceController.eQBoxCraftingControllers[i].UpdateItemAmount(craftingSlots[i].itemAmount, craftingSlots[i].itemProperties);
        }

        userInterfaceController.EQBoxReadyRecipieController.UpdateItemAmount(readyRecipieSlot.itemAmount, readyRecipieSlot.itemProperties);


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
        tempItemProperties = ScriptableManager.Instance.itemList.item[0];
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
            userInterfaceController.eQBoxControllers[slotNumber].UpdateItemAmount(slot.itemAmount, slot.itemProperties);
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
            AddNewItem(tempItemProperties, tempAmountOnPickup);
            ClearTempPick();
        }

        tempItemProperties = inventorySlots[slotNumber].itemProperties;

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

      

        tempItemProperties = craftingSlots[slotNumber].itemProperties;

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
        userInterfaceController.eQBoxCraftingControllers[slotNumber].UpdateItemAmount(craftingSlots[slotNumber].itemAmount, craftingSlots[slotNumber].itemProperties);
        lastSlotGet = slotNumber;
        UpdateTempPickCanvas();
    }

    public void SetSlotDataOnClick(int slotNumber)
    {
        if(tempItemProperties.itemType!=ItemType.none)

        if (craftingSlots[slotNumber].itemProperties.itemType == ItemType.none || craftingSlots[slotNumber].itemProperties == tempItemProperties)
        {
            craftingSlots[slotNumber].itemProperties = tempItemProperties;
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
                userInterfaceController.eQBoxCraftingControllers[slotNumber].UpdateItemAmount(craftingSlots[slotNumber].itemAmount, craftingSlots[slotNumber].itemProperties);
            }

            else
            {
                craftingSlots[slotNumber].itemAmount++;
                tempAmountOnPickup--;

            }
            craftingSlots[slotNumber].itemProperties = tempItemProperties;
        }

        if (tempAmountOnPickup == 0)
        {
            ClearTempPick();
        }

        userInterfaceController.eQBoxCraftingControllers[slotNumber].UpdateItemAmount(craftingSlots[slotNumber].itemAmount, craftingSlots[slotNumber].itemProperties);

        UpdateTempPickCanvas();
        CheckRecipie();
    }

    public void GetReadyRecipie()
    {
        if (readyRecipieSlot.itemAmount == 0)
        {
            return;
        }


        AddNewItem(tempItemProperties);
        ClearTempPick();


        AddNewItem(readyRecipieSlot.itemProperties, readyRecipieSlot.itemAmount);
      

        for (int i = 0; i < 9; i++)
        {
            if (craftingSlots[i].itemAmount > 0)
            {

                craftingSlots[i].itemAmount--;
                if (craftingSlots[i].itemAmount <= 0)
                {
                    craftingSlots[i].itemProperties = ScriptableManager.Instance.itemList.item[0];
                }

                userInterfaceController.eQBoxCraftingControllers[i].UpdateItemAmount(craftingSlots[i].itemAmount, craftingSlots[i].itemProperties);
            }
        }

        userInterfaceController.EQBoxReadyRecipieController.UpdateItemAmount(0, ScriptableManager.Instance.itemList.item[0]);
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
                if (craftingRecipie.requiedItem[i] == craftingSlots[i].itemProperties)
                {
                    Debug.Log(correctCount);
                    correctCount++;
                }
            }
            if (correctCount == 9)
            {
                readyRecipieSlot.itemAmount = craftingRecipie.craftedItemAmount;
                readyRecipieSlot.itemProperties = craftingRecipie.itemProperties;
                userInterfaceController.EQBoxReadyRecipieController.UpdateItemAmount(readyRecipieSlot.itemAmount, readyRecipieSlot.itemProperties);
                return;
            }
            else
            {
                userInterfaceController.EQBoxReadyRecipieController.UpdateItemAmount(0, ScriptableManager.Instance.itemList.item[0]);
            }

        }
    }



    public void AddNewItem(ItemProperties itemProperties, int amount = 1)
    {
        if (itemProperties.itemType == ItemType.none)
            return;


        int slotNumber = 0;
        foreach (InventorySlot slot in inventorySlots)
        {
            if (itemProperties == slot.itemProperties)
            {

                slot.itemAmount += amount;

                userInterfaceController.eQBoxControllers[slotNumber].UpdateItemAmount(slot.itemAmount, slot.itemProperties);
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
                slot.itemProperties = itemProperties;
                userInterfaceController.eQBoxControllers[slotNumber].UpdateItemAmount(slot.itemAmount, slot.itemProperties);
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
            inventorySlot.itemProperties = ScriptableManager.Instance.itemList.item[0];
        }

        if (isCraftingSloot)
            userInterfaceController.eQBoxCraftingControllers[slot].UpdateItemAmount(inventorySlot.itemAmount, inventorySlot.itemProperties);
        else
            userInterfaceController.eQBoxControllers[slot].UpdateItemAmount(inventorySlot.itemAmount, inventorySlot.itemProperties);
    }

    private void ClearRecipieSlot()
    {
        StartCoroutine(ClearRecipieSlotCoroutine());
    }
    IEnumerator ClearRecipieSlotCoroutine()
    {
        yield return new WaitForEndOfFrame();
        readyRecipieSlot.itemProperties = ScriptableManager.Instance.itemList.item[0];
    }
    private void ClearTempPick()
    {
        tempItemProperties = ScriptableManager.Instance.itemList.item[0];
        FloatingCanvasController.Instance.HideTempPickVizualisation();
    }
    private void UpdateTempPickCanvas()
    {
        if(tempItemProperties.itemType != ItemType.none)
        FloatingCanvasController.Instance.ShowTempPickVizualisation(tempItemProperties, tempAmountOnPickup);
    }

    private GameObject tempHoldingObjectPrefab;
    public void SetupPlayerInventory()
    {
        if (tempHoldingObjectPrefab!=null)
        {
            Destroy(tempHoldingObjectPrefab.gameObject);
        }
        ItemType currentItemType = inventorySlots[userInterfaceController.GetCurrentSlotNumber()].itemProperties.itemType;

        if (currentItemType == ItemType.none)
        {
            playerInventoryVisualization.enabled = false;
        }
         

        if (currentItemType == ItemType.ore)
        {
            if (inventorySlots[userInterfaceController.GetCurrentSlotNumber()].itemProperties.itemType == ItemType.none)
            {
                playerInventoryVisualization.enabled = false;
            }
            else
            {
                playerInventoryVisualization.enabled = true;
                playerInventoryVisualization.sprite = inventorySlots[userInterfaceController.GetCurrentSlotNumber()].itemProperties.sprite;
            }
        }
      
        else if (currentItemType == ItemType.weapon)
        {
            playerInventoryVisualization.enabled = false;
            tempHoldingObjectPrefab = Instantiate(inventorySlots[userInterfaceController.GetCurrentSlotNumber()].itemProperties.itemPrefab);
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
    public int itemAmount;
    public ItemProperties itemProperties;
}

