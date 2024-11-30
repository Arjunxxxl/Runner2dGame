using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input")] 
    [SerializeField] private Vector2 input;

    [Header("Movement")] 
    [SerializeField] private Vector2 moveDir;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveSpeedSmoothIncrement;
    private Vector2 vel;

    [Header("Jump")] 
    [SerializeField] private bool jump;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isJumpingFromKill;
    [SerializeField] private float jumpForce;
    [SerializeField] private float enemyKillJumpForce;
    [SerializeField] private float damageTakenJumpForce;
    
    [Header("Ground")]
    [SerializeField] private bool isGroundedPhantom;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool wasGrounded;
    [SerializeField] private float groundedPhantomTime;
    [SerializeField] private float groundedPhantomTimeELapsed;
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Look Dir")] 
    [SerializeField] private GameObject characterObj;
    [SerializeField] private Vector3 characterScale;
    
    private Rigidbody2D rb;
    private Player player;

    public float VelocityX => rb.linearVelocityX;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    internal void SetUp(Player player)
    {
        this.player = player;
        
        rb = GetComponent<Rigidbody2D>();
        
        characterScale = new Vector3(1, 1, 1);
        characterObj.transform.localScale = characterScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetGameState() == GameState.GameComplete)
        {
            rb.linearVelocity = Vector2.zero;
            
            SetRunAnimation();
            SetJumpingAnimation();
            SetFallingAnimation();
            
            return;
        }
        
        if (GameManager.Instance.GetGameState() != GameState.Gameplay)
        {
            SetRunAnimation();
            SetJumpingAnimation();
            SetFallingAnimation();
            
            return;
        }
        
        GetInput();
        SetMoveDir();
        
        CheckGrounded();
        GroundPhantomTime();
        
        SetCharacterLookDir();

        SetRunAnimation();
        SetJumpingAnimation();
        SetFallingAnimation();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GetGameState() != GameState.Gameplay)
        {
            return;
        }
        
        Move();
        Jump();
    }

    #region Input
    
    private void GetInput()
    {
        if (player.playerHp.IsPlayerInventuable())
        {
            input = Vector2.zero;
            jump = false;
            return;
        }

        input.x = UserInput.Instance.MoveDir;
        jump = UserInput.Instance.IsJumping;
    }
    
    #endregion

    #region Move

    private void SetMoveDir()
    {
        moveDir.x = input.x;
    }

    private void Move()
    {
        Vector3 targetVelocity = new Vector2(moveDir.x * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity,
            targetVelocity,
            ref vel,
            Time.fixedDeltaTime * moveSpeedSmoothIncrement);
    }

    #endregion

    #region Jump

    private void Jump()
    {
        if (jump && isGrounded && !isJumping)
        {
            isJumping = true;
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Force);

            SoundManager.Instance.PlayAudio("jump");
        }
    }

    internal void JumpOnEnemyKill()
    {
        if (rb.linearVelocityY < 0)
        {
            isJumpingFromKill = false;
        }
        
        if (isJumpingFromKill)
        {
            return;
        }
        
        isJumping = true;
        isJumpingFromKill = true;
        
        Vector2 oldVelocity = rb.linearVelocity;
        oldVelocity.y = 0;
        rb.linearVelocity = oldVelocity;
        
        rb.AddForce(new Vector2(0f, enemyKillJumpForce), ForceMode2D.Force);
    }
    
    internal void JumpOnDmgTaken()
    {
        isJumping = true;
        
        Vector2 velocityDir = rb.linearVelocity.normalized;
        velocityDir *= -1;

        if (rb.linearVelocity.magnitude < 0.1f)
        {
            velocityDir = Vector2.up;
        }

        rb.linearVelocity = Vector2.zero;

        Vector3 force = velocityDir * damageTakenJumpForce;
        
        rb.AddForce(force, ForceMode2D.Force);

        SetHurtAnimation(true);
    }

    #endregion

    #region Grounded

    private void CheckGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + groundCheckOffset,
            groundCheckRadius, groundLayerMask);
        
        isGroundedPhantom = colliders.Length > 0;

        if (isGroundedPhantom)
        {
            isGrounded = true;
            
            if (!wasGrounded)
            {
                //TODO: Play land effects    
            }
            
            isJumping = false;
            isJumpingFromKill = false;
            groundedPhantomTimeELapsed = 0;
            wasGrounded = isGrounded;
        }
    }

    private void GroundPhantomTime()
    {
        if (!isGroundedPhantom)
        {
            groundedPhantomTimeELapsed += Time.deltaTime;

            if (groundedPhantomTimeELapsed >= groundedPhantomTime)
            {
                isGrounded = false;
                wasGrounded = isGrounded;
            }
        }
    }

    #endregion

    #region LookDir

    private void SetCharacterLookDir()
    {
        if (rb.linearVelocityX > 0.1f)
        {
            characterScale.x = 1;
        }
        else if (rb.linearVelocityX < -0.1f)
        {
            characterScale.x = -1;
        }
        
        characterObj.transform.localScale = characterScale;
    }

    #endregion

    #region Aniamtion

    private void SetRunAnimation()
    {
        player.playerAnimator.PlayAnim(PlayerAnimationType.Running, Mathf.Abs(rb.linearVelocityX) > 0.01f);
    }

    private void SetJumpingAnimation()
    {
        player.playerAnimator.PlayAnim(PlayerAnimationType.Jumping, rb.linearVelocityY > 0.01f  && !player.playerHp.IsPlayerInventuable());
    }

    private void SetFallingAnimation()
    {
        player.playerAnimator.PlayAnim(PlayerAnimationType.Falling, rb.linearVelocityY < -0.01f && !player.playerHp.IsPlayerInventuable());
    }

    internal void SetHurtAnimation(bool play)
    {
        player.playerAnimator.PlayAnim(PlayerAnimationType.Hurt, play);
    }

    #endregion
    
    #region Gizmo

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position + groundCheckOffset, groundCheckRadius);
    }

    #endregion
}
