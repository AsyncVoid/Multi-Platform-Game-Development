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
    private FastNoiseLite fastNoiseLite = null;
    private Tilemap tileMap;
    private Transform trackingTransform;

    private void OnEnable()
    {
        fastNoiseLite = new FastNoiseLite(seed);
        fastNoiseLite.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        fastNoiseLite.SetFrequency(frequency);
        fastNoiseLite.SetFractalType(FastNoiseLite.FractalType.FBm);
        fastNoiseLite.SetFractalOctaves(5);
        fastNoiseLite.SetFractalLacunarity(2f);
        fastNoiseLite.SetFractalGain(0.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        tileMap = floorComponent.GetComponent<Tilemap>();
        trackingTransform = trackingObject.GetComponent<Transform>();
        for (int x = -screenTileWidth; x <= screenTileWidth; x++)
            GenerateColumn(x);
    }

    void GenerateColumn(int x)
    {
        int height = getHeight(x);
        for (int y = min; y <= max; y++)
        {
            if (y < height)
                tileMap.SetTile(new Vector3Int(x, y, 0), groundM);
            else if (y == height)
                tileMap.SetTile(new Vector3Int(x, y, 0), groundT);
            else
                tileMap.SetTile(new Vector3Int(x, y, 0), null);
        }
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
        float noise = fastNoiseLite.GetNoise(x, 0f); //-1 <-> 1
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
