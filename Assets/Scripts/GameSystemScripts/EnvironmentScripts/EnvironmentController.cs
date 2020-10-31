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
    private GameObject CombatSystem;
    private GameObject Player;

    private void OnEnable()
    {
        InitialMovementSpeed = 1f;
        MovementSpeed = InitialMovementSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        CombatScene = false;
        CombatSystem = GameObject.FindWithTag("CombatSystem");
        Player = GameObject.FindWithTag("Player");

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
    public void TriggerCombatScene(GameObject enemy) {
        CombatScene = true;
        MovementSpeed = 0f;

        CombatSystem combat = CombatSystem.GetComponent<CombatSystem>();
        combat.StartBattle(Player.GetComponent<Player>(), enemy.GetComponent<Enemy>());

        foreach (TileMovement tileMovement in Object.FindObjectsOfType<TileMovement>()) {
            tileMovement.HaltTileMovement();
        }

        foreach (EntityController entityController in Object.FindObjectsOfType<EntityController>())
        {
            entityController.HaltEntityMovement();
            if (entityController.tag != "Enemy") { 
                entityController.DisableCollisions();
            }
        }

    }

    public void EndCombatScene() {
        CombatScene = false;
        MovementSpeed = InitialMovementSpeed;


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
