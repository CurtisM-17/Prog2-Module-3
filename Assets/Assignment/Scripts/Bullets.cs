using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
	public bool damagesTowers = true; // By default
	public float speed = 1f;
	public float hitDeleteDelay = 0.25f;

	Rigidbody2D rb;

	private void Start() {
		Destroy(gameObject, 5);

		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {
		rb.MovePosition(speed * Time.deltaTime * (Vector2)transform.up + rb.position);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (damagesTowers && !collision.gameObject.CompareTag("Tower")) return; // Tower mode
		else if (!damagesTowers && !collision.gameObject.CompareTag("Enemy")) return; // Enemy mode

		Destroy(gameObject, hitDeleteDelay);
	}
}
