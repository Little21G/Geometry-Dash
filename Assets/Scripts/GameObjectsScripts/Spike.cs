using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player touches the spike trigger, reload the level
        if (collision.gameObject.CompareTag("Player"))
        {
            AttemptManager.RegisterDeath();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}