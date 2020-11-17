using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Slider hpSlider;

	// Sets the Healthbar UI with the unit's health values.
	public void SetHUD(Unit unit)
	{
		hpSlider.maxValue = unit.maxHP;
		hpSlider.value = unit.ReturnHP();
	}

	public void SetHP(int hp)
	{
		hpSlider.value = hp;
	}
}