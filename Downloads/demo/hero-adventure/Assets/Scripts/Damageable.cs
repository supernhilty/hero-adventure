using Assets.Scripts.Events;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;
    public UnityEvent gameOverEvent;
    public UnityEvent winGameEvent;

    [SerializeField] private int _maxHealth = 100;
    Animator animator;
    public bool LockVectocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVectocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVectocity, value);
        }
    }
    public int MaxHealth
    {
        get { return _maxHealth; }
        set
        {
            _maxHealth = value;
        }
    }
    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);
            if (_health <= 0)
            {
                IsAlive = false;
                if (gameOverEvent != null)
                {
                    gameOverEvent.Invoke();
                    if (gameObject.CompareTag("Player"))
                    {
                        Time.timeScale = 0;
                    }        
                }
            }
        }
    }
    [SerializeField]
    private bool _isAlive = true;
    [SerializeField]
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

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

            if (value == false)
            {
                damageableDeath.Invoke();
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
            animator.SetTrigger(AnimationStrings.hit);
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged?.Invoke(gameObject, damage);
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;

        }

    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;

            CharacterEvents.characterHealed(gameObject, healthRestore);
            return true;
        }

        return false;
    }
}
