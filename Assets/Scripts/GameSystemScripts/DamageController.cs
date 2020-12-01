using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    private Player player;

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
        enemy.TakeDamage(skill.skillTierBase());
    }

    public void DamagePlayer(Enemy enemy)
    {
        player.TakeDamage(enemy.ReturnDmg());
    }
}
