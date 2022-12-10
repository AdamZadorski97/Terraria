using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProperties", menuName = "ScriptableObjects/PlayerProperties", order = 1)]
public class PlayerProperties : ScriptableObject
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
