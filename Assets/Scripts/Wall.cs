using UnityEngine;

public class Wall : MonoBehaviour
{ 
    [SerializeField] private GameObject box;
    private Animator anim;
    private bool hasPlayedAanimation=false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&& !hasPlayedAanimation)
        {
            box.SetActive(false);
            hasPlayedAanimation = true;
            anim.SetTrigger("Move");
        }
    }
}
