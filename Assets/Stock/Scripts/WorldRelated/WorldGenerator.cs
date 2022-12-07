using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{

    [SerializeField] private WorldParameters worldParameters;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Tile tile;

    private void Start()
    {
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        for (int x = 0; x <= worldParameters.worldSize.x; x++)
        {
            for (int y = 0; y <= worldParameters.worldSize.y; y++)
            {
                var tilePos = tileMap.WorldToCell(new Vector2(x,y));
                tileMap.SetTile(tilePos, tile);
            }
        }
    }
}



