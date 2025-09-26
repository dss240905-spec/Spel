using UnityEngine;

public class TotemTrap : MonoBehaviour
{
    [SerializeField] private float attackInterval = 3f;
    [SerializeField] private Animator[] headAnimators; // assign each head's Animator
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private Transform[] firePoints;   // 3 mouths

    private bool playerInside = false;
    private float attackTimer = 0f;

    private void Start()
    {
        // Ensure heads start in Idle
        foreach (Animator anim in headAnimators)
        {
            anim.SetBool("IsActive", false);
        }
    }

    private void Update()
    {
        if (playerInside)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackInterval;
            }
        }
    }

    private void Attack()
    {
        // Play attack animation
        foreach (Animator anim in headAnimators)
        {
            anim.SetBool("IsActive", true);
        }

        // Fire spikes
        foreach (Transform firePoint in firePoints)
        {
            Instantiate(spikePrefab, firePoint.position, firePoint.rotation);
        }

        // Reset back to idle shortly after
        Invoke(nameof(ResetToIdle), 0.5f);
    }

    private void ResetToIdle()
    {
        foreach (Animator anim in headAnimators)
        {
            anim.SetBool("IsActive", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            attackTimer = 0f; // fire immediately on entering
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            ResetToIdle();
        }
    }
}
