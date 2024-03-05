using System;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Goblin : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    Rigidbody2D rb;
    public enum WalkableDirection { Left, Right }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    TouchingDirections touchingDirections;
    public DetectionZone detectionZone;
    Animator animator;
    public WalkableDirection  WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                if(value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            
            }
            
        }
    }
    public bool _hasTarget = false;
    public bool HasTarget { get { return _hasTarget; }
        private set { 
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if(touchingDirections.IsGround && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
       
        rb.velocity = new Vector2(walkSpeed*walkDirectionVector.x, rb.velocity.y);
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else
        {
            Debug.LogError("Invalid WalkableDirection");
        }        
    }
    private void Update()
    {
        HasTarget = detectionZone.detectedColliders.Count > 0;
    }
}
