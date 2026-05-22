using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the object that touched the spike is tagged "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Grab the Movement script off the player
            Movement playerMovement = collision.GetComponent<Movement>();
            
            if (playerMovement != null)
            {
                // Tell the player to run its public Die() method!
                playerMovement.Die();
            }
        }
    }
}