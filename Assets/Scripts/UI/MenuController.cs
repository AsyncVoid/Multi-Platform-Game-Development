using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private SkillsMenu SkillsMenu;
    private HotkeyMenu HotkeyMenu;
    private ActiveSkills activeSkills;
    public bool isPaused;
    private PlayerController playerController;
    public Dictionary<string, Skill> hotkeyDict {get; set;} // To keep track of the hotkey and Skill


    void Start()
    {
        hotkeyDict = new Dictionary<string, Skill>();

        for (int i = 2; i <= 4; i++) {
            hotkeyDict.Add(i.ToString(), null);
        }

        isPaused = false;

        SkillsMenu = GameObject.FindGameObjectWithTag("SkillsMenu").GetComponent<SkillsMenu>();
        HotkeyMenu = GameObject.FindGameObjectWithTag("HotkeyMenu").GetComponent<HotkeyMenu>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        SkillsMenu.gameObject.SetActive(false);
        HotkeyMenu.gameObject.SetActive(false);

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)){
            if(!SkillsMenu.gameObject.activeSelf && !HotkeyMenu.gameObject.activeSelf){
                PauseGame();
                SkillsMenu.gameObject.SetActive(true);
                HotkeyMenu.gameObject.SetActive(true);
                
                
            }
                
            else{
                SkillsMenu.gameObject.SetActive(false);
                HotkeyMenu.gameObject.SetActive(false);
                ContinueGame();
            }
        }
        
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        playerController.enabled = false;
        //Disable scripts that still work while timescale is set to 0
    } 

    void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        playerController.enabled = true;
        //enable the scripts again
    }
}