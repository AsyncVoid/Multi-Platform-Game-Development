using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{

    private float InitialMovementSpeed;
    private float MovementSpeed;
    public GameObject FloorTile;
    private List<GameObject> InitialFloorTiles;

    private bool CombatScene;
    // Start is called before the first frame update
    void Start()
    {
        CombatScene = false;
        InitialMovementSpeed = 0.8f;
        MovementSpeed = InitialMovementSpeed;

        // Create initial floor tiles.
        foreach (int value in Enumerable.Range(-10, 24)) {
            Instantiate(FloorTile, new Vector3((float) value + 0.5f, -1.5f, 0f), Quaternion.identity);
        }

        // Give them movement speed to allow them to scroll.
        foreach (GameObject floorTile in GameObject.FindGameObjectsWithTag("Floor")) {
            floorTile.GetComponent<TileMovement>().SetSpeed(MovementSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Returns movement speed.
    public float GetMovementSpeed() {
        return MovementSpeed;
    }

    public void TriggerCombatScene() {
        CombatScene = true;
        MovementSpeed = 0f;

        foreach (GameObject floorTile in GameObject.FindGameObjectsWithTag("Floor")) {
            floorTile.GetComponent<TileMovement>().HaltTileMovement();
        }
    }

    public void EndCombatScene() {
        CombatScene = false;
        MovementSpeed = InitialMovementSpeed;

        foreach (GameObject floorTile in GameObject.FindGameObjectsWithTag("Floor"))
        {
            floorTile.GetComponent<TileMovement>().RestoreTileMovement();
        }
    }

    public bool GetIsCombatScene() {
        return CombatScene;
    }

}
