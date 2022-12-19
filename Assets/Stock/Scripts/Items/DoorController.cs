using FunkyCode;
using FunkyCode.EventHandling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{


    [SerializeField] private Vector3 doorOpenScale;
    [SerializeField] private Vector3 doorCloseScale;
    [SerializeField] private Transform renderer;
    [SerializeField] private LightCollider2D lightCollider2D;
    private P_Sounds p_Sounds;

    private void Start()
    {
        p_Sounds = P_Sounds.Instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<P_MoveController>())
        {
            p_Sounds.PlaySound("DoorOpen");
            renderer.transform.localScale = doorOpenScale;
            lightCollider2D.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<P_MoveController>())
        {
            p_Sounds.PlaySound("DoorClose");
            renderer.transform.localScale = doorCloseScale;
            lightCollider2D.enabled = true;
        }
    }
}
