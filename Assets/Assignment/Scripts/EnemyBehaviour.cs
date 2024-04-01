using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyBehaviour : MonoBehaviour
{
	public int health = 100;
	public Slider healthSlider;
	float timer = 0;

	public static GameObject[] towers; // Gets set by enemy spawner
	public static Transform[] pathPoints; // Set by enemy spawner
	int currentGoalPoint = 1;

	public float moveSpeed = 1f;
	bool reachedEnd = false;

	Rigidbody2D rb;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();

		transform.position = pathPoints[0].transform.position;
	}

	Vector2 direction;

	private void Update() {
		timer += Time.deltaTime;

		if (health <= 0 || reachedEnd) {
			direction = Vector2.zero;
		} else CalculateDirection();

		if (health > 0) AttackNearestTower(); // Can still fire if reached end
	}

	/// Attacking
	public float fireRate = 0.5f;
	public GameObject bulletPrefab;
	float lastBulletFire;

	void AttackNearestTower() {
		GameObject nearestTower = null;
		float nearestDist = 0;

		if (!reachedEnd) {
			// Determine which tower is closest to this enemy
			foreach (GameObject tower in towers) {
				float dist = (tower.transform.position - transform.position).magnitude;

				if (nearestTower == null) {
					nearestTower = tower;
					nearestDist = dist;
					continue;
				}

				float distDifference = Mathf.Abs(dist - nearestDist);

				//if (distDifference < 1) Debug.Log(distDifference + " | " + Random.Range(0, 2));

				if (dist < nearestDist) {
					if (distDifference <= 0.5f && Random.Range(0, 2) == 0) continue; // Choose between one randomly if the difference is minimal

					nearestTower = tower;
					nearestDist = dist;
				}
			}
		} else nearestTower = towers.Last(); // Only the main tower if at the end

		// Target acquired. Attack it
		BulletFire(nearestTower);
    }

	/// Bullet firing
	void BulletFire(GameObject target) {
		if (timer < 1) return; // Too early

		if (timer - lastBulletFire >= fireRate) {
			lastBulletFire = timer;

			Vector3 aimDir = target.transform.position - transform.position;
			float angle = (Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg);

			Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle-90));
		}
	}

	/// Movement
	void CalculateDirection() {
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
	public void IncrementHealth(int increment) {
		health = increment;
		if (health < 0) {
			health = 0;
			Die();
		}

		healthSlider.value = health;
	}

	void Die() {
		Destroy(gameObject, 1);
	}
}
