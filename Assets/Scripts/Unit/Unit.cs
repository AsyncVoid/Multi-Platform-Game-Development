using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public string unitName;
	public int damage;
	public int maxHP;
	private int currentHP;
	public bool isDead;

	void Awake()
	{
		currentHP = maxHP;
		isDead = false;
	}

	// Unit health removal function.
	public bool TakeDamage(int dmg)
	{
		currentHP -= dmg;

		if (currentHP <= 0)
		{
			isDead = true;
			return true;
		}
		else
			return false;
	}

	public void IncreaseMaxHealth(int healthIncrease) {
		maxHP += healthIncrease;
		Heal(healthIncrease);
	}

	public void Heal(int health) {
		currentHP += health;

		if (currentHP > maxHP) {
			currentHP = maxHP;
		}
	}

	public int ReturnHP()
	{
		return currentHP;
	}

	public bool ReturnDeathStatus() {
		return isDead;
	}

	public int ReturnDmg()
	{
		return damage;
	}

	public int ReturnMaxHP()
	{
		return maxHP;
	}

	public void SetDmg(int dmg) {
		damage = dmg;
	}
}