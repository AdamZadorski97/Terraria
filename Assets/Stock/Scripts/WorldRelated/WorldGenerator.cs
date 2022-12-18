using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    const int mapConst = 5;

    [SerializeField] private WorldProperties worldParameters;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Tile tile;

    private void Start()
    {
        //GenerateWorld(MapSize.small);
        GenerateWorldV2();
    }

    private void GenerateWorld(MapSize mapSize)
    {
        GetMapSize(mapSize, out int width, out int height);
        
        int x0 = - mapConst * width / 2;
        int x1 = - x0;

        int y0 = 0;
        int y1 = - mapConst * height / 2 - mapConst / 2;

        int yt = 0;
        var random = new System.Random();
        for (int x = x0; x < x1; x++)
        {
            yt += random.Next(-1, 2); ;
            for (int y = yt; y1 < y; y--)
            {
                var tilePos = tileMap.WorldToCell(new Vector2(x, y));
                tileMap.SetTile(tilePos, tile);
            }
        }
    }


    private void GenerateWorldV2()
    {
        int k = 2;

        int tr = 10;
        int size_x = 200;
        int size_y = - 20;

        int max_y = 15;
        int min_y = - 10;
        var listY = new List<int>();

        int x = 0;
        int y = 0;
        var rand = new System.Random();
        for (x = 0; x < size_x + tr; x++)
        {
            y += rand.Next(-1 - k, 2 + k);

            if (max_y < y)
            {
                y = max_y;
            }
            else if (y < min_y)
            {
                y = min_y;
            }

            listY.Add(y);
        }

        int y2;
        var listY2 = new List<int>();
        for (int i0 = tr; i0 < size_x + tr; i0++)
        {
            y2 = 0;
            for (int i1 = 0; i1 < tr; i1++)
                y2 += listY[i0 - i1];

            listY2.Add(y2 / tr);
        }

        for (x = 0; x < size_x; x++)
        {
            for (y = listY2[x]; y > size_y; y--)
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
