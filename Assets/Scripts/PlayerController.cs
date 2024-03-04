using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float airRunSpeed = 2f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deadkick = new Vector2(20f, 20f);
    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;
    SpriteRenderer spriteRenderer;
    float gravityScaleAtStart;
    bool isAlive = true;
    TouchingDirections touchingDirections;
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = rb2d.gravityScale;
        touchingDirections = GetComponent<TouchingDirections>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;
        Run();
        FlipSprite();
        //ClimbLadder();
        Die();
    }
    private void FixedUpdate()
    {
        animator.SetFloat(AnimationStrings.yVeclocity, rb2d.velocity.y);

    }

    void Die()
    {
        if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            isAlive = false;
            animator.SetTrigger("Hurting");
            rb2d.velocity = deadkick;
            //StartAgain();
        }
    }
    //void StartAgain()
    //{
    //    SceneManager.LoadScene(0);
    //}

    //void ClimbLadder()
    //{
    //    if (!capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
    //    {
    //        rb2d.gravityScale = gravityScaleAtStart;
    //        animator.SetBool("isClimbing", false);
    //        return;
    //    }
    //    bool playerHasVerticalSpeed = Mathf.Abs(rb2d.velocity.y) > Mathf.Epsilon;
    //    animator.SetBool("isClimbing", playerHasVerticalSpeed);
    //    rb2d.gravityScale = 0f;
    //    Vector2 climbVectocity = new Vector2(rb2d.velocity.x, moveInput.y * climbSpeed);
    //    rb2d.velocity = climbVectocity;
    //}

    private void FlipSprite()
    {
        float moveInputValue = Input.GetAxis("Horizontal");

        if(moveInputValue < 0)
        {
            spriteRenderer.flipX = true;
        }
        if(moveInputValue > 0)
        {
            spriteRenderer.flipX = false;
        }

        //bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        //if (playerHasHorizontalSpeed)
        //    transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
    }

    void Run()
    {
        Vector2 playerVectocity;
        if (CanMove)
        {
            if (touchingDirections.IsGround)
            {
                playerVectocity = new Vector2(moveInput.x * runSpeed, rb2d.velocity.y);
            }
            else
            {
                playerVectocity = new Vector2(moveInput.x * airRunSpeed, rb2d.velocity.y);
            }
        }
        else
        {
            playerVectocity = new Vector2(0, rb2d.velocity.y);

        }


        rb2d.velocity = playerVectocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        animator.SetBool(AnimationStrings.isRunning, playerHasHorizontalSpeed);

    }

    void OnMove(InputValue value)
    {
        if (!isAlive)
            return;
        moveInput = value.Get<Vector2>();
        if (!touchingDirections.IsOnWall)
        {
            moveInput = value.Get<Vector2>();
        }
        else
        {
            moveInput = new Vector2(0, 0);
        }

    }

    void OnAttack(InputValue value)
    {
        if (!isAlive)
            return;
        animator.SetTrigger("attack");
    }

	void Attack()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			animator.SetTrigger("attack");
		}
		else
		{
			animator.ResetTrigger("attack");
		}
	}
	//void OnJump(InputValue value)
	//{
	//    if (!isAlive)
	//        return;
	//    if (!capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
	//        return;
	//    if (value.isPressed)
	//    {
	//        rb2d.velocity += new Vector2(0f, jumpSpeed);
	//    }
	//}

    void OnJump(InputValue value)
    {
        if (!isAlive)
            return;

        if (touchingDirections.IsGround && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jump);

            rb2d.velocity += new Vector2(rb2d.velocity.x, jumpSpeed);

        }
    }
}
