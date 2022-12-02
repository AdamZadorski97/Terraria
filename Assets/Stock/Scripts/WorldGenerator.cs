using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{

    [SerializeField] private WorldParameters worldParameters;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private List<BlockProporties>  blockProporties;

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
                Block block = Instantiate(blockPrefab);
                block.transform.position = new Vector2(x, y);
                block.blockProporties = GetBlockProporties("Stone");
            }
        }
    }
    private BlockProporties GetBlockProporties(string name)
    {
        foreach(BlockProporties block in blockProporties)
        {
            if (block.name == name) return block;
        }
        Debug.Log($"No block with name {name}");
        return null;
    }
}



