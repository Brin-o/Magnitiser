using UnityEngine;

public class PickupTrigger : MonoBehaviour
{
    int id;
    private void Start()
    {
        id = GetComponent<pickup>().id;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameEvents.current.CollectibePickedUp(id);
            other.GetComponent<PlayerJuice>().BumpGracePeriod();
        }
    }
}