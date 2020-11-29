using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int seed = 1337;
    public float frequency = 0.01f;
    public int min = 0;
    public int max = 10;
    public GameObject floorComponent;
    public GameObject trackingObject;
    public TileBase groundM;
    public TileBase groundT;
    public int screenTileWidth = 30;
    private FastNoiseLite groundNoise = null;
    private FastNoiseLite caveNoise = null;
    private FastNoiseLite biomeNoise = null;
    private Tilemap tileMap;
    private Transform trackingTransform;

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
        //biomeNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        biomeNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        biomeNoise.SetFrequency(0.005f);
        biomeNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        biomeNoise.SetFractalOctaves(1);
        biomeNoise.SetFractalLacunarity(2f);
        biomeNoise.SetFractalGain(0.5f);
        //biomeNoise.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.Hybrid);
        //biomeNoise.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
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
        /*int height = getHeight(x);
        float hP = (height + min) / max;

        for (int y = min; y <= max; y++)
        {
            float caveNoiseF = (caveNoise.GetNoise(x, y)+1f)/2f;
            float yP = (min + y) / max;
            caveNoiseF *= (float)hP/yP;
            //Debug.Log("x: " + x + " y: " + y + " caveNoise: " + caveNoiseF);
            if (y < height && caveNoiseF > 0.6f)
                tileMap.SetTile(new Vector3Int(x, y, 0), null);
            else if (y < height)
                tileMap.SetTile(new Vector3Int(x, y, 0), groundM);
            else if (y == height)
                tileMap.SetTile(new Vector3Int(x, y, 0), groundT);
            else
                tileMap.SetTile(new Vector3Int(x, y, 0), null);
        }*/

        for(int y = min; y <= max; y++)
        {
            Vector3Int pos = new Vector3Int(x, y, 0);
            BlockType blockType = getBlockType(x, y);
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
    }

    enum BlockType
    {
        TOP,
        GROUND,
        NOTHING
    }

    private BlockType getBlockType(int x, int y)
    {
        int height = getHeight(x);
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
    enum BiomeType
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

    private BiomeType GetBiomeType(int x, int y)
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
        
        int biomeId = (int)(((biomeNoise.GetNoise(x, 0) + 1f) / 2f) * biomeTypeValuesLength);
        //Debug.Log("BiomeId: " + biomeId);
        return (BiomeType)biomeTypeValues.GetValue(biomeId);
    }

    void ClearColumn(int x)
    {
        for (int y = min; y <= max; y++)
        {
             tileMap.SetTile(new Vector3Int(x, y, 0), null);
        }
    }

    int getHeight(int x)
    {
        float noise = groundNoise.GetNoise(x, 0f); //-1 <-> 1
        float normal = (noise + 1f) / 2f;  //0 <-> 1
        return (int) (normal * (max - min)) + min;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = trackingTransform.position;
        int playerX = (int)playerPos.x;
        ClearColumn(playerX + screenTileWidth + 1);
        ClearColumn(playerX - screenTileWidth - 1);
        GenerateColumn(playerX + screenTileWidth);
        GenerateColumn(playerX - screenTileWidth);
    }
}
