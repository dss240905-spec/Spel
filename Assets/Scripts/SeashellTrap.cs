using UnityEngine;
using UnityEngine.Audio;

public class SeashellTrap : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject pearl;
    [SerializeField] private Animator animator;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private GameObject interactUIPrompt;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;

    private bool isOpened = false;
    private bool playerNearby = false;

    public void PlayOpenSound()
    {
        if (audioSource != null && openSound != null)
            audioSource.PlayOneShot(openSound);
    }

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (interactUIPrompt != null)
            interactUIPrompt.SetActive(false);
    }

    private void Update()
    {
        if (playerNearby && !isOpened && Input.GetKeyDown(interactKey))
        {
            OpenShell();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (interactUIPrompt != null)
                interactUIPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (interactUIPrompt != null)
                interactUIPrompt.SetActive(false);
        }
    }

    private void OpenShell()
    {
        isOpened = true;
        animator.SetBool("IsOpen", true);  // ✅ change trigger to bool

        if (interactUIPrompt != null)
            interactUIPrompt.SetActive(false);

        if (pearl != null)
            Destroy(pearl);

        if (mapPrefab != null)
        {
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position + Vector3.up * 1f;
            Instantiate(mapPrefab, spawnPos, Quaternion.identity);
        }
    }
}