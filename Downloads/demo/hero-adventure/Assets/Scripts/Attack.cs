using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField]
    private int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            Vector2 deliverKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            damageable.Hit(attackDamage, deliverKnockback);
        }
    }
}
