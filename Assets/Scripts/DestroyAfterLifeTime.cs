using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float lifetime = 1.0f;

    void Start()
    {
        Destroy(gameObject, lifetime);  
    }

    
}
