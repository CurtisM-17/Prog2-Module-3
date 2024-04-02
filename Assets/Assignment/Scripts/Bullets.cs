using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
	public bool damagesTowers = true; // By default
	public float speed = 1f;
	public float hitDeleteDelay = 0.25f;

	public float regularTowerDmg = 1f;
	public float mainTowerDmg = 0.5f;

	Rigidbody2D rb;

	private void Start() {
		Destroy(gameObject, 5);

		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {
		rb.MovePosition(speed * Time.deltaTime * (Vector2)transform.up + rb.position);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (damagesTowers && collision.gameObject.layer != 3) return; // Tower mode
		else if (!damagesTowers && !collision.gameObject.CompareTag("Enemy")) return; // Enemy mode

		Destroy(gameObject, hitDeleteDelay);

		if (collision.CompareTag("MainTower")) {
			collision.gameObject.GetComponent<Towers>().IncrementHealth(-mainTowerDmg);

			return;
		};

		Towers tower = collision.gameObject.GetComponent<TowerSelections>().towerInPlace;
		if (!tower) return; // Empty plot

		tower.IncrementHealth(-regularTowerDmg);
	}
}
