using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skills", order = 1)]
[System.Serializable]
public class Skill : ScriptableObject
{
    public string skillName;
    public int skillTier;

    public int skillProgression;

    public SkillType skillType;
    public Sprite skillSprite;
    public int skillBase;


    public GameObject skillPrefab;

    public void IncrementSkillTier()
    {
        switch (skillTier)
        {
            case 1:
                skillTier += 1;
                break;

            case 2:
                skillTier += 1;
                break;

            case 3:
                break;
        }
    }

    public void IncrementSkillProgression()
    {
        skillProgression += 1;

        if (skillProgression == 1)
        {
            skillTier = 1;
        }
        else if (skillProgression == 5)
        {
            IncrementSkillTier();
        }
        else if (skillProgression == 15)
        {
            IncrementSkillTier();
        }
    }

    public int skillTierBase()
    {
        return skillBase * skillTier;
    }

}

public enum SkillType { Magical, Physical, Recovery, Passive, Status }

