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

    private void OnEnable()
    {
        CombatScene = false;
        InitialMovementSpeed = 0.8f;
        MovementSpeed = InitialMovementSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
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

    // Methods for combat scene initiation (Pause scrolling and background gen).
    public void TriggerCombatScene() {
        CombatScene = true;
        MovementSpeed = 0f;

        /*foreach (GameObject floorTile in GameObject.FindGameObjectsWithTag("Floor")) {
            floorTile.GetComponent<TileMovement>().HaltTileMovement();
        }*/
        foreach (TileMovement tileMovement in Object.FindObjectsOfType<TileMovement>()) {
            tileMovement.HaltTileMovement();
        }
        foreach (EntityController entityController in Object.FindObjectsOfType<EntityController>())
        {
            entityController.HaltEntityMovement();
            entityController.DisableCollisions();
        }

    }

    public void EndCombatScene() {
        CombatScene = false;
        MovementSpeed = InitialMovementSpeed;

        /*foreach (GameObject floorTile in GameObject.FindGameObjectsWithTag("Floor"))
        {
            floorTile.GetComponent<TileMovement>().RestoreTileMovement();
        }*/

        foreach (TileMovement tileMovement in Object.FindObjectsOfType<TileMovement>())
        {
            tileMovement.RestoreTileMovement();
        }
        foreach (EntityController entityController in Object.FindObjectsOfType<EntityController>())
        {
            entityController.RestoreEntityMovement();
            entityController.RestoreCollisions();
        }
    }

    public bool GetIsCombatScene() {
        return CombatScene;
    }

}
