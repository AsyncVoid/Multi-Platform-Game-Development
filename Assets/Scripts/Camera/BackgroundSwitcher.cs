using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{
    public GameObject plainsBackground;
    public GameObject taigaBackground;
    public GameObject player;
    private Transform playerTransform;
    private MapGenerator _mapGenerator;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        _mapGenerator = Object.FindObjectOfType<MapGenerator>();
    }

    private float timeSinceChange = 0;

    private MapGenerator.BiomeType lastBiome;
    // Update is called once per frame
    void Update()
    {
        timeSinceChange += Time.deltaTime;
        if (timeSinceChange > 1f) //1s 
        {
            Vector3 pos = playerTransform.position;
            MapGenerator.BiomeType biome =
                _mapGenerator.GetBiomeType((int)pos.x, (int)pos.y);
            if (biome != lastBiome)
            {
                SetBackground(biome);
                timeSinceChange = 0;
            }
            lastBiome = biome;
        }
    }

    void SetBackground(MapGenerator.BiomeType biomeType)
    {
        switch (biomeType)
        {
            case MapGenerator.BiomeType.PLAINS:
            case MapGenerator.BiomeType.DESERT:
                plainsBackground.SetActive(true);
                taigaBackground.SetActive(false);
                break;
            case MapGenerator.BiomeType.SWAMP:
            case MapGenerator.BiomeType.TAIGA:
                taigaBackground.SetActive(true);
                plainsBackground.SetActive(false);
                break;
        }
    }
}
