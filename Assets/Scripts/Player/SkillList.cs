using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillList", fileName = "SkillList.asset")]
[System.Serializable]

public class SkillList : ScriptableObject
{
    public List<Skill> skills;

    // Method to be called when entity is eaten for skill unlocking / progression.
    public void addSkill(Skill skill) {

        // If skill already unlocked, increase skill progression.
        if (skills.Contains(skill)) {
            skill.IncrementSkillProgression();
        }

        // If skill not known, add as an unlocked skill and increase progression.
        else {
            skills.Add(skill);
            skill.IncrementSkillProgression();
        }
    }
}
