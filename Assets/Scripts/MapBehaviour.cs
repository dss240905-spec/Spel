using UnityEngine;
using UnityEngine.Audio;

public class MapBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float revealDuration = 2f; // how long map stays unfolded
    [SerializeField] private float foldDuration = 1f;   // how long fold animation lasts

    [SerializeField] private AudioClip pickupSound;

    private bool isRevealed = false;
    private bool isCollectible = false;
    private bool playerNearby = false;
    private AudioSource audioSource;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

        // Start closed
        animator.SetBool("IsOpen", false);
        animator.SetBool("IsClosed", true);

        // Begin unfolding right away when spawned
        UnfoldMap();
    }
    private void Update()
    {

    }


    private void UnfoldMap()
    {
        animator.SetBool("IsClosed", false);
        animator.SetBool("IsOpen", true);
        isRevealed = true;

        // After some time, fold the map back
        Invoke(nameof(FoldMap), revealDuration);
    }

    private void FoldMap()
    {
        if (isRevealed)
        {
            animator.SetBool("IsOpen", false);
            animator.SetBool("IsClosed", true);

            isRevealed = false;

            // After fold animation ends, make the map collectible
            Invoke(nameof(MakeCollectible), foldDuration);
        }
    }

    private void MakeCollectible()
    {
        isCollectible = true; }
 
    private void CollectMap()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                
                inventory.AddItem("Map", null);
            }
            Destroy(gameObject, 0.2f); // small delay so sound plays
        }

        Debug.Log("Player picked up the map!");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
    public bool IsCollectible()
    {
        return isCollectible;
    }

    public void CollectMapFromPlayer()
    {
        CollectMap();
    }

}
