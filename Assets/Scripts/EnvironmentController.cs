using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{

    private float MovementSpeed;
    public GameObject FloorTile;
    private List<GameObject> InitialFloorTiles;

    private bool CombatScene = false;
    // Start is called before the first frame update
    void Start()
    {
        MovementSpeed = 0.8f;
        foreach (int value in Enumerable.Range(-10, 24)) {
            Instantiate(FloorTile, new Vector3((float) value + 0.5f, -1.5f, 0f), Quaternion.identity);
        }

        foreach (GameObject floorTile in GameObject.FindGameObjectsWithTag("Floor")) {
            floorTile.GetComponent<TileMovement>().SetSpeed(MovementSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetMovementSpeed() {
        return MovementSpeed;
    }
}
