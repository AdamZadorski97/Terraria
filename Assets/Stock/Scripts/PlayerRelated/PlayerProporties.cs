using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProporties", menuName = "ScriptableObjects/PlayerProporties", order = 1)]
public class PlayerProporties : ScriptableObject
{
    public float movementSpeed;
    public float jumpForce;
}
