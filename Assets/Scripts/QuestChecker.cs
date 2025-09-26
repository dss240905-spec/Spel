using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestChecker : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox, finishText, incompleteText;
    [SerializeField] private int questGoal = 20;
    [SerializeField] private int diamondGoal = 1;
    [SerializeField] private int levelToLoad;

    private bool levelIsLoading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovements player = other.GetComponent<PlayerMovements>();
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (player != null && inventory != null)
            {
                if (player.coinsCollected >= questGoal &&
                    player.diamondsCollected >= diamondGoal &&
                    inventory.HasMap())
                {
                    dialogueBox.SetActive(true);
                    finishText.SetActive(true);

                    if (!levelIsLoading)
                    {
                        levelIsLoading = true;
                        Invoke(nameof(LoadNextLevel), 2f); // wait 2 seconds
                    }
                }
                else
                {
                    dialogueBox.SetActive(true);
                    incompleteText.SetActive(true);
                }
            }
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !levelIsLoading)
        {
            dialogueBox.SetActive(false);
            finishText.SetActive(false);
            incompleteText.SetActive(false);
        }
    }
}
