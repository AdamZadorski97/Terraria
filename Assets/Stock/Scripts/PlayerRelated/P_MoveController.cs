using UnityEngine;

public class P_MoveController : MonoBehaviour
{
    public static P_MoveController Instance { get; private set; }



    private P_Sounds p_Sounds;
    private PlayerProperties playerProporties;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private Vector2 inputValue;
    private Vector2 moveVector;
    private float runSpeedValue;
    private float jumpsCount;
    private bool grounded;
    private bool isOnLeadder;
    private bool isInJumpState;
    private float jumpStateTime;
    public bool IsOnLeadder
    {
        get
        {
            return isOnLeadder;
        }
        set
        {
            isOnLeadder = value;
        }
    }

    public bool Grounded
    {
        get
        {
            return grounded;
        }
        set
        {
            if (!grounded && value == true)
            {
                OnGroundHit();
            }
            grounded = value;
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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
        p_Sounds = GetComponent<P_Sounds>();
    }

    private void Update()
    {
        CheckJump();
        CheckRun();
        CheckGrounded();
        inputValue = InputController.Instance.MoveValue();
        rb.velocity = moveVector;

    }
    private void LateUpdate()
    {
        Movement();
    }

    private void SetFacing(float value)
    {
        transform.localScale = new Vector3(value, 1, 1);
    }

    private void Movement()
    {
        if(isInJumpState)
        {
            jumpStateTime += Time.deltaTime;
            if(jumpStateTime>0.25f)
            {
                isInJumpState = false;
                jumpStateTime = 0;
            }
        }

        if (IsOnLeadder && !isInJumpState)
        {
            LeaderMovement();
            return;
        }



        VerticalAirboneMovement();

        if (inputValue.x != 0) SetFacing(inputValue.x);

        if (inputValue.x < 0 && CheckWall(Vector2.left) || inputValue.x > 0 && CheckWall(Vector2.right))
        {
            moveVector = new Vector2(0, moveVector.y);
            return;
        }

        HorizontalAirboneMovement();
        HorizontalGroundedMovement();
    }

    private void LeaderMovement()
    {
        Debug.Log(inputValue.y);
        moveVector.x = Mathf.Lerp(moveVector.x, 0 + inputValue.x, Time.deltaTime *10);
        moveVector.y = inputValue.y  * 4;
    }





    private void HorizontalGroundedMovement()
    {
        if (Grounded)
        {
            if (inputValue != Vector2.zero)
            {
                moveVector = Vector2.Lerp(moveVector, new Vector2(inputValue.x * (playerProporties.maxGroundedSpeed + runSpeedValue), moveVector.y), Time.deltaTime * playerProporties.horizontalGroundedAcceleration);
            }
            else
            {
                moveVector = Vector2.Lerp(moveVector, new Vector2(inputValue.x * playerProporties.maxGroundedSpeed, moveVector.y), Time.deltaTime * playerProporties.groundedSpeedLose);
            }
            if (moveVector.x > 0.1f || moveVector.x < -0.1f) p_Sounds.FootStep(moveVector.x);
        }
    }

    private void HorizontalAirboneMovement()
    {
        if (!Grounded)
        {
            moveVector = Vector2.Lerp(moveVector, new Vector2(inputValue.x * playerProporties.maxHorizontalAirboneSpeed, moveVector.y), Time.deltaTime * playerProporties.horizontalAirboneSpeedLose);
        }
    }

    private void VerticalAirboneMovement()
    {
        if (!Grounded)
        {
            moveVector = Vector2.Lerp(moveVector, new Vector2(moveVector.x, moveVector.y - playerProporties.gravity), Time.deltaTime * playerProporties.verticalAirboneSpeedLose);
        }
        else
        {
            moveVector.y = 0;
        }
    }

    private void CheckGrounded()
    {
        Vector2 position = capsuleCollider.transform.position + (Vector3)capsuleCollider.offset - new Vector3(capsuleCollider.size.x / 1.75f, capsuleCollider.size.y / 2) - new Vector3(0, 0.1f, 0);
        Vector2 direction = Vector2.right;
        float distance = capsuleCollider.size.x / 1.5f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            jumpsCount = 0;
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }

    }

    private bool CheckWall(Vector2 direction)
    {
        Vector2 position = capsuleCollider.transform.position + (Vector3)capsuleCollider.offset - new Vector3(0, capsuleCollider.size.y / 2) + new Vector3(0, 0.03f, 0);
        float distance = (capsuleCollider.size.x / 2f) + 0.25f;
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

        if (!Grounded)
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

        if (!Grounded && jumpsCount > 1)
        {
            return;
        }
        if (jumpsCount == 0)
        {
            p_Sounds.PlaySound("Jump");
        }
        else
        {
            p_Sounds.PlaySound("DoubleJump");
        }
        jumpsCount++;
        transform.position += new Vector3(0, 0.1f, 0);
        moveVector = new Vector2(moveVector.x, playerProporties.jumpForce);
        isInJumpState = true;
    }
    public void OnGroundHit()
    {
        p_Sounds.PlaySound("JumpEnd");
    }
}





