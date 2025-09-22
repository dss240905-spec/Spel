using UnityEngine;
using UnityEngine.UI;

public class HealthSystemManager : MonoBehaviour
{

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Animator[] heartAnimators;


    public void SetHealth(int health)
    {

        for (int i = 0; i < heartAnimators.Length; i++)
        {
            if (i < health)
            {
                heartAnimators[i].SetBool("IsFull", true);
            }

            else
            {
                heartAnimators[i].SetBool("IsFull", false);
            }
        }
    }





}
