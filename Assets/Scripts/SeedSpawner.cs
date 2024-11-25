using UnityEngine;
using System.Collections.Generic;

public class SeedSpawner : MonoBehaviour
{
    public List<GameObject> prefabs;
    public float spawnInterval = 10f;
    public int maxSeeds = 5;

    // Definir el área de spawn manualmente
    public Vector2 spawnAreaSize = new Vector2(5f, 5f); // Ancho y alto del área
    public Vector2 spawnAreaCenter = new Vector2(0f, 0f); // Centro del área (relativo al Spawner)

    private List<GameObject> activeSeeds = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating(nameof(SpawnSeed), spawnInterval, spawnInterval);
    }

    private void SpawnSeed()
    {
        if (activeSeeds.Count >= maxSeeds) return;

        GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Count)];
        Vector3 spawnPosition = GetRandomPositionInArea();

        GameObject newSeed = Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
        activeSeeds.Add(newSeed);

        Destroy(newSeed, 30f);
    }

    private Vector3 GetRandomPositionInArea()
    {
        // Genera una posición aleatoria dentro del área definida
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);

        return new Vector3(
            spawnAreaCenter.x + randomX + transform.position.x,
            spawnAreaCenter.y + randomY + transform.position.y,
            transform.position.z
        );
    }
}
