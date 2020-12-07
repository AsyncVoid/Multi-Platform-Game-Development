using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = System.Object;

public class MapGenerator : MonoBehaviour
{
    public int seed = 1337;
    [Range(0, 1)]
    public float frequency = 0.01f;
    public int min = 0;
    public int max = 10;
    public GameObject floorComponent;
    public GameObject trackingObject;
    public TileBase groundM;
    public TileBase groundT;
    public int screenTileWidth = 30;
    [Range(0, 1)]
    public float enemyChance = 0.5f;
    public int enemyEvery = 16;
    public GameObject[] enemies = new GameObject[0];
    [Range(0, 1)]
    public float treasureChance = 0.5f;
    public int treasureEvery = 16;
    public GameObject[] treasures = new GameObject[0];
    public Dictionary<float, float> test;
    private FastNoiseLite groundNoise = null;
    private FastNoiseLite caveNoise = null;
    private FastNoiseLite biomeNoise = null;
    private FastNoiseLite treasureNoise = null;
    private FastNoiseLite enemyNoise = null;
    private FastNoiseLite decorationNoise = null;
    private Tilemap tileMap;
    private Transform trackingTransform;
    
    public GameObject tree1;
    public GameObject tree2;
    public GameObject bush1;
    public GameObject bush2;
    public GameObject bush3;
    public GameObject rock1;
    public GameObject rock2;
    public GameObject rock3;
    public GameObject gravestone1;
    public GameObject sunflower1;
    public GameObject sunflower2;

    private void OnEnable()
    {
        groundNoise = new FastNoiseLite(seed);
        groundNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        groundNoise.SetFrequency(frequency);
        groundNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        groundNoise.SetFractalOctaves(5);
        groundNoise.SetFractalLacunarity(2f);
        groundNoise.SetFractalGain(0.5f);
        caveNoise = new FastNoiseLite(~seed);
        caveNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        caveNoise.SetFrequency(0.1f);//frequency / 128f);
        caveNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        caveNoise.SetFractalOctaves(5);
        caveNoise.SetFractalLacunarity(2f);
        caveNoise.SetFractalGain(0.5f);
        biomeNoise = new FastNoiseLite(~seed - seed);
        biomeNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        biomeNoise.SetFrequency(0.005f);
        biomeNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        biomeNoise.SetFractalOctaves(1);
        biomeNoise.SetFractalLacunarity(2f);
        biomeNoise.SetFractalGain(0.5f);
        //Could also have the biomes be cellular noise
        //biomeNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        //biomeNoise.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.Hybrid);
        //biomeNoise.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
        treasureNoise = new FastNoiseLite(seed);
        treasureNoise.SetNoiseType(FastNoiseLite.NoiseType.Value);
        treasureNoise.SetFractalType(FastNoiseLite.FractalType.None);
        treasureNoise.SetFrequency(0.2f);
        treasureNoise.SetFractalOctaves(2);
        treasureNoise.SetFractalLacunarity(2f);
        treasureNoise.SetFractalGain(0.5f);
        enemyNoise = new FastNoiseLite(~seed);
        enemyNoise.SetNoiseType(FastNoiseLite.NoiseType.Value);
        enemyNoise.SetFractalType(FastNoiseLite.FractalType.None);
        enemyNoise.SetFrequency(0.2f);
        enemyNoise.SetFractalOctaves(2);
        enemyNoise.SetFractalLacunarity(2f);
        enemyNoise.SetFractalGain(0.5f);
        decorationNoise = new FastNoiseLite(seed + 64);
        decorationNoise.SetNoiseType(FastNoiseLite.NoiseType.Value);
        decorationNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        decorationNoise.SetFrequency(0.25f);
        decorationNoise.SetFractalOctaves(3);
        decorationNoise.SetFractalLacunarity(2f);
        decorationNoise.SetFractalGain(0.5f);
    }

    void Awake() {
        seed = UnityEngine.Random.Range(0, 99999);
    }
    // Start is called before the first frame update
    void Start()
    {
        tileMap = floorComponent.GetComponent<Tilemap>();
        trackingTransform = trackingObject.GetComponent<Transform>();
        int xPos = (int)trackingTransform.position.x;
        for (int x = xPos - screenTileWidth; x <= xPos + screenTileWidth; x++)
            GenerateColumn(x);
    }

    void GenerateColumn(int x)
    {
        int height = getHeight(x);
        for(int y = min; y <= max; y++)
        {
            Vector3Int pos = new Vector3Int(x, y, 0);
            BlockType blockType = getBlockType(height, x, y);
            switch(blockType)
            {
                case BlockType.NOTHING:
                    tileMap.SetTile(pos, null);
                    break;
                case BlockType.TOP:
                    tileMap.SetTile(pos, groundT);
                    break;
                case BlockType.GROUND:
                    tileMap.SetTile(pos, groundM);
                    break;
            }
            BiomeType biomeType = GetBiomeType(x, y);
            tileMap.SetTileFlags(pos, ~TileFlags.LockColor);
            switch (biomeType)
            {
                case BiomeType.PLAINS:
                    //No color tint
                    break;
                case BiomeType.SWAMP:
                    //tileMap.SetColor(pos, new Color(0.376f, 0.502f, 0.22f, 1f));
                    tileMap.SetColor(pos, new Color(0.752f, 1f, 0.44f));
                    break;
                case BiomeType.TAIGA:
                    tileMap.SetColor(pos, new Color(0.137f, 0.5450f, 0.137f, 1f));
                    break;
                case BiomeType.DESERT:
                    tileMap.SetColor(pos, new Color(0.7568f, 0.60392f, 0.4196f));
                    break;
                /*case BiomeType.ICE:
                    //1 0.98 0.98
                    tileMap.SetColor(pos, new Color(2f, 2f, 2f));
                    break;*/
                case BiomeType.CAVE:
                    tileMap.SetColor(pos, new Color(0.5f, 0.5f, 0.5f));
                    break;
            }
            tileMap.SetTileFlags(pos, TileFlags.LockColor);
        }
        doTreasure(x, height + 1);
        doEnemySpawn(x, height + 1);
        doDecoration(x, height);
    }

    public enum BlockType
    {
        TOP,
        GROUND,
        NOTHING
    }

    private BlockType getBlockType(int height, int x, int y)
    {
        float hP = (height + min) / max;
        float caveNoiseF = (caveNoise.GetNoise(x, y) + 1f) / 2f;
        float yP = (min + y) / max;
        caveNoiseF *= (float)hP / yP;
        //Debug.Log("x: " + x + " y: " + y + " caveNoise: " + caveNoiseF);
        if (y < height - 1 && caveNoiseF > 0.6f)
            return BlockType.NOTHING;
        else if (y < height)
            return BlockType.GROUND;
        else if (y == height)
            return BlockType.TOP;
        else
            return BlockType.NOTHING;
    }

    //Put the biomes in order from coldest to hottest
    //then reserve some at the end to stop them being surface biomes
    public enum BiomeType
    {
        TAIGA,
        PLAINS,
        SWAMP,
        DESERT,
        //ICE,
        CAVE
    }

    private static readonly Array biomeTypeValues = Enum.GetValues(typeof(BiomeType));
    private static readonly int biomeTypeValuesLength = biomeTypeValues.Length - 1; //-1 to remove caves

    public BiomeType GetBiomeType(int x, int y)
    {
        int height = getHeight(x);
        if (y < height - 3)
            return BiomeType.CAVE;
        /*int hash = (int) (biomeNoise.GetNoise(x, y) * 2147483648.0f);
        hash = (hash >> 4) & 0b11111111;
        Debug.Log("Hash: " + hash);
        Array values = Enum.GetValues(typeof(BiomeType));
        int biomeId = hash % (values.Length - 1);
        //Debug.Log("BiomeId: " + biomeId);
        return (BiomeType)values.GetValue(biomeId);*/
        
        int biomeId = (int)Math.Floor(((biomeNoise.GetNoise(x, 0) + 1f) / 2f) * biomeTypeValuesLength);
        //Debug.Log("BiomeId: " + biomeId);
        return (BiomeType)biomeTypeValues.GetValue(biomeId);
    }

    void ClearColumn(int x)
    {
        for (int y = min; y <= max; y++)
        {
             tileMap.SetTile(new Vector3Int(x, y, 0), null);
        }
        
        string[] removeTags = {"Treasure", "Decoration", "Enemy"};
        foreach(string tag in removeTags)
        {
            GameObject[] items = GameObject.FindGameObjectsWithTag(tag);
            foreach (var tI in items)
            {
                Vector3 position = tI.transform.position;
                if((int)position.x == x) {
                    Destroy(tI);
                }
            }
        }
    }

    int getHeight(int x)
    {
        float noise = groundNoise.GetNoise(x, 0f); //-1 <-> 1
        float normal = (noise + 1f) / 2f;  //0 <-> 1
        return (int) (normal * (max - min)) + min;
    }

    void doTreasure(int x, int y) {
        bool doTreasure = treasures.Length > 0 && x % treasureEvery == 0;
        if(doTreasure)
        {
            int scope = (int) (treasures.Length / treasureChance);
            float normalised = ((treasureNoise.GetNoise(x, y) + 1f) / 2f); //0-1
            int treasureResult = (int)(normalised * scope);
            if(treasureResult < 0 || treasureResult >= treasures.Length)
                return;
            int treasureId = treasureResult;
            GameObject treasureItem = treasures[treasureId];
            if(treasureItem != null)
                Instantiate(treasureItem, new Vector3(x+0.5f, y+0.5f, 0), Quaternion.identity);
        }
    }
    
    void doEnemySpawn(int x, int y)
    {
        if(enemies.Length > 0 && (x+6) % enemyEvery == 0) //offset x
        {
            int scope = (int) (enemies.Length / enemyChance);
            float normalised = ((enemyNoise.GetNoise(x, y) + 1f) / 2f); //0-1
            float chanced = normalised * normalised;
            int enemyResult = (int)Math.Floor(chanced * scope);
            if(enemyResult < 0 || enemyResult >= enemies.Length)
                return;
            int enemyId = enemyResult;
            GameObject enemy = enemies[enemyId];
            if(enemy != null)
                Instantiate(enemy, new Vector3(x+0.5f, y+1.5f, 0), Quaternion.identity);
        }
    }

    void doDecoration(int x, int y)
    {
        float noise = decorationNoise.GetNoise(x, y);
        int test = (int) (noise * 100) % 3;
        if (noise > 0 && test != 2)
        {
            BiomeType biomeType =  GetBiomeType(x, y);
            int index = (int) (noise * 20);
            switch (biomeType)
            {
                case BiomeType.PLAINS:
                    switch (index)
                    {
                        case 0:
                            Instantiate(rock1, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity);
                            break;
                        case 1:
                            Instantiate(bush1, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity);
                            break;
                        case 2:
                            Instantiate(rock2, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity);
                            break;
                        case 3:
                            Instantiate(bush2, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity);
                            break;
                        case 4:
                            Instantiate(rock3, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity);
                            break;
                    }
                    break;
                case BiomeType.TAIGA:
                    switch (index)
                    {
                        case 0:
                        case 1: 
                        case 2:
                        case 3:
                        case 4:
                            break;
                    }
                    break;
                case BiomeType.SWAMP:
                    switch (index)
                    {
                        case 0:
                            Instantiate(bush1, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity);
                            break;
                        case 1: 
                            Instantiate(tree1, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity);
                            break;
                        case 2:
                            Instantiate(tree2, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity); 
                            break;
                        case 3:
                            Instantiate(bush2, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity);
                            break;
                        case 4:
                            Instantiate(bush3, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity);
                            break;
                    }
                    break;
                case BiomeType.DESERT:
                    switch (index)
                    {
                        case 0:
                            Instantiate(gravestone1, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity);
                            break;
                        case 1:
                            Instantiate(sunflower1, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity);
                            break;
                        case 2:
                            Instantiate(sunflower2, new Vector3(x+0.5f, y+1f, 0.5f), Quaternion.identity);
                            break;
                    }
                    break;
            }
            
            //tileMap.SetTile(new Vector3Int(x, y, -1), tree1);
        }
    }

    private int lastX = 0;
    private float lastRemove = 0;

    // Update is called once per frame
    void Update()
    {
        lastRemove += Time.deltaTime;
        Vector3 playerPos = trackingTransform.position;
        int playerX = (int) playerPos.x;
        if (playerX != lastX)
        {
            ClearColumn(playerX + screenTileWidth + 1);
            ClearColumn(playerX - screenTileWidth - 1);
            GenerateColumn(playerX + screenTileWidth);
            GenerateColumn(playerX - screenTileWidth);
        }

        lastX = playerX;
        if (lastRemove > 2f) //every 2s
        {
            removeFallen();
            lastRemove = 0;
        }
    }

    void removeFallen()
    {
        string[] removeTags = {"Treasure", "Decoration", "Enemy"};
        foreach(string tag in removeTags)
        {
            GameObject[] items = GameObject.FindGameObjectsWithTag(tag);
            foreach (var tI in items)
            {
                Vector3 position = tI.GetComponent<Transform>().position;
                if(position.y < min - 10) {
                    Destroy(tI);
                }
            }
        }
    }
}
