using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_BuildController : MonoBehaviour
{
    public GameObject ObjectToBuild;

    private void Update()
    {
        if (InputController.Instance.Actions.buildAction.WasReleased)
        {
            Build();
        }
    }

    private void Build()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject objectToBuild = Instantiate(ObjectToBuild);
        objectToBuild.transform.position = new Vector3(pos.x, pos.y, 0);
    }
}
