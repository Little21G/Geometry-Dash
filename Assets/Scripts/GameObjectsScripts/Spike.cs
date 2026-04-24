using UnityEngine;
using UnityEngine.SceneManagement; // We need this to reload the level!

public class Spike : MonoBehaviour
{
    // OnTriggerEnter2D runs the exact moment another 2D collider enters this object's space.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // We check if the object that hit us has the "Movement" script. 
        // If it does, we know for a fact it's the player!
        Movement player = collision.GetComponent<Movement>();
        
        if (player != null)
        {
            Die();
        }
    }

    void Die()
    {
        // 1. Tell our AttemptManager that we died so it adds 1 to the counter!
        AttemptManager.RegisterDeath();
        
        // 2. Reload the scene (Classic GD restart)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}