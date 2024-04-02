using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyBehaviour : MonoBehaviour
{
	public float health = 100;
	public Slider healthSlider;
	float timer = 0;

	public static TowerSelections[] towers; // Gets set by enemy spawner
	public static Transform[] pathPoints; // Set by enemy spawner
	public static GameObject mainTower; // ^^^
	int currentGoalPoint = 1;

	public float moveSpeed = 1f;
	bool reachedEnd = false;

	Rigidbody2D rb;
	GameObject bulletContainer;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();

		transform.position = pathPoints[0].transform.position;

		bulletContainer = GameObject.FindGameObjectWithTag("BulletContainer");
	}

	Vector2 direction;

	private void Update() {
		timer += Time.deltaTime;

		if (health <= 0 || reachedEnd) direction = Vector2.zero;

		if (health > 0) {
			// Can still fire if reached end
			AttackNearestTower();
			CalculateDirection();
		}
	}

	/// Attacking
	public float fireRate = 0.5f;
	public GameObject bulletPrefab;
	float lastBulletFire;

	public float maxAttackDistance = 3f;

	void AttackNearestTower() {
		if (timer < 1) return; // Too early

		if (timer - lastBulletFire >= fireRate) {
			GameObject nearestTower = null;
			float nearestDist = 0;

			if (!reachedEnd) {
				// Determine which tower is closest to this enemy
				foreach (TowerSelections towerSelection in towers) {
					if (towerSelection.towerInPlace == null) continue;

					GameObject tower = towerSelection.gameObject;

					float dist = (tower.transform.position - transform.position).magnitude;

					if (nearestTower == null) {
						nearestTower = tower;
						nearestDist = dist;
						continue;
					}

					float distDifference = Mathf.Abs(dist - nearestDist);

					if (dist < nearestDist) {
						if (distDifference <= 0.5f && Random.Range(0, 2) == 0) continue; // Choose between one randomly if the difference is minimal

						nearestTower = tower;
						nearestDist = dist;
					}
				}
			} else {
				nearestTower = mainTower; // Only the main tower if at the end
			}

			if (!nearestTower) nearestTower = mainTower; // Target is the main tower if there are no defences

			// Don't attack if too far away
			Vector3 difference = nearestTower.transform.position - transform.position;

			if (difference.magnitude > maxAttackDistance) return; // Too far

			// Attack
			lastBulletFire = timer;

			GameObject bullet = Instantiate(bulletPrefab, bulletContainer.transform);

			float angle = (Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg);

			bullet.transform.position = transform.position;
			bullet.transform.eulerAngles = new Vector3(0, 0, angle - 90);
		}
	}

	/// Movement
	void CalculateDirection() {
		if (reachedEnd) {
			rb.rotation = 0;
			return;
		}

		direction = pathPoints[currentGoalPoint].position - transform.position;

		// Face point
		float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
		rb.rotation = angle - 90;
	}

	private void FixedUpdate() {
		if (reachedEnd || health <= 0) return;

		if (Vector2.Distance(transform.position, pathPoints[currentGoalPoint].position) <= 0.05f) { // Close to goal
			transform.position = pathPoints[currentGoalPoint].position;
			CalculateDirection(); // Re-calculate

			if (currentGoalPoint == pathPoints.Length - 1) {
				reachedEnd = true;
			} else {
				currentGoalPoint++;
			}
		}


		rb.MovePosition(rb.position + direction.normalized * Time.deltaTime * moveSpeed); // Move along path
	}

	/// Health
	public void IncrementHealth(float increment) {
		health += increment;
		if (health <= 5) {
			health = 0;
			Die();
		}

		healthSlider.value = health;
	}

	void Die() {
		EnemySpawner.enemies.Remove(this);

		Destroy(gameObject, 1);
	}
}
