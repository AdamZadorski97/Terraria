using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public BlockProporties blockProporties;


    public void Start()
    {
        spriteRenderer.sprite = blockProporties.texture;
    }
}
