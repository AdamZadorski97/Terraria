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
    public BlocksLoot blocksLoot;
    public int value;
}

[Serializable]
public class BlocksLoot
{
    public int id;
    public string lootName;
    public Sprite lootSprite;
}
