using UnityEngine;
using UnityEngine.UI;

public class HealthSystemManager : MonoBehaviour
{

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    


    public void SetHealth(int health)
    {

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = fullHeart;

            else
                hearts[i].sprite = emptyHeart;
        }
    }





}
