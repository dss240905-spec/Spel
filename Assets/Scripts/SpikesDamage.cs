using UnityEngine;

public class SpikesDamage : MonoBehaviour
{

    [SerializeField] private int damage = 10;
    [SerializeField] private bool destroyOnTouch = false; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovements player = other.GetComponent<PlayerMovements>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            if (destroyOnTouch)
            {
                Destroy(gameObject);
            }
        }
    }
}
