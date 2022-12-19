using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BlockProperties", menuName = "ScriptableObjects/BlockProperties", order = 1)]
public class BlockProperties : ScriptableObject
{
    public List<Block> blocks;
}

[Serializable]
public class Block
{
    public int tileId;
    public string tileName;
    public ItemType itemType;
    public Tile tile;
    public float timeToDestroy;
    public List<LootFromBlock> lootFromBlocks;
}

[Serializable]
public class LootFromBlock
{
    public int id;
    public int value;
}
