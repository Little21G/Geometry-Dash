using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    // This runs automatically whenever the player physically hits something solid
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Make sure we are hitting the blocks and not something else
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Get the exact point and angle where the player touched the block
            ContactPoint2D contact = collision.GetContact(0);

            // If the normal's X is negative, the block is pushing us to the LEFT. 
            // This means we crashed head-on into the wall!
            if (contact.normal.x < -0.5f)
            {
                Die();
            }
            // If the normal's Y is positive, the block is pushing us UP.
            // This means we landed safely on the top!
            else if (contact.normal.y > 0.5f)
            {
                Debug.Log("Landed safely!");
                // You can put your jump reset code here!
            }
        }
    }

    private void Die()
    {
        // Restart the level immediately
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}