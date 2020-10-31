using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    private Skill enemySkill;
    void Awake()
    {
        // Filler skill
        enemySkill = new Skill("Fire", SkillType.Magical);
    }

    public Skill ReturnSkill()
    {
        return enemySkill;
    }


}