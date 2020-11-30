using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillList", fileName = "SkillList.asset")]
[System.Serializable]

public class SkillList : ScriptableObject
{
    public List<Skill> skills;

    public void addSkill(Skill skill) {
        if (skills.Contains(skill)) {
            skill.IncrementSkillProgression();
        }
        else {
            skills.Add(skill);
            skill.IncrementSkillProgression();
        }
    }
}
