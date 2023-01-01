using FunkyCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
public class P_MineController : MonoBehaviour
{
    public LightTilemapCollider2D lightTilemapCollider2D;
    public ParticleSystem miningParticles;
    public GameObject minedSprite;
    private ItemList itemList;
    private P_Sounds p_Sounds;
    private P_InventoryController p_InventoryController;
    private float currentMiningTime;
    public float miningTime = Mathf.Infinity;
    private Vector3Int currentMinePosition;

    [SerializeField] private LayerMask objectMask;
    private void Start()
    {
        itemList = ScriptableManager.Instance.itemList;
        p_Sounds = GetComponent<P_Sounds>();
        p_InventoryController = GetComponent<P_InventoryController>();
    }

    private void Update()
    {
        Mine();
    }

    private void Mine()
    {
        if (InputController.Instance.Actions.mineAction.WasPressed)
        {
            OnStartMining();
        }

        if (InputController.Instance.Actions.mineAction.IsPressed)
        {

            RaycastHit2D hit = GetMouseHit();


            if (GetMouseHit().collider != null)
            {
                if (hit.collider.GetComponent<ItemController>())
                {
                    ItemProperties item = hit.collider.GetComponent<ItemController>().itemproperties;
                    miningTime = item.miningTime;
                    currentMiningTime += Time.deltaTime;
                    if (currentMiningTime > miningTime)
                    {
                        p_InventoryController.AddNewItem(item);
                        Destroy(hit.collider.gameObject);
                        OnStopMining();
                    }
                }
            }



            if (GetMiningTilemap() != null)
            {
                Tilemap tilemap = GetMiningTilemap();

                if (currentMinePosition != tilemap.WorldToCell(GetMouseHit().point))
                {
                    currentMinePosition = tilemap.WorldToCell(GetMouseHit().point);
                    miningParticles.Play();
                    currentMiningTime = 0;
                }
                Tile tile = (Tile)tilemap.GetTile(tilemap.WorldToCell(GetMouseHit().point));
                miningTime = GetBlockProporties(tile).miningTime;
                Debug.Log(GetBlockProporties(tile).miningTime);
                miningParticles.transform.position = tilemap.WorldToCell(GetMouseHit().point) + new Vector3(0.5f, 1.1f, 0);

                currentMiningTime += Time.deltaTime;
                if (currentMiningTime > miningTime)
                {

                    OnResourceMined(tilemap);
                }
            }
            else
            {
                miningParticles.Stop();
            }
        }
        if (InputController.Instance.Actions.mineAction.WasReleased)
        {
            OnStopMining();
        }
    }

    private RaycastHit2D GetMouseHit()
    {
        Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics2D.Raycast(mRay.origin, Vector2.zero, Mathf.Infinity, objectMask);
    }

    private Tilemap GetMiningTilemap()
    {
        RaycastHit2D hit = GetMouseHit();
        if (GetMouseHit().collider != null)
        {
            if (hit.collider.GetComponent<TilemapCollider2D>())
            {
                return hit.collider.GetComponent<Tilemap>();
            }
        }
        return null;
    }

    private void OnStartMining()
    {
        miningParticles.Play();
        currentMiningTime = 0;
    }
    private void OnStopMining()
    {
        miningParticles.Stop();
        currentMiningTime = 0;
    }

    private void OnResourceMined(Tilemap tilemap)
    {
        p_Sounds.PlaySound("ResourceMined");
        miningParticles.Stop();
        var tilePos = tilemap.WorldToCell(GetMouseHit().point);
        SetupMinedResource(tilemap);
        tilemap.SetTile(tilePos, null);
        lightTilemapCollider2D.Initialize();
        Light2D.ForceUpdateAll();
        LightingManager2D.ForceUpdate();
        currentMiningTime = 0;

    }

    //private void CheckIsOnUp(Vector3 checkPosition)
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.up, 1, objectMask);
    //    if (hit.collider != null)
    //    {
    //        if (hit.transform.GetComponent<ItemController>())
    //        {
    //            ItemProperties item = hit.collider.GetComponent<ItemController>().itemproperties;
    //            p_InventoryController.AddNewItem(item.itemID, ItemType.interactiveItem, null, item.sprite);
    //        }
    //        if(hit.collider.GetComponent<ItemController>())
    //        {
    //            Destroy(hit.transform.gameObject);
    //        }
          
    //    }
    //}

    private void SetupMinedResource(Tilemap tilemap)
    {
        Tile tile = (Tile)tilemap.GetTile(tilemap.WorldToCell(GetMouseHit().point));
        ItemProperties minedResource = GetBlockProporties(tile);


        foreach (LootOnMined lootFromBlock in minedResource.loot)
        {
            for (int i = 0; i < lootFromBlock.amount; i++)
            {
                p_InventoryController.AddNewItem(lootFromBlock.item);
            }
        }



       // CheckIsOnUp(tilemap.WorldToCell(GetMouseHit().point) + new Vector3(0.5f, 0, 0));
        minedSprite.gameObject.SetActive(true);
        minedSprite.GetComponent<SpriteRenderer>().sprite = tilemap.GetSprite(tilemap.WorldToCell(GetMouseHit().point));
        minedSprite.transform.localScale = Vector3.one * 0.8f;
        minedSprite.transform.position = tilemap.WorldToCell(GetMouseHit().point);
        minedSprite.transform.DOJump(transform.position, 1, 1, 0.5f);
        minedSprite.transform.DOScale(Vector3.one * 0.2f, 0.5f).OnComplete(() => OnPickup());
    }
    private void OnPickup()
    {
        minedSprite.gameObject.SetActive(false);
        p_Sounds.PlaySound("ResourcePickup");
    }

    private ItemProperties GetBlockProporties(Tile tile)
    {
        foreach (ItemProperties block in itemList.item)
        {
            if (block.tile == tile)
            {
                return block;
            }
        }
        return null;
    }
}
