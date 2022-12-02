using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{

    [SerializeField] private WorldParameters worldParameters;
    [SerializeField] private Block blockPrefab;


    private void Start()
    {
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        for (int i = 0; i <= worldParameters.worldSize.x; i++)
        {
            Block block = Instantiate(blockPrefab);
            block.transform.position = new Vector2(i, 0);
        }
    }
}
