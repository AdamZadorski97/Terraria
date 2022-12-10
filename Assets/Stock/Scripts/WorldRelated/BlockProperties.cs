using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BlockProporties", menuName = "ScriptableObjects/BlockProporties", order = 1)]
public class BlockProperties: ScriptableObject
{
    public List<Block> blocks;
}

[Serializable]
public class Block
{
    public string tileName;
    public TileBase tileBase;
    public float timeToDestroy;
    public ParticleSystem destroyParticles;
}
