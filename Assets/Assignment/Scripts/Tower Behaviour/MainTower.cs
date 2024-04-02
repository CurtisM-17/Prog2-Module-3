using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainTower : Towers
{
	public Slider mainTowerHealthBar;

	private void Start() {
		Setup(gameObject);
	}

	public override void Setup(GameObject _) {
		// Don't create a slider
		healthSlider = mainTowerHealthBar;

		health = maxHealth;
		healthSlider.maxValue = maxHealth;
		healthSlider.value = health;
	}

	protected override void Die() {
		SceneManager.LoadScene(1);
	}
}
