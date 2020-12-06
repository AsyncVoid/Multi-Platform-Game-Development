using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillSlot : MonoBehaviour
{
    public Text hotkey;
    public Image skillIcon;

    private Skill skill {get; set;}

    void Start()
    {
        /* Child Components:
        - Icon (Image)
        - Hotkey (Text)
        */
        skillIcon = transform.Find("Icon").GetComponent<Image>();
        hotkey = transform.Find("Hotkey").GetComponent<Text>();

        if(skillIcon.sprite == null){
            // Set empty image to clear (transparent) to remove white box. 
            skillIcon.color = Color.clear;
        }
        else{
            skillIcon.color = Color.white;
        }

    }

    void Update(){
        if(skillIcon.sprite == null){
            // Set empty image to clear (transparent) to remove white box. 
            skillIcon.color = Color.clear;
        }
        else{
            skillIcon.color = Color.white;
        } 
    }



}
