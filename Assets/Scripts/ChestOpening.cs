using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour
{
    [Header("Chest Settings")]
    [SerializeField] private Animator chestAnimator;
    [SerializeField] private string openTriggerName = "Open";
    [SerializeField] private bool spawnOnlyOnce = true;

    [Header("Diamond Settings")]
    [SerializeField] private GameObject diamondPrefab;
    [SerializeField] private float diamondForce = 5f;
    [SerializeField] private float spawnOffsetY = 0.5f;
    [SerializeField] private float delayBeforeSpawn = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource; // assign in Inspector

    private bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        if (spawnOnlyOnce) isOpened = true;

        // Play chest opening animation
        if (chestAnimator != null)
        {
            chestAnimator.SetTrigger(openTriggerName);
        }

        // Play sound
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Spawn and animate the diamond
        StartCoroutine(SpawnDiamond());
    }

    private IEnumerator SpawnDiamond()
    {
        yield return new WaitForSeconds(delayBeforeSpawn);

        if (diamondPrefab == null) yield break;

        Vector3 spawnPos = transform.position + Vector3.up * spawnOffsetY;
        GameObject diamond = Instantiate(diamondPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb2d = diamond.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            Vector2 force = new Vector2(Random.Range(-0.8f, 0.8f), 1f).normalized * diamondForce;
            rb2d.AddForce(force, ForceMode2D.Impulse);
            rb2d.linearVelocity = force;
            rb2d.AddTorque(Random.Range(-2f, 2f), ForceMode2D.Impulse);
        }
    }
}
