﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    private Player player;
    public DamageIndicator dmgIndicator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method to be used if skill hits an enemy.
    public void DamageEnemy(Skill skill, Enemy enemy)
    {
        if (enemy == null)
            return;
        int calcNum = skill.skillTierBase();
        enemy.TakeDamage(calcNum);

        // Damage indicator.
        DamageIndicator dmg = Instantiate(dmgIndicator, enemy.transform.position, Quaternion.identity);
        dmg.GetComponent<DamageIndicator>().SetText(calcNum.ToString());
    }

    // Method to be used if skill hits player.
    public void DamagePlayer(Enemy enemy)
    {
        int calcDmg = enemy.ReturnDmg();
        player.TakeDamage(calcDmg);

        DamageIndicator dmg = Instantiate(dmgIndicator, player.transform.position, Quaternion.identity);
        dmg.GetComponent<DamageIndicator>().SetText(calcDmg.ToString());
    }

    public void HealEnemy(Enemy enemy) {
        int calcNum = enemy.ReturnDmg();
        enemy.Heal(calcNum);

        // Damage indicator.
        DamageIndicator dmg = Instantiate(dmgIndicator, enemy.transform.position, Quaternion.identity);
        dmg.GetComponent<DamageIndicator>().SetText(calcNum.ToString());
        dmg.GetComponent<DamageIndicator>().HealColour();
    }

    public void HealPlayer(Skill skill) {
        int calcNum = skill.skillTierBase();
        player.Heal(calcNum);

        DamageIndicator dmg = Instantiate(dmgIndicator, player.transform.position, Quaternion.identity);
        dmg.GetComponent<DamageIndicator>().SetText(calcNum.ToString());
        dmg.GetComponent<DamageIndicator>().HealColour();
    }
}
