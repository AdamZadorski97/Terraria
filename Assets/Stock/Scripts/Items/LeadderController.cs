using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadderController : MonoBehaviour
{
    private P_MoveController p_moveController;


    private void Start()
    {
        p_moveController = P_MoveController.Instance;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(InputController.Instance.MoveValue().y!=0)
        if (collision.GetComponent<P_MoveController>())
        {
            p_moveController.IsOnLeadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<P_MoveController>())
        {
            p_moveController.IsOnLeadder = false;
        }
    }

}
