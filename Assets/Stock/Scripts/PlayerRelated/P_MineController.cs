using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class P_MineController : MonoBehaviour
{
    [SerializeField] private PlayerProporties playerProporties;
    [SerializeField] BlockProporties blockProporties;
    private float currentMiningTime;
    private float miningTime = Mathf.Infinity;

    public void Update()
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
            if (GetMiningTilemap() != null)
            {
                Tilemap tilemap = GetMiningTilemap();
                miningTime = GetBlockProporties(tilemap.GetTile(tilemap.WorldToCell(GetMouseHit().point))).timeToDestroy;
                currentMiningTime += Time.deltaTime;
                if (currentMiningTime > miningTime)
                {
                    OnResourceMined(tilemap);
                }
            }
        }
    }

    private RaycastHit2D GetMouseHit()
    {
        Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics2D.Raycast(mRay.origin, Vector2.zero, Mathf.Infinity);
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
        currentMiningTime = 0;
    }

    private void OnResourceMined(Tilemap tilemap)
    {
        var tilePos = tilemap.WorldToCell(GetMouseHit().point);
        tilemap.SetTile(tilePos, null);
        currentMiningTime = 0;
    }

    private Block GetBlockProporties(TileBase tileBase)
    {
        foreach (Block block in blockProporties.blocks)
        {
            if (block.tileBase = tileBase)
            {
                return block;
            }
        }
        return null;
    }
}
