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

            bool shouldBeFull = i < health;
            
            if (heartAnimators[i].GetBool("IsFull") != shouldBeFull)
            {
                heartAnimators[i].SetBool("IsFull", shouldBeFull);
            }
        }
    }




}
