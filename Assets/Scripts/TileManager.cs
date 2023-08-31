using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public GameObject[] tilePrefabs;
    public float zSpawn = 0;
    public float tileLength = 30;
    public int numberOfTiles = 5;
    private List<GameObject> activeTiles = new List<GameObject>();

    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i == 0)
                spawnTile(0);
            else
                spawnTile(Random.Range(0, tilePrefabs.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.z - 35 > zSpawn - numberOfTiles * tileLength)
        {
            spawnTile(Random.Range(0, tilePrefabs.Length));
            deleteTile();
        }
    }

    public void spawnTile(int tileIndex)
    {
        GameObject gameObject = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(gameObject);
        zSpawn += tileLength;
    }

    private void deleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

}
