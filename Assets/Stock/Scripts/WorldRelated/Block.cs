using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public BlockProporties blockProporties;

    private float timeToDestroy;
    private SpriteRenderer destroyTextureSprite;
    private ParticleSystem destroyParticles;

    public void Start()
    {
        spriteRenderer.sprite = blockProporties.texture;
        timeToDestroy = blockProporties.timeToDestroy;
    }

    public void OnMine()
    {
        if (destroyTextureSprite == null)
        {
            CreateDestroyParticles();
            CreateDestroyTexture();
        }

        UpdateParticles();
        UpdateDestroyTexture();

        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0)
        {
            OnDestroyBlock();
        }
    }

    private void CreateDestroyTexture()
    {
        GameObject destroyTextureGameobject = new GameObject("Destroyed Texture");
        destroyTextureGameobject.transform.SetParent(transform);
        destroyTextureGameobject.transform.position = transform.position;
        destroyTextureSprite = destroyTextureGameobject.AddComponent<SpriteRenderer>();
        destroyTextureSprite.sortingOrder = 1;
    }

    private void CreateDestroyParticles()
    {
        destroyParticles = Instantiate(blockProporties.destroyParticles) as ParticleSystem;
        destroyParticles.transform.SetParent(transform);
        destroyParticles.transform.position = transform.position + new Vector3(0,0.8f);
        destroyParticles.Play();
    }

    private void UpdateDestroyTexture()
    {
        switch (timeToDestroy)
        {
            case >= 1.8f:
                destroyTextureSprite.sprite = blockProporties.destroySprites[9];
                break;
            case > 1.6f:
                destroyTextureSprite.sprite = blockProporties.destroySprites[8];
                break;
            case > 1.4f:
                destroyTextureSprite.sprite = blockProporties.destroySprites[7];
                break;
            case > 1.2f:
                destroyTextureSprite.sprite = blockProporties.destroySprites[6];
                break;
            case > 1.0f:
                destroyTextureSprite.sprite = blockProporties.destroySprites[5];
                break;
            case > 0.8f:
                destroyTextureSprite.sprite = blockProporties.destroySprites[4];
                break;
            case > 0.6f:
                destroyTextureSprite.sprite = blockProporties.destroySprites[3];
                break;
            case > 0.4f:
                destroyTextureSprite.sprite = blockProporties.destroySprites[2];
                break;
            case > 0.2f:
                destroyTextureSprite.sprite = blockProporties.destroySprites[1];
                break;
            case > 0.0f:
                destroyTextureSprite.sprite = blockProporties.destroySprites[0];
                break;
        }
    }
    private void UpdateParticles()
    {
        if(destroyParticles.isStopped)
        destroyParticles.Play();
    }

    public void OnStopMining()
    {
        Debug.Log("StopMining");
       destroyParticles.Stop();
    }
    private void OnDestroyBlock()
    {
        destroyParticles.gameObject.transform.SetParent(null);
        destroyParticles.Stop();
        Destroy(destroyParticles, 1f);
        Destroy(gameObject);
    }
}
