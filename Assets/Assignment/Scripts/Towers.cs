using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Towers : MonoBehaviour
{
	/// <summary>
	/// Base class for towers
	/// Handles attacking/lifecycle/etc.
	/// </summary>

	public int maxHealth = 100;
	protected float health;

	protected Slider healthSlider;

	public virtual void Setup(GameObject healthBarPrefab) {
		GameObject healthCanvas = Instantiate(healthBarPrefab, transform);
		healthSlider = healthCanvas.transform.Find("HealthBar").GetComponent<Slider>();

		health = maxHealth;
		healthSlider.maxValue = maxHealth;
		healthSlider.value = health;
	}

	/// Health
	public void IncrementHealth(float increment) {
		health += increment;
		if (health < 0) {
			health = 0;
			Die();
		} else if (health > maxHealth) health = maxHealth;

		healthSlider.value = health;
	}

	protected virtual void Die() {
		Destroy(gameObject);
	}

	public virtual string GetTowerType() {
		return "None";
	}

	protected virtual void ShootEnemy() {

	}
}
