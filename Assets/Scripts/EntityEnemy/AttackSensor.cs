using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour
{

    private bool attackHit;
    // Start is called before the first frame update
    void Start()
    {
        attackHit = false;
    }

    public bool AttackConfirm()
    {
        return attackHit;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            attackHit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            attackHit = false;
        }
    }
}
