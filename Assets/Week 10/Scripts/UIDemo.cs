using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDemo : MonoBehaviour
{
	public TMP_Dropdown dropdown;

	SpriteRenderer sr;
	public Color start, end;
	float lerp;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	public void SliderValueHasChanged(Single value)
	{
		lerp = value;
	}

	public void DropdownHasChanged(int index)
	{
		Debug.Log(dropdown.options[index].text);
	}

	private void Update()
	{
		sr.color = Color.Lerp(start, end, lerp/60);
	}
}
