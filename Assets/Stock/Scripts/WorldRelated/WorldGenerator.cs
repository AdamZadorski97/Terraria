using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using static FunkyCode.DayLightCollider2D;

public class WorldGenerator : MonoBehaviour
{
    private WorldProperties worldParameters;
   [SerializeField] private ItemList blockList;

    struct Point
    {
        public int x, y;
    }

    [SerializeField] private Tilemap tileMap;

    private void Start()
    {
        worldParameters = ScriptableManager.Instance.worldProperties;
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        int mapX0 = worldParameters.mapX0;
        int mapX1 = worldParameters.mapX1;
        int width = mapX1 - mapX0;

        int biome = worldParameters.mapBiome;
        int treshold = worldParameters.mapAxtreshold;

        Tile tile = blockList.item[0].tile;
        List<Point> lineDown;
        int y0;

        // ------------------------- top

        var lineUp = GenerateLineX(tile, mapX0, width, mapX0);

        y0 = -1 * biome;
        lineDown = GenerateCurveX(tile, mapX0, width, y0, treshold);
        GenerateBetween(tile, lineUp, lineDown);

        // ------------------------- mid

        for (int i = 0; i < 1; i++)
        {
           

            tile = blockList.item[0].tile;
            lineUp = lineDown;

            y0 = -(i + 1) * biome;
            lineDown = GenerateCurveX(tile, mapX0, width, y0, treshold);
            GenerateBetween(tile, lineUp, lineDown);
        }

        // ------------------------- bottom

        lineUp = lineDown;
        y0 -= treshold + 1;
        lineDown = GenerateLineX(tile, mapX0, width, y0);
        GenerateBetween(tile, lineUp, lineDown);
    }

    private void GenerateBetween(Tile tile, List<Point> lineUp, List<Point> lineDown)
    {
        if (lineUp == null || lineDown == null)
            throw new Exception("");

        if (lineUp.Count != lineDown.Count)
            throw new Exception("");

        int x, y;
        for (int i = 0; i < lineUp.Count; i++)
        {
            if (lineUp[i].x != lineDown[i].x)
                throw new Exception("");

            float done;
            float upY = lineUp[i].y;
            for (int yt = lineDown[i].y; yt < lineUp[i].y; yt++)
            {
                x = lineDown[i].x;
                y = yt;

                done = Mathf.Abs(upY / yt);
                //Debug.Log(done);

                var tilePos = tileMap.WorldToCell(new Vector2(x, y));
                tileMap.SetTile(tilePos, tile);
            }
        }
    }

    private List<Point> GenerateCurveX(Tile tile, int x0, int x1, int y0, int treshold)
    {
        var points = new List<Point>();
        var rand = new System.Random();

        int r;
        int y2 = y0;

        for (int x = x0; x < x1; x++)
        {
            r = rand.Next(-1, 2);
            y2 += r;

            if (y2 > y0 + treshold)
            {
                y2 = y0 + treshold;
            }
            else if (y2 < y0 - treshold) {
                y2 = y0 - treshold;
            }

            var tilePos = tileMap.WorldToCell(new Vector2(x, y2));
            tileMap.SetTile(tilePos, tile);

            points.Add(new Point() { x = x, y = y2 });
        }

        return points;
    }

    private List<Point> GenerateLineX(Tile tile, int x0, int x1, int y0)
    {
        var points = new List<Point>();

        for (int x = x0; x < x1; x++)
        {
            var tilePos = tileMap.WorldToCell(new Vector2(x, y0));
            tileMap.SetTile(tilePos, tile);

            points.Add(new Point() { x = x, y = y0 });
        }

        return points;
    }
}
