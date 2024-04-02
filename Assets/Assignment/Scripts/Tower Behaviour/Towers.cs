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

	public TowerUpgrades.TowerTypes towerType = TowerUpgrades.TowerTypes.Main;

	public int maxHealth = 100;
	[System.NonSerialized] public float health;

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

	/// DEFENCE FUNCTIONALITY
	/// Attacking nearest enemies by default
	public float damage, fireRate, attackRange;
	float timer, lastShot;
	[System.NonSerialized] public GameObject bulletPrefab; // Set by TowerUpgrades
	GameObject bulletContainer;

	private void Start() {
		bulletContainer = GameObject.FindGameObjectWithTag("BulletContainer");
	}

	private void Update() {
		if (towerType == TowerUpgrades.TowerTypes.Main) return;

		timer += Time.deltaTime;

		if (timer - lastShot >= fireRate) {
			bool executed = Attack();

			if (executed) lastShot = timer;
		}
	}

	protected virtual bool Attack() {
		// Default behaviour is choosing the nearest enemy and shooting it
		EnemyBehaviour closestEnemy = null;
		float closestDistance = 0;

        foreach (EnemyBehaviour enemy in EnemySpawner.enemies)
        {
			if (enemy.health <= 0) continue;

			Vector2 diff =  enemy.gameObject.transform.position - transform.position;
			float dist = diff.magnitude;

			if (dist > attackRange) continue; // Too far away

			// Target the closest enemy
			if (closestEnemy == null) {
				closestEnemy = enemy;
				closestDistance = dist;
				continue;
			}

			if (dist < closestDistance) {
				closestEnemy = enemy;
				closestDistance = dist;
			}
        }

		if (!closestEnemy) return false;

		// Attack closest
		GameObject bullet = Instantiate(bulletPrefab, bulletContainer.transform);

		bullet.GetComponent<Bullets>().enemyDamage = damage;

		// Shoot slightly in front of the enemy in the direction of their movement
		float forwardIncrease = 0.25f;
		if (closestDistance > 5) forwardIncrease *= 2; // Account for longer travel time

		Vector3 forwardPos = closestEnemy.gameObject.transform.position + (closestEnemy.transform.up * forwardIncrease);
		Vector2 enemyPosDiff = forwardPos - transform.position;

		// Calculate angle and shoot
		float angle = (Mathf.Atan2(enemyPosDiff.y, enemyPosDiff.x) * Mathf.Rad2Deg);

		bullet.transform.position = transform.position;
		bullet.transform.eulerAngles = new Vector3(0, 0, angle - 90);

		return true;
    }


}
