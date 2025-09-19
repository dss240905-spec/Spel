using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    public int damage = 1;
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyMovement>().TakeDamage(damage);
        }
    }
}
