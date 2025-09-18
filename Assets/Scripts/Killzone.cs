using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Killzone : MonoBehaviour
{
[SerializeField] private Transform spawnPosition;
    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.CompareTag("Player"))
        {

            other.transform.position = spawnPosition.position;
            other.GetComponent<Rigidbody2D>().linearVelocity = UnityEngine.Vector2.zero;
     }
  
    }

}
