using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public Skill enemySkill;

    public Skill ReturnSkill()
    {
        return enemySkill;
    }


}