using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.0f;
    private bool canMove = true;
    [SerializeField] private SpriteRenderer rend; // Assign in Inspector for flashing

    [Header("Stats")]
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    [Header("Boss Settings")]
    [SerializeField] private bool isBoss = false; // Check this for the boss
    [SerializeField] private GameObject doorToOpen; // Assign the door in Inspector

    [Header("Bounce/Knockback")]
    [SerializeField] private float bounciness = 100f;
    [SerializeField] private float knockbackForce = 200f;
    [SerializeField] private float upwardForce = 100f;
    [SerializeField] private int damageGiven = 1;

    [Header("Flash Settings")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.2f;

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        currentHealth = maxHealth;

    rend = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>();
    
    canMove = true;
        if (rend == null)
            rend = GetComponent<SpriteRenderer>();
    
    }


    private void FixedUpdate()
    {
        if (!canMove) return;

        // Move horizontally
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        // Flip sprite based on direction
        if (rend != null)
            rend.flipX = moveSpeed < 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reverse direction when hitting walls or other enemies
        if (collision.gameObject.CompareTag("EnemyBlock") || collision.gameObject.CompareTag("Enemy"))
        {
            moveSpeed = -moveSpeed;
        }

        // Damage the player
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovements player = collision.gameObject.GetComponent<PlayerMovements>();
            if (player != null)
            {
                player.TakeDamage(1);
                float direction = collision.transform.position.x > transform.position.x ? 1 : -1;
                player.TakeKnockBack(200f * direction, 100f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            other.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(other.GetComponent<Rigidbody2D>().linearVelocity.x, 0);
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bounciness));

            GetComponent<Animator>().SetTrigger("Hit");

            if (audioSource != null)
            {
                audioSource.Play();
            }

            
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            canMove = false;

            Destroy(gameObject,0.5f);
        }

        // Bounce the player when jumping on the enemy
        Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0);
            playerRb.AddForce(new Vector2(0, bounciness), ForceMode2D.Impulse);
        }

        // Take damage
        TakeDamage(1);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Flash(); // Flash once per hit

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        canMove = false;

        // Disable colliders
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
        }

        // Only open/destroy the door if this is the boss
        if (isBoss && doorToOpen != null)
        {
            Destroy(doorToOpen); // or trigger an Animator instead
        }

        Destroy(gameObject, 0.5f); // destroy the enemy after short delay
    }

    private void Flash()
    {
        if (rend != null)
            StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        Color originalColor = rend.color;
        rend.color = flashColor; // turn white
        yield return new WaitForSeconds(flashDuration);
        rend.color = originalColor; // back to normal
    }
}
