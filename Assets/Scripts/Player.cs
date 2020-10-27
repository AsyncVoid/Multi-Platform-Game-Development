using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit

{   
    public List<Skill> allSkills = new List<Skill>();  // All of the skills the player can have

    private int numSlots; // No. of active skills the player can have during combat?
    private List<Skill> activeSkills;

    void Start()
    {
        numSlots = 3;
        activeSkills = new List<Skill>(numSlots);
    }

    public void AddSkill (Skill skill)
    {
        allSkills.Add(skill);
    }


}
