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
        if (transform.position == target1.position)
        {
            currentTarget = target2;
        }
        if (transform.position == target2.position)
        {
            currentTarget = target1;
        }


        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
    }

}
