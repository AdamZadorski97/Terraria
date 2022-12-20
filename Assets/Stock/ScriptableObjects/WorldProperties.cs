using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldProperties", menuName = "ScriptableObjects/WorldProperties", order = 1)]
public class WorldProperties : ScriptableObject
{
    public int mapX0;
    public int mapX1;
    public int mapBiome;
}
