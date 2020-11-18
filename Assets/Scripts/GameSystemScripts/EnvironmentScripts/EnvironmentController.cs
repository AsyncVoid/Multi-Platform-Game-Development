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

    public bool GetIsCombatScene() {
        return CombatScene;
    }

}
