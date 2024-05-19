using UnityEngine;

public class TigeInsideUsineSpawner : MonoBehaviour
{
    // The prefab to instantiate
    public GameObject prefab;

    // The number of prefabs to spawn
    public int numberOfPrefabs = 10;

    // Range for random positions
    // Range offsets for random positions relative to this GameObject's position
    public Vector2 xRangeOffset = new Vector2(-10f, 10f);
    public Vector2 yRangeOffset = new Vector2(-10f, 10f);
    public Vector2 zRangeOffset = new Vector2(-10f, 10f);

    bool once = true;

    void Start()
    {
        //SpawnPrefabs();
    }

    void SpawnPrefabs()
    {
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // Generate random position within the specified range relative to this GameObject's position
            float randomX = Random.Range(this.transform.position.x + xRangeOffset.x, this.transform.position.x + xRangeOffset.y);
            float randomY = Random.Range(this.transform.position.y + yRangeOffset.x, this.transform.position.y + yRangeOffset.y);
            float randomZ = Random.Range(this.transform.position.z + zRangeOffset.x, this.transform.position.z + zRangeOffset.y);

            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);

            // Instantiate the prefab at the random position and set it as a child of this GameObject
            Instantiate(prefab, randomPosition, Quaternion.identity, this.transform);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (once)
            {
                SpawnPrefabs();
                once = false;
            }

        }
    }
}


