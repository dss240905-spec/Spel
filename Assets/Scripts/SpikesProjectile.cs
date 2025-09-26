using UnityEngine;

public class SpikesProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 5f;

    private Vector2 direction;

    private void Start()
    {
        if (transform.root.localScale.x < 0)
            direction = Vector2.left;
        else
            direction = Vector2.right;

        Destroy(gameObject, lifetime);
    }

   

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovements player = other.GetComponent<PlayerMovements>();
            if(player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
