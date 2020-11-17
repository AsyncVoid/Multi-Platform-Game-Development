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
                // Ensures items on the floor do not interfere with combat.
                entityController.DisableCollisions();
            }
        }

    }

    // Method to reenable scrolling and ending of combat scene.
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
