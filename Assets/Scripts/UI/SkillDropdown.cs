using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* SkillDropdown is assigned to each Dropdown object */

public class SkillDropdown : MonoBehaviour
{
    private Dropdown skillDropdown;
    private Player player;
    private SkillList playerSkills;
    private Skill chosenSkill; 
    private string hotkey;
    private MenuController menuController;
    private ActiveSkills activeSkills;

    void Start()
    {
        menuController = GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        activeSkills = GameObject.FindGameObjectWithTag("ActiveSkills").GetComponent<ActiveSkills>();
        playerSkills = player.skills;

        skillDropdown = GetComponent<Dropdown>();
        skillDropdown.options.Clear(); 

        // Placeholder label
        skillDropdown.captionText = transform.Find("Label").GetComponent<Text>();
        string label = "None";
        skillDropdown.options.Add(new Dropdown.OptionData() {text=label});

        // Add skills from player's skillList to all skill dropdowns. 
        for(int i = 0; i < playerSkills.skills.Count; i++){
            skillDropdown.options.Add(new Dropdown.OptionData() {text=playerSkills.skills[i].skillName, image=playerSkills.skills[i].skillSprite});
        }

        // To handle when a new value is selected from the dropdown
        skillDropdown.onValueChanged.AddListener(delegate {
            OnValueChanged(skillDropdown);
        });

        skillDropdown.RefreshShownValue();
    }

    void OnValueChanged(Dropdown dropdown)
    {
        string skillname = dropdown.captionText.text;        
        dropdown.RefreshShownValue();

        Text text = transform.Find("Text").GetComponent<Text>();
        this.hotkey = text.text;
        this.chosenSkill = playerSkills.skills.Find(Skill => Skill.skillName == skillname);
        
        activeSkills.SetActiveSkills(hotkey, chosenSkill);
        
        // Change hotkey values in dictionary 
        Dictionary<string, Skill> dict = menuController.hotkeyDict;
        if(dict.ContainsKey(hotkey)){
            dict[hotkey] = chosenSkill;
        }
        else{
            dict.Add(hotkey, chosenSkill);
        }

    }

}