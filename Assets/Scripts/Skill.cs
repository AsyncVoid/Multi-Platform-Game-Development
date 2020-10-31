using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Skill", menuName = "Skills", order = 1)]
[System.Serializable]
public class Skill : ScriptableObject
{
    public string skillName; 
    public SkillType skillType;
    public Sprite skillSprite;
    public int skillDamage;
}
public enum SkillType { Magical, Physical, Healing, Passive, Ailment, Recovery }
