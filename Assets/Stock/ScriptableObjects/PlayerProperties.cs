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
    public float runSpeed;
    public float jumpForce;
    public float gravity;

    public float timeToMineResources;

    public float oxygenLoseFrequency = 0.25f;
    public float oxygenLoseValue = 0.1f;

    public Vector2 zoomMinMax;
    public float zoomSpeed;
    public Vector2 lookOffset;
    public float lookSpeed;

    public float footStepRate;

}
