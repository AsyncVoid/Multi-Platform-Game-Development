using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Skill skill;
    public void UpdateSkill(Skill _skill){
        skill = _skill;
    }
}
