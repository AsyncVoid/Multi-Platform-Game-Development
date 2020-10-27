
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int maxHP;
    public int currentHP { get; private set; }
    public Stat damage;
    public bool isDead;

    void Awake ()
    {
        currentHP = maxHP;
    }

    public void TakeDamage (int damage)
    {
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        currentHP -= damage;
        if(currentHP <= 0)
        {
            isDead = true;
        }
    }

    public void SetMaxHP (int maxHP)
    {
        this.maxHP = maxHP;
    }

    public bool ReturnDeathState() {
        return isDead;
    }



}
