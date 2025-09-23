using UnityEngine;
using UnityEngine.UI;

public class HealthSystemManager : MonoBehaviour
{

    
    public Animator[] heartAnimators;


    public void SetHealth(int health)
    {

        for (int i = 0; i < heartAnimators.Length; i++)
        {

            bool shouldBeFull = i < health;

            if (heartAnimators[i].GetBool("IsFull") != shouldBeFull)
            {
                heartAnimators[i].SetBool("IsFull", shouldBeFull);


                if (shouldBeFull)
                {

                    AnimatorStateInfo refState = heartAnimators[0].GetCurrentAnimatorStateInfo(0);

                    if (refState.IsName("HeartIdle"))
                    {
                        heartAnimators[i].Play("HeartIdle", -1, refState.normalizedTime);
                    }
                }

            }
        }
    }
}
