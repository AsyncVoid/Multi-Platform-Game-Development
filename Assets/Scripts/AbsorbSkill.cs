using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbSkill : MonoBehaviour
{
    public GameObject player;
    public GameObject entity; // Player can inherit skills from enemies, environments.. 
    private Player playerUnit;
    private Skill skill;

    void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerUnit = player.GetComponent<Player>();
    }

    void OnCollisionEnter(Collision other)
    {
        // If player collides with dead enemy & is in liquid state 
        if(other.gameObject.tag == "Enemy" && player.GetComponent<PlayerController>().ReturnState()){
            entity = other.gameObject.gameObject;
            if(entity.GetComponent<Unit>().ReturnDeathState()){ 
                skill = entity.GetComponent<Enemy>().ReturnSkill();
                playerUnit.AddSkill(skill); 
                GameObject.Destroy(entity); 
                //Debug.Log (player.name + "absorbs skill: " + skill.skillName);
            }
            // If enemy is alive -> Trigger combat
            else{

            }

        }
    }

}