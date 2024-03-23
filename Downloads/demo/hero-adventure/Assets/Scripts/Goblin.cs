using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class Goblin : MonoBehaviour
{
    public float walkSpeed = -3.0f;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    public DetectionZone detectionZone;
    public DetectionZone cliffdetectionZone;
    Animator animator;
    Damageable damageable;
   
    public bool _hasTarget = false;
    public float walkStopRate = 0.05f;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.AttackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.AttackCooldown, Mathf.Max(value,0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }
    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
            walkSpeed = - walkSpeed;
        }
       
        if (!damageable.LockVectocity)
        {
            if (CanMove)
            {
               
                    rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
               
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
           
        }    
    }

    private void FlipDirection()
    {

        gameObject.transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
    private void Update()
    {
        HasTarget = detectionZone.detectedColliders.Count > 0;

        if(AttackCooldown >0)
        {
            AttackCooldown -= Time.deltaTime;
        }
        
    }

    public void OnHit(int damage, Vector2 knockback)
    {

        rb.velocity = new Vector2(knockback.x, knockback.y + rb.velocity.y);
        Debug.Log("Goblin Hit");
    }
    public void OnCliffDetected()
    {
        if (!touchingDirections.IsGrounded)
        {
            FlipDirection();
            Debug.Log("Cliff Detected");
        }
    }
}
