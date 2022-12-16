using FunkyCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class P_MineController : MonoBehaviour
{
    public LightTilemapCollider2D lightTilemapCollider2D;
    public ParticleSystem miningParticles;
    private PlayerProperties playerProporties;
    private BlockProperties blockProporties;
    private float currentMiningTime;
    private float miningTime = Mathf.Infinity;

    private void Start()
    {
        playerProporties = ScriptableManager.Instance.playerProperties;
        blockProporties = ScriptableManager.Instance.blockProperties;
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
            if (GetMiningTilemap() != null)
            {
                Tilemap tilemap = GetMiningTilemap();
                miningTime = GetBlockProporties(tilemap.GetTile(tilemap.WorldToCell(GetMouseHit().point))).timeToDestroy;
                miningParticles.transform.position = tilemap.WorldToCell(GetMouseHit().point) + new Vector3(0.5f,1.1f,0);
                if(miningParticles.isPlaying==false)
                miningParticles.Play();
                currentMiningTime += Time.deltaTime;
                if (currentMiningTime > miningTime)
                {
                   
                    OnResourceMined(tilemap);
                }
            }
        }
        if(InputController.Instance.Actions.mineAction.WasReleased)
        {
            OnStopMining();
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
                Debug.Log(hit.collider);
          
                return hit.collider.GetComponent<Tilemap>();
            }
        }
        return null;
    }

    private void OnStartMining()
    {
      
        miningParticles.gameObject.SetActive(true);
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
        Debug.Log("Stop");
        var tilePos = tilemap.WorldToCell(GetMouseHit().point);
        tilemap.SetTile(tilePos, null);
        lightTilemapCollider2D.Initialize();
        Light2D.ForceUpdateAll();
        LightingManager2D.ForceUpdate();
        currentMiningTime = 0;
    
    }

    IEnumerator UpdateLight()
    {
        yield return new WaitForSeconds(0.01f);
      
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
