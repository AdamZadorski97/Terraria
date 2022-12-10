using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    const int mapConst = 8;

    [SerializeField] private WorldParameters worldParameters;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Tile tile;

    private void Start()
    {
        GenerateWorld(MapSize.small);
    }

    private void GenerateWorld(MapSize mapSize)
    {
        GetMapSize(mapSize, out int width, out int height);
        
        int x0 = - mapConst * width / 2;
        int x1 = - x0;

        int y0 = 0;
        int y1 = - mapConst * height / 2 - mapConst / 2;

        for (int x = x0; x < x1; x++)
        {
            for (int y = y0; y1 < y; y--)
            {
                var tilePos = tileMap.WorldToCell(new Vector2(x, y));
                tileMap.SetTile(tilePos, tile);
            }
        }
    }

    private void GetMapSize(MapSize mapSize, out int width, out int height)
    {
        switch (mapSize) {
            case MapSize.small:
                {
                    width = mapConst * 6;
                    height = mapConst * 2;
                    break;
                }
            case MapSize.medium:
                {
                    width = mapConst * 10;
                    height = mapConst * 3;
                    break;
                }
            case MapSize.large:
                {
                    width = mapConst * 14;
                    height = mapConst * 4;
                    break;
                }
            default:
                {
                    throw new Exception("Error");
                }
        }
    }

    enum MapSize
    {
        small = 0,
        medium = 1,
        large = 2
    }
}
