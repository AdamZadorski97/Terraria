using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    int worldWidth = 100;
    int worldHeight = 100;

    [SerializeField] private WorldProperties worldParameters;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Tile tile;

    private void Start()
    {
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        int wl = - worldWidth / 2;
        int wr = worldWidth / 2;
        
        int hu = 0;
        int hd = -worldHeight / 2;

        for (int x = wl; x < wr; x++)
        {
            for (int y = hu; hd < y; y--)
            {
                var tilePos = tileMap.WorldToCell(new Vector2(x, y));
                tileMap.SetTile(tilePos, tile);
            }
        }
    }
}



