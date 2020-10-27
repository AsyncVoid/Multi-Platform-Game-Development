using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    [SerializeField]
    public string skillName; 
    public SkillType skillType;

    public Skill(string skillName, SkillType skillType)
    {
        this.skillName = skillName;
        this.skillType = skillType;
    }

}
public enum SkillType { Magical, Physical, Healing, Passive, Ailment }
