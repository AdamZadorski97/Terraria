using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{

    [SerializeField] private Vector2 floatingCanvasOffset;
    [SerializeField] private string triggerEnterMessage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<P_MoveController>())
        {
            FloatingCanvasController.Instance.TurnOnCanvas(triggerEnterMessage, transform.position + (Vector3)floatingCanvasOffset);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<P_MoveController>())
        {
            FloatingCanvasController.Instance.TurnOffCanvas();
        }
    }

    
}
