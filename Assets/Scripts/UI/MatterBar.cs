using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatterBar : MonoBehaviour
{
	public Slider matterSlider;
	private Player player;

	void Start()
	{
		player = GameObject.FindWithTag("Player").GetComponent<Player>();

		matterSlider.maxValue = player.ReturnMaxMatter();
		matterSlider.value = player.ReturnMatter();
	}

	void FixedUpdate()
	{
		matterSlider.maxValue = player.ReturnMaxMatter();
		matterSlider.value = player.ReturnMatter();
	}
}