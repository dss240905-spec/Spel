using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestChecker : MonoBehaviour
{
    [Header("Quest Settings")]
    [Tooltip("Number of coins or diamonds required to finish the quest")]
    [SerializeField] private int questGoal = 20;

    [Tooltip("Set this to true if the quest should require diamonds instead of coins")]
    [SerializeField] private bool requiresDiamonds = false;

    [Header("Level Settings")]
    [SerializeField] private int levelToLoad;

    [Header("Door Settings")]
    [SerializeField] private Animator doorAnimator;

    private bool levelIsLoading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !levelIsLoading)
        {
            PlayerMovements player = other.GetComponent<PlayerMovements>();
            if (player != null)
            {
                int playerProgress = requiresDiamonds ? player.diamondsCollected : player.coinsCollected;

                if (playerProgress >= questGoal)
                {
                    OpenDoor();
                    levelIsLoading = true;
                    Invoke(nameof(LoadNextLevel), 2.0f);
                }
                else
                {
                    Debug.Log("You haven't collected enough yet!");
                }
            }
        }
    }

    private void OpenDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("OpenDoor");
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
