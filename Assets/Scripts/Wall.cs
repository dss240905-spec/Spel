using UnityEngine;

public class Wall : MonoBehaviour
{ 
    [SerializeField] private GameObject box;
    [SerializeField] private AudioClip buttonSound;

    private Animator anim;
    private AudioSource audioSource;

    private bool hasPlayedAanimation=false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&& !hasPlayedAanimation)
        {
            box.SetActive(false);
            hasPlayedAanimation = true;
            anim.SetTrigger("Move");

            if (buttonSound != null)
            {
                audioSource.PlayOneShot(buttonSound);
            }
        }
    }
}
