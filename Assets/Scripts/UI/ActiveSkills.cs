using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* UI to display active skills chosen by the player */
public class ActiveSkills : MonoBehaviour
{
    private SkillSlot[] skillSlots;
    private GameObject[] skillSlotObj;
    private Player player;
    private MenuController menuController;
    private SkillList playerSkills;
    private HotkeyMenu hotkeyMenu;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerSkills = player.skills;
        menuController = GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>();

        skillSlotObj = GameObject.FindGameObjectsWithTag("SkillSlot");
        skillSlots = new SkillSlot[skillSlotObj.Length];

        for(int i = 0; i<skillSlotObj.Length;i++){
            skillSlots[i] = skillSlotObj[i].GetComponent<SkillSlot>();
            //Debug.Log(i);
        }

    }


    /* Set skill icons for active skills */
    public void SetActiveSkills(string hotkey, Skill skill)
    {
        if(menuController.isPaused){
            for(int i = 0;i<skillSlots.Length;i++){
                string key = skillSlots[i].hotkey.text;
                if(key == hotkey){
                    skillSlots[i].skillIcon.sprite = skill.skillSprite;
                }
            }

        }

    }


}
