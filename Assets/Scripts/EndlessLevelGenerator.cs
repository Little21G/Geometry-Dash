using System.Collections.Generic;
using UnityEngine;

public class EndlessLevelGenerator : MonoBehaviour
{
    [Header("Assign your objects")]
    public Transform player;
    public GameObject[] chunkPrefabs; 

    [Header("Level Settings")]
    public float chunkWidth = 20f; 
    public int chunksToPreload = 5; 

    private float spawnX = 0f;
    private List<GameObject> activeChunks = new List<GameObject>();

    void Start()
    {
        // Spawn the starting path
        for (int i = 0; i < chunksToPreload; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        // 1. SPAWN NEW CHUNKS
        if (player.position.x > spawnX - (chunksToPreload * chunkWidth))
        {
            SpawnChunk();

            // Trigger the score bonus every time a new chunk loads!
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddChunkBonus();
            }
        }

        // 2. DELETE OLD CHUNKS
        GameObject oldestChunk = activeChunks[0];
        if (player.position.x - oldestChunk.transform.position.x > (chunkWidth * 2)) 
        {
            DeleteOldChunk();
        }
    }

    void SpawnChunk()
    {
        int randomIndex = Random.Range(0, chunkPrefabs.Length);
        GameObject chunkToSpawn = chunkPrefabs[randomIndex];

        GameObject newChunk = Instantiate(chunkToSpawn, new Vector3(spawnX, 0, 0), Quaternion.identity);
        activeChunks.Add(newChunk);

        spawnX += chunkWidth;
    }

    void DeleteOldChunk()
    {
        GameObject oldChunk = activeChunks[0];
        activeChunks.RemoveAt(0);
        Destroy(oldChunk);
    }
}