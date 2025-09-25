using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool destroyOnOpen = false;  // Destroy or animate
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string openTriggerName = "Open"; // Name of animation trigger

    public void OpenDoor()
    {
        if (destroyOnOpen)
        {
            Destroy(gameObject);
        }
        else if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(openTriggerName);
        }
        else
        {
            Debug.LogWarning("DoorController: No Animator assigned for animation!");
        }
    }
}
