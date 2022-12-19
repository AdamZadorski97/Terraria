using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlocksLootProperties", menuName = "ScriptableObjects/BlocksLootProperties", order = 1)]
public class BlocksLootProperties : ScriptableObject
{
    public List<BlocksLoot> blocksLoots;
}

[Serializable]
public class BlocksLoot
{
    public int id;
    public string lootName;
    public Sprite lootSprite;
}
