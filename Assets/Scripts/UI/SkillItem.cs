using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* Menu items in Skills Menu */
public class SkillItem : MonoBehaviour
{
    public Text skillName;
    public Image tierOne;
    public Image tierTwo;
    public Image tierThree;

    void Awake() 
    {
        skillName = transform.Find("Skill_text").GetComponent<Text>();

        tierOne = transform.Find("1").GetComponent<Image>();
        tierTwo = transform.Find("2").GetComponent<Image>();
        tierThree = transform.Find("3").GetComponent<Image>();

        tierOne.gameObject.SetActive(true);
        tierTwo.gameObject.SetActive(true);
        tierThree.gameObject.SetActive(true);

        tierOne.color = Color.clear;
        tierTwo.color = Color.clear;
        tierThree.color = Color.clear;
    }

    void Start()
    { 
    }

    public void UpdateSkillTier(string skillname, int skilltier)
    {
        Color32 c1 = new Color32(0,191,255,255);
        Color32 c2 = new Color32(0,115,255,255);

        skillName.text = skillname;

        if(skilltier == 1){
            tierOne.color = Color.white;
            tierTwo.color = Color.clear;
            tierThree.color = Color.clear;
        }
        else if(skilltier == 2){
            tierOne.color = Color.white;
            tierTwo.color = c1;
            tierThree.color = Color.clear;
        }
        else if(skilltier == 3){
            tierOne.color = Color.white;
            tierTwo.color = c1;
            tierThree.color = c2;
        }
        else{
            tierOne.color = Color.clear;
            tierTwo.color = Color.clear;
            tierThree.color = Color.clear;     
        }
    }
}
