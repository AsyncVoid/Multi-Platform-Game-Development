using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotkeyMenu : MonoBehaviour
{
    private GameObject[] dropdownObjects;
    private SkillDropdown[] skillDropdowns; // Array of dropdowns which contains list of player skills (Options)
    private MenuController menuController;

    void Start()
    {
        dropdownObjects = GameObject.FindGameObjectsWithTag("Dropdown");
        skillDropdowns = new SkillDropdown[dropdownObjects.Length];

        for(int i = 0;i<dropdownObjects.Length;i++){
            skillDropdowns[i] = dropdownObjects[i].GetComponent<SkillDropdown>();
        }
    }

}


