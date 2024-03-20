using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderClock : MonoBehaviour
{
	Slider slider;
	float timer;
	public float speed = 1f;

	private void Start()
	{
		slider = GetComponent<Slider>();
	}

	private void Update()
	{
		timer += Time.deltaTime * speed;
		timer %= 60;

		slider.value = timer;
	}
}
