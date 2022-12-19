using FunkyCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class P_BuildController : MonoBehaviour
{

    public LightTilemapCollider2D lightTilemapCollider2D;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask interactiveObjectLayer;

    private P_InventoryController p_InventoryController;
    [SerializeField] private Tilemap tileMap;
    private BlockProperties blockProperties;
    private ItemProperties itemProperties;
    private void Update()
    {
        if (InputController.Instance.Actions.buildAction.WasReleased)
        {
            Build();
        }
    }
    private void Start()
    {
        blockProperties = ScriptableManager.Instance.blockProperties;
        itemProperties = ScriptableManager.Instance.itemProperties;
        p_InventoryController = GetComponent<P_InventoryController>();
    }


    private RaycastHit2D GetMouseHit()
    {
        Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics2D.Raycast(mRay.origin, Vector2.zero, Mathf.Infinity);
    }



    private void Build()
    {

        if (GetMouseHit())
            return;

        if (p_InventoryController.inventorySlots[UserInterfaceController.Instance.GetCurrentSlot()].itemAmount > 0)
        {
            if (p_InventoryController.inventorySlots[UserInterfaceController.Instance.GetCurrentSlot()].itemType == ItemType.block)
            {
                var tilePos = tileMap.WorldToCell(tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0))));
                tileMap.SetTile(tilePos, GetBlockProporties(p_InventoryController.inventorySlots[UserInterfaceController.Instance.GetCurrentSlot()].itemID));
            }

            if (p_InventoryController.inventorySlots[UserInterfaceController.Instance.GetCurrentSlot()].itemType == ItemType.interactiveItem)
            {
                if (!CheckBlockAbove())
                    return;
                Debug.Log("place");
                var itemPos = tileMap.WorldToCell(tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0))));
                var item = GetItemProperties(p_InventoryController.inventorySlots[UserInterfaceController.Instance.GetCurrentSlot()].itemID);
                GameObject placedItem = Instantiate(item.itemPrefab);
                placedItem.transform.position = itemPos + new Vector3(0.5f, 0.5f);
            }


            p_InventoryController.GetItem(UserInterfaceController.Instance.GetCurrentSlot());
            lightTilemapCollider2D.Initialize();
            Light2D.ForceUpdateAll();
            LightingManager2D.ForceUpdate();
        }
    }


    private bool CheckBlockAbove()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitGround = Physics2D.Raycast(pos, Vector2.down, 1, groundLayer);
        if (hitGround.collider != null)
        {
            return true;
        }

     

        RaycastHit2D hitInteractive = Physics2D.Raycast(pos, Vector2.down, 1f, interactiveObjectLayer);
        if (hitInteractive.collider != null)
        {
            if (hitInteractive.collider.GetComponent<LeadderController>())
            {
                return true;
            }
        }
           
             

        return false;
           

    }

    private void SnapObject(Transform objectToBuild)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 5, groundLayer);
        if (hit.collider != null)
        {
            BoxCollider2D boxCollider2D = objectToBuild.GetComponent<BoxCollider2D>();
            objectToBuild.transform.position = hit.point + new Vector2(0, boxCollider2D.size.x / 2);
        }
    }

    private Tile GetBlockProporties(int id)
    {
        foreach (Block block in blockProperties.blocks)
        {
            if (block.tileId == id)
            {
                return block.tile;
            }
        }
        return null;
    }

    private Item GetItemProperties(int id)
    {
        foreach (Item item in itemProperties.item)
        {
            if (item.itemID == id)
            {
                return item;
            }
        }
        return null;
    }
}
