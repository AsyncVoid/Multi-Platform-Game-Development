using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Slider hpSlider;
	private Player player;

	void Start() {
		player = GameObject.FindWithTag("Player").GetComponent<Player>();

		hpSlider.maxValue = player.ReturnMaxHP();
		hpSlider.value = player.ReturnHP();
	}

	void FixedUpdate() {
		hpSlider.maxValue = player.ReturnMaxHP();
		hpSlider.value = player.ReturnHP();
	}
}