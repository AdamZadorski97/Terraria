using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_UseItem : MonoBehaviour
{
    private P_InventoryController p_InventoryController;
    private void Start()
    {
        p_InventoryController = GetComponent<P_InventoryController>();
    }


    private void Update()
    {

        if (InputController.Instance.Actions.mineAction.WasPressed)
        {
            UseWeapon();
        }
    } 

    public void UseWeapon()
    {
        if (p_InventoryController.inventorySlots[UserInterfaceController.Instance.GetCurrentSlotNumber()].itemType != ItemType.weapon)
            return;
    }

}
