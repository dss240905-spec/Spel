using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform target1, target2;
    [SerializeField] private float moveSpeed = 2.0f;

    private Transform currentTarget;
    void Start()
    {
        currentTarget = target1;
    }

    void FixedUpdate()
   
    {

        transform.position = Vector2.MoveTowards(
             transform.position,
             currentTarget.position,
             moveSpeed * Time.deltaTime
         );

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.05f)
        {
            currentTarget = (currentTarget == target1) ? target2 : target1;
        }
    }

}
