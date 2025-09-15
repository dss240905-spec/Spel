using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject CreditsPanel;
  public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
 public void QuitGame()
    {
        Application.Quit();
    }
    public void ShowCredits() 
    {
       CreditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        CreditsPanel.SetActive(false);
    }
}
