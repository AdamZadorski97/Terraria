using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public PlayerProporties playerProporties;
    private Rigidbody2D rb;
    private Block lastMinedBlock;

    private void Awake()
    {
        IntializePlayer();
    }
    private void IntializePlayer()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MouseInput();
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        gameObject.transform.position = new Vector2(transform.position.x + (h * playerProporties.movementSpeed),
           transform.position.y + (v * playerProporties.movementSpeed));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(0, playerProporties.jumpForce);
        }

    }

    private void MouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mRay.origin, Vector2.zero, Mathf.Infinity);
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<Block>())
                {
                   
                    if(lastMinedBlock!=null)
                    {
                        if(lastMinedBlock != hit.collider.GetComponent<Block>())
                        {
                            lastMinedBlock.OnStopMining();
                        }
                    }
                    lastMinedBlock = hit.collider.GetComponent<Block>();


                    lastMinedBlock.OnMine();
                }
            }
        }
        else
        {
            if(lastMinedBlock!=null)
            {
                lastMinedBlock.OnStopMining();
            }
            
        }
    }
}
