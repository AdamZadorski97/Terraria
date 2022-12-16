using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_BuildController : MonoBehaviour
{
    public GameObject ObjectToBuild;
    [SerializeField] private LayerMask groundLayer;
    private void Update()
    {
        if (InputController.Instance.Actions.buildAction.WasReleased)
        {
            Build();
        }
    }

    private void Build()
    {
      
        GameObject objectToBuild = Instantiate(ObjectToBuild);
        SnapObject(objectToBuild.transform);
    }
    private void SnapObject(Transform objectToBuild)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 5, groundLayer);
        if (hit.collider != null)
        {
            BoxCollider2D boxCollider2D = objectToBuild.GetComponent<BoxCollider2D>();
            objectToBuild.transform.position = hit.point + new Vector2(0, boxCollider2D.size.x/2);
        }
    }
}
