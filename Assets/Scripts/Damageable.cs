using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    Animator animator;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set
        {
            _maxHealth = value;
        }
    }
    [SerializeField]
    private int _health=100;

    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }
    [SerializeField]
    private bool _isAlive = true;
    [SerializeField]
    private bool isInvincible = false;
    private float timeSinceHit=0;
    public float invincibilityTime =0.25f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            animator.SetBool(AnimationStrings.isAlive, value);
            _isAlive = value;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Hit(int damage)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
        }
    }

    private void Update()
    {
        if (isInvincible)
        {
            if(timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
           
              timeSinceHit += Time.deltaTime;
           
        }
        Hit(10);
    }
}
