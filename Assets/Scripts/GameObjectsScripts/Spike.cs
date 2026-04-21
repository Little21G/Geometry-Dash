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
        // This is the classic Geometry Dash restart. 
        // It asks Unity for the currently active level, and tells it to load it from the beginning.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}