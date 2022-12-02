using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldParameters", menuName = "ScriptableObjects/WorldParameters", order = 1)]
public class WorldParameters : ScriptableObject
{
    public Vector2 worldSize;
}
