using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	/// <summary>
	/// Enemy spawning system
	/// Only one instance runs
	/// </summary>

	public Transform[] pathPoints;
	public GameObject enemyPrefab;

	public static List<EnemyBehaviour> enemies = new();

	public float minSpawnInterval, maxSpawnInterval;
	float currentInterval;

	GameObject enemyContainer;

	private void Start() {
		EnemyBehaviour.pathPoints = pathPoints;
		EnemyBehaviour.mainTower = GameObject.FindGameObjectWithTag("MainTower");

		// Collect selection scripts
		List<TowerSelections> selectionScripts = new();
		foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
			selectionScripts.Add(tower.GetComponent<TowerSelections>());
        }

		EnemyBehaviour.towers = selectionScripts.ToArray();

		enemyContainer = GameObject.FindGameObjectWithTag("EnemyContainer");

		// Spawn the first enemy instantly
        StartCoroutine(SpawnEnemyWhenTime());
	}

	static void LogEnemy(GameObject enemy) {
		enemies.Add(enemy.GetComponent<EnemyBehaviour>());
	}

	IEnumerator SpawnEnemyWhenTime() {
		yield return new WaitForSeconds(currentInterval);

		// Spawn enemy
		GameObject enemy = Instantiate(enemyPrefab, enemyContainer.transform);
		if (enemy != null) LogEnemy(enemy);

		// New interval & coroutine
		NewInterval();
	}


	void NewInterval() {
		currentInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

		StartCoroutine(SpawnEnemyWhenTime());
	}
}
