using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using static FunkyCode.DayLightCollider2D;

public class WorldGenerator : MonoBehaviour
{
    private WorldProperties worldParameters;
    struct Point
    {
        public int x, y;
    }

    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Tile tile;

    private void Start()
    {
        worldParameters = ScriptableManager.Instance.worldProperties;

        int mapX0 = worldParameters.mapX0;
        int mapX1 = worldParameters.mapX1;
        int width = mapX1 - mapX0;
        int biome = worldParameters.mapBiome;

        int tresh = 100;
        var lineUp = GenerateLineX(mapX0, width, 0);
        int y0;

        y0 = -1 * biome;
        var lineDown = GenerateCurveX(mapX0, width, y0, tresh);
        //GenerateBetween(lineUp, lineDown);

        y0 = -2 * biome;
        lineUp = lineDown;
        lineDown = GenerateCurveX(mapX0, width, y0, tresh);
        GenerateBetween(lineUp, lineDown);

        y0 = -3 * biome;
        lineUp = lineDown;
        lineDown = GenerateCurveX(mapX0, width, y0, tresh);
        //GenerateBetween(lineUp, lineDown);
    }

    private void GenerateCup() { 
    }

    private void GenerateBetween(List<Point> lineUp, List<Point> lineDown)
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
                Debug.Log(done);

                var tilePos = tileMap.WorldToCell(new Vector2(x, y));
                tileMap.SetTile(tilePos, tile);
            }
        }
    }

    private List<Point> GenerateCurveX(int x0, int x1, int y0, int treshold)
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

    private List<Point> GenerateLineX(int x0, int x1, int y0)
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
