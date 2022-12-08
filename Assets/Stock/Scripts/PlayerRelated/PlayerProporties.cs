using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProporties", menuName = "ScriptableObjects/PlayerProporties", order = 1)]
public class PlayerProporties : ScriptableObject
{
    public float verticalAirboneSpeedLose;
    public float horizontalAirboneSpeedLose;

    public float horizontalGroundedAcceleration;


    public float maxHorizontalAirboneSpeed;

    public float groundedSpeedLose;
    public float maxGroundedSpeed;
    public float movementSpeed;
    public float jumpForce;
    public float gravity;

    public float timeToMineResources;

    public Vector2 lookOffset;
    public float lookSpeed;
}
