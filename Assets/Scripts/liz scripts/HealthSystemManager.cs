using UnityEngine;
using UnityEngine.UI;

public class HealthSystemManager : MonoBehaviour
{

    
    public Animator[] heartAnimators;


    public void SetHealth(int health)
    {

        float refTime = 0f;
        if (heartAnimators.Length > 0)
        {
            AnimatorStateInfo refState = heartAnimators[0].GetCurrentAnimatorStateInfo(0);
            refTime = refState.normalizedTime % 1f;
        }
        


        for (int i = 0; i < heartAnimators.Length; i++)
        {

            bool shouldBeFull = i < health;

            if (heartAnimators[i].GetBool("IsFull") != shouldBeFull)
            {
                heartAnimators[i].SetBool("IsFull", shouldBeFull);


                if (shouldBeFull)
                {

                    heartAnimators[i].Play("HeartIdle", -1, refTime);
                }
                else
                {
                    heartAnimators[i].Play("HeartBreak", -1, 0f);
                }
            }
        }
    }
}
