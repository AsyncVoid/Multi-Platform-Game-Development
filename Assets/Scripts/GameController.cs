using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private GameObject player;
    public Text playerStateText;
    public Text playerCoolDownText;
    public Text playerSkillText;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.GetComponent<PlayerController>().ReturnState())
        {
            playerStateText.text = "State: Liquid";
        }
        else {
            playerStateText.text = "State: Normal";
        }

        if (player.GetComponent<PlayerController>().ReturnCooldown())
        {
            playerCoolDownText.text = "Cooldown Inactive";
        }
        else {
            playerCoolDownText.text = "Cooldown Active";
        }
        
        if (player.GetComponent<Player>().skills.Count != 0){
            List<Skill> currentSkills = player.GetComponent<Player>().skills;
            string skills = "";
            for(int i = 0;i<currentSkills.Count; i++){
                skills += currentSkills[i].skillName + " - " + currentSkills[i].skillType + "\n";
            }
            playerSkillText.text = skills;
        }
        else{
            playerSkillText.text = "No skills acquired.";
        }

    }
}