using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public Transform[] pathPoints;
	public GameObject enemyPrefab;

	public float minSpawnInterval, maxSpawnInterval;
	float currentInterval;

	private void Start() {
		EnemyBehaviour.pathPoints = pathPoints;
		EnemyBehaviour.towers = GameObject.FindGameObjectsWithTag("Tower");

		StartCoroutine(SpawnEnemyWhenTime()); // Spawn instantly
	}

	IEnumerator SpawnEnemyWhenTime() {
		yield return new WaitForSeconds(currentInterval);

		// Spawn enemy
		Instantiate(enemyPrefab);

		// New interval & coroutine
		NewInterval();
	}

	void NewInterval() {
		currentInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

		StartCoroutine(SpawnEnemyWhenTime());
	}
}
