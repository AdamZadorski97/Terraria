using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public PlayerProporties playerProporties;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private Block lastMinedBlock;

    public Vector2 inputValue;
    public Vector2 moveVector;
    public LayerMask groundLayer;

    private void Awake()
    {
        IntializePlayer();
    }
    private void IntializePlayer()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MouseInput();
        KeyboardInput();

        rb.velocity = moveVector;
    }
    private void LateUpdate()
    {
        HorizontalAirboneMovement();
        VerticalAirboneMovement();
        HorizontalGroundedMovement();
    }

    private void HorizontalGroundedMovement()
    {
        if (IsGrounded())
        {
            if(inputValue!=Vector2.zero)
            {
                moveVector = Vector2.Lerp(moveVector, new Vector2(inputValue.x * playerProporties.maxGroundedSpeed, moveVector.y), Time.deltaTime * playerProporties.horizontalGroundedAcceleration);
            }
            else
            {
                moveVector = Vector2.Lerp(moveVector, new Vector2(inputValue.x * playerProporties.maxGroundedSpeed, moveVector.y), Time.deltaTime * playerProporties.groundedSpeedLose);
            }
           
        }
    }


    private void HorizontalAirboneMovement()
    {
        if (!IsGrounded())
        {
            moveVector = Vector2.Lerp(moveVector, new Vector2(inputValue.x * playerProporties.maxHorizontalAirboneSpeed, moveVector.y), Time.deltaTime * playerProporties.horizontalAirboneSpeedLose);
        }
    }

    private void VerticalAirboneMovement()
    {
        if (!IsGrounded())
        {
            moveVector = Vector2.Lerp(moveVector, new Vector2(moveVector.x, moveVector.y - playerProporties.gravity), Time.deltaTime * playerProporties.verticalAirboneSpeedLose);
        }
        else
        {
            moveVector.y = 0;
        }
    }


    private bool IsGrounded()
    {
        Vector2 position = capsuleCollider.transform.position + (Vector3)capsuleCollider.offset - new Vector3(capsuleCollider.size.x/2, capsuleCollider.size.y / 2) - new Vector3(0,0.1f,0);
        Vector2 direction = Vector2.right;
        float distance = capsuleCollider.size.x / 2;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }



    private void Jump()
    {
        if (!IsGrounded()) return;
        transform.position += new Vector3(0, 0.1f, 0);
        moveVector = new Vector2(moveVector.x, playerProporties.jumpForce);
    }
    private void KeyboardInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        inputValue = new Vector2(h, v);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
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

                    if (lastMinedBlock != null)
                    {
                        if (lastMinedBlock != hit.collider.GetComponent<Block>())
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
            if (lastMinedBlock != null)
            {
                lastMinedBlock.OnStopMining();
            }

        }
    }
}
