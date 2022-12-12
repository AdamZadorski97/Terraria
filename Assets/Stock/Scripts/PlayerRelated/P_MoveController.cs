using UnityEngine;

public class P_MoveController : MonoBehaviour
{
    private PlayerProperties playerProporties;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private Vector2 inputValue;
    private Vector2 moveVector;
    private float runSpeedValue;


    private void Awake()
    {
        IntializePlayer();
    }

    private void Start()
    {
        playerProporties = ScriptableManager.Instance.playerProperties;
    }

    private void IntializePlayer()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckJump();
        CheckRun();
        inputValue = InputController.Instance.MoveValue();
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
            if (inputValue != Vector2.zero)
            {
                moveVector = Vector2.Lerp(moveVector, new Vector2(inputValue.x * (playerProporties.maxGroundedSpeed + runSpeedValue) , moveVector.y), Time.deltaTime * playerProporties.horizontalGroundedAcceleration);
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
        Vector2 position = capsuleCollider.transform.position + (Vector3)capsuleCollider.offset - new Vector3(capsuleCollider.size.x / 1.75f, capsuleCollider.size.y / 2) - new Vector3(0, 0.1f, 0);
        Vector2 direction = Vector2.right;
        float distance = capsuleCollider.size.x / 1.5f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }


    private void CheckRun()
    {
        if (!InputController.Instance.Actions.runAction.IsPressed)
        {
            runSpeedValue = 0;
            return;
        }

        if (!IsGrounded())
        {
            runSpeedValue = 0;
            return;
        }
        runSpeedValue = playerProporties.runSpeed;
    }

    private void CheckJump()
    {
        if (!InputController.Instance.Actions.jumpAction.WasPressed)
        {
            return;
        }

        if (!IsGrounded())
        {
            return;
        }
        transform.position += new Vector3(0, 0.1f, 0);
        moveVector = new Vector2(moveVector.x, playerProporties.jumpForce);
    }
}





