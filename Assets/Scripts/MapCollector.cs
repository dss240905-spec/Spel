using UnityEngine;

public class MapCollector : MonoBehaviour
{
    [SerializeField] private KeyCode pickupKey = KeyCode.E;

    private MapBehaviour mapNearby;

    private void Update()
    {
        if (mapNearby != null && Input.GetKeyDown(pickupKey))
        {
            mapNearby.CollectMapFromPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        MapBehaviour map = other.GetComponent<MapBehaviour>();
        if (map != null && map.IsCollectible())
        {
            mapNearby = map;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        MapBehaviour map = other.GetComponent<MapBehaviour>();
        if (map != null && mapNearby == map)
        {
            mapNearby = null;
        }
    }
}
