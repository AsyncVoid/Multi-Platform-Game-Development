using System.Collections;
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

    public void DamageEnemy(Skill skill, Enemy enemy)
    {

        int calcNum = skill.skillTierBase();
        enemy.TakeDamage(calcNum);
        DamageIndicator dmg = Instantiate(dmgIndicator, enemy.transform.position, Quaternion.identity);
        dmg.GetComponent<DamageIndicator>().SetText(calcNum.ToString());
    }

    public void DamagePlayer(Enemy enemy)
    {
        int calcDmg = enemy.ReturnDmg();
        player.TakeDamage(calcDmg);
        DamageIndicator dmg = Instantiate(dmgIndicator, player.transform.position, Quaternion.identity);
        dmg.GetComponent<DamageIndicator>().SetText(calcDmg.ToString());
    }
}
