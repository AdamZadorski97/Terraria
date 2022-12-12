using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldProperties", menuName = "ScriptableObjects/WorldProperties", order = 1)]
public class WorldProperties : ScriptableObject
{
    public Vector2 worldSize;
}
