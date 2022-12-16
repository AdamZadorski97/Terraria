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
    private PlayerProperties playerProporties;
    private BlockProperties blockProporties;
    private P_Sounds p_Sounds;
    private float currentMiningTime;
    private float miningTime = Mathf.Infinity;
    private Vector3Int currentMinePosition;
    private void Start()
    {
        playerProporties = ScriptableManager.Instance.playerProperties;
        blockProporties = ScriptableManager.Instance.blockProperties;
        p_Sounds = GetComponent<P_Sounds>();
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

                if (currentMinePosition != tilemap.WorldToCell(GetMouseHit().point))
                {
                    currentMinePosition = tilemap.WorldToCell(GetMouseHit().point);
                    miningParticles.Play();
                    currentMiningTime = 0;
                }
                miningTime = GetBlockProporties(tilemap.GetTile(currentMinePosition)).timeToDestroy;
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

    private void SetupMinedResource(Tilemap tilemap)
    {
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
