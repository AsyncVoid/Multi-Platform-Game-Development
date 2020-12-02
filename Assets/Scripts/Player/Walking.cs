using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : StateMachineBehaviour
{

    private GameObject player;
    private GameObject playerModel;

    // Keeps the player controller gameobject with the player model.
    // Makes sure the collider stays with the animation.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player");
        playerModel = GameObject.FindWithTag("PlayerModel");

        // player.transform.position = playerModel.transform.position;

        player.transform.position += playerModel.transform.localPosition;
        playerModel.transform.localPosition -= playerModel.transform.localPosition;
    }
}
