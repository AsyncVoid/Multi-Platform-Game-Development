
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillsMenu : MonoBehaviour
{
    private Player player;
    private SkillList playerSkills;
    private SkillItem[] skillItems;
    private GameObject[] itemObj;
    private Scrollbar scrollbar; 
    private ScrollRect scrollRect;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerSkills = player.skills;   

        itemObj = GameObject.FindGameObjectsWithTag("SkillItem");
        skillItems = new SkillItem[itemObj.Length];

        scrollbar = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();
        scrollRect = GameObject.Find("ScrollArea").GetComponent<ScrollRect>();

        UpdateMenu();
    }
    
    void UpdateMenu(){

        for(int i = 0;i<skillItems.Length;i++){
            skillItems[i] = itemObj[i].GetComponent<SkillItem>();
        }

        if (playerSkills.skills.Count>6)
            enableScroll(true);
        else 
            enableScroll(false);

        for(int i = 0; i < playerSkills.skills.Count; i++){
            string skillName = playerSkills.skills[i].skillName;
            int skillTier = playerSkills.skills[i].skillTier;
            skillItems[i].UpdateSkillTier(skillName, skillTier);
        }    
    }

    void Update()
    {
        UpdateMenu();
    }

    void enableScroll(bool requireScroll)
    {
        if(requireScroll){
            scrollbar.gameObject.SetActive(true);
            scrollRect.enabled = true;
        }
        else{
            scrollbar.gameObject.SetActive(false);
            scrollRect.enabled = false;
        }

    }
}