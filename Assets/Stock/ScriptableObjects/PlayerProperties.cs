using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PlayerProperties", menuName = "ScriptableObjects/PlayerProperties", order = 1)]
public class PlayerProperties : ScriptableObject
{
    [BoxGroup("Movement")]
    public float verticalAirboneSpeedLose;
    [BoxGroup("Movement")]
    public float horizontalAirboneSpeedLose;
    [BoxGroup("Movement")]
    public float horizontalGroundedAcceleration;

    [BoxGroup("Movement")]
    public float maxHorizontalAirboneSpeed;
    [BoxGroup("Movement")]
    public float groundedSpeedLose;
    [BoxGroup("Movement")]
    public float maxGroundedSpeed;
    [BoxGroup("Movement")]

    public float runSpeed;
    [BoxGroup("Movement")]
    public float jumpForce;
    [BoxGroup("Movement")]
    public float gravity;

    [BoxGroup("Mining")]
    public float timeToMineResources;

    [BoxGroup("Stats")]
    [BoxGroup("Stats/Oxygen")]
    public float oxygenLoseFrequency = 0.25f;
    [BoxGroup("Stats/Oxygen")]
    public float oxygenLoseValue = 0.1f;
    [BoxGroup("Stats/Health")]
    public float healthLoseFrequency;
    [BoxGroup("Stats/Health")]
    public float healthLoseValue;

    [BoxGroup("Camera")]
    public Vector2 zoomMinMax;
    [BoxGroup("Camera")]
    public float zoomSpeed;
    [BoxGroup("Camera")]
    public Vector2 lookOffset;
    [BoxGroup("Camera")]
    public float lookSpeed;

    [BoxGroup("Sounds")]
    public float footStepRate;

}
