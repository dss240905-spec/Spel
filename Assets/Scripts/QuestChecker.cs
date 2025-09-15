using UnityEngine;
using UnityEngine.SceneManagement;
public class QuestChecker : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox, finishText, incompleteText;
    [SerializeField] private int questGoal = 20;
    [SerializeField] private int levelToLoad;

    private bool levelIsLoading = false;
  

    
    private void OnTriggerEnter2D(Collider2D other)
    {
         if (other.CompareTag("Player"))
         {
            if (other.GetComponent<PlayerMovements>().coinsCollected >= 20)
            {
               dialogueBox.SetActive(true);
               finishText.SetActive(true);
                SceneManager.LoadScene(levelToLoad);
                Invoke("LoadNextLevel", 20.0f);
                levelIsLoading = true;
               //TODO: ChangeLevel
            }
            else
            {
                dialogueBox.SetActive(true);
                incompleteText.SetActive(true);
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

