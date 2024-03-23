using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float airRunSpeed = 2f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 1f;
    [SerializeField] Vector2 deadkick = new Vector2(20f, 20f);
    public UnityEvent winGameEvent;
    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;
    float gravityScaleAtStart;
    TouchingDirections touchingDirections;
    Damageable damageable;
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    public bool IsRunning { get; set; }


    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = rb2d.gravityScale;
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsAlive)
            return;
        if (!damageable.LockVectocity)
        {
            Run();
        }
        IsRunning = animator.GetBool(AnimationStrings.isRunning);
        FlipSprite();

        ClimbLadder();

    }
    private void FixedUpdate()
    {
        animator.SetFloat(AnimationStrings.yVeclocity, rb2d.velocity.y);

    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    void EndGame()
    {
        if(winGameEvent != null)
        {
            winGameEvent.Invoke();
            Time.timeScale = 0;
        }
    }
    void UpLevel()
    {
        SceneManager.LoadScene(2);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
        {
            Invoke("UpLevel", 0.5f);
        }
        if(collision.tag == "EndGame")
        {
            Invoke("EndGame", 0.5f);
            Debug.Log("Win game");
        }
    }


    void ClimbLadder()
    {
        if (!capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            rb2d.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }
        bool playerHasVerticalSpeed = Mathf.Abs(rb2d.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
        rb2d.gravityScale = 0f;
        Vector2 climbVectocity = new Vector2(rb2d.velocity.x, moveInput.y * climbSpeed);
        rb2d.velocity = climbVectocity;
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        Debug.Log(rb2d.velocity.x);
        if (playerHasHorizontalSpeed)
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
    }

    void Run()
    {
        Vector2 playerVectocity;



        if (CanMove)
        {

            if (touchingDirections.IsGrounded)
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
        Debug.Log("Run:" + rb2d.velocity.x.ToString());
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        animator.SetBool(AnimationStrings.isRunning, playerHasHorizontalSpeed);
        if (touchingDirections.IsOnWall)
        {
            animator.SetBool(AnimationStrings.isRunning, false);
        }

    }

    void OnMove(InputValue value)
    {
        if (!IsAlive)
            return;

        moveInput = value.Get<Vector2>();
    }

    void OnAttack(InputValue value)
    {
        if (!IsAlive)
            return;
        animator.SetTrigger(AnimationStrings.attack);
    }

    void OnJump(InputValue value)
    {
        if (!IsAlive)
            return;

        if (touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jump);

            rb2d.velocity += new Vector2(rb2d.velocity.x, jumpSpeed);


        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {

        rb2d.velocity = new Vector2(knockback.x, knockback.y + rb2d.velocity.y);
        Debug.Log("Player Hit");
    }
}
