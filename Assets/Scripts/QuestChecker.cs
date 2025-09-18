using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestChecker : MonoBehaviour
{
    [SerializeField] private int questGoal = 20;
    [SerializeField] private int levelToLoad;
    [SerializeField] private Animator doorAnimator;
    
    // Remove the openAnimationName since we'll use a trigger instead
    private bool levelIsLoading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !levelIsLoading)
        {
            PlayerMovements player = other.GetComponent<PlayerMovements>();
            if (player != null && player.coinsCollected >= questGoal)
            {
                OpenDoor();
                levelIsLoading = true;
                Invoke("LoadNextLevel", 2.0f);
            }
        }
    }

    private void OpenDoor()
    {
        if (doorAnimator != null)
        {
            // Use SetTrigger instead of Play to trigger the animation transition
            doorAnimator.SetTrigger("OpenDoor");
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}