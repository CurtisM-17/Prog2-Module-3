using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerUpgrades : MonoBehaviour
{
	/// <summary>
	/// Upgrade menu for towers & main system
	/// Only one instance runs
	/// </summary>

	public enum TowerTypes {
		Sniper, Turret, Support, OilDrill, Main
	}

	/// Use static function for fetching upgrade/tower info
	public GameObject purchaseMenu;
	public GameObject[] towerObjects;
	public GameObject healthBarPrefab;

	public static TowerSelections selectedTower;

	TextMeshProUGUI descriptor;
	private void Start() {
		descriptor = purchaseMenu.transform.Find("Descriptor").gameObject.GetComponent<TextMeshProUGUI>();
	}

	public void SelectTower(TowerSelections tower) {
		if (selectedTower != null) DeselectTower();
		selectedTower = tower;

		purchaseMenu.SetActive(true);

		bool occupied = tower.towerInPlace != null; // True if occupied

		purchaseMenu.transform.Find("Purchase").gameObject.SetActive(!occupied);
		purchaseMenu.transform.Find("Upgrade").gameObject.SetActive(occupied);

		if (occupied) descriptor.text = "REINFORCE";
		else descriptor.text = "CONSTRUCT";
	}

	public void DeselectTower() {
		selectedTower.DeselectTower();
		purchaseMenu.SetActive(false);

		selectedTower = null;
	}

	public void SelectTowerPurchase(int purchaseIndex) {
		purchaseMenu.SetActive(false);

		GameObject towerPrefab = towerObjects[purchaseIndex];

		GameObject towerObject = Instantiate(towerPrefab, selectedTower.transform.position, towerPrefab.transform.rotation);
		towerObject.transform.Translate(0, 0.75f, 0);

		selectedTower.towerInPlace = towerObject.GetComponent<Towers>(); // Keep track of the tower script in the selection script & mark as occupied

		selectedTower.towerInPlace.Setup(healthBarPrefab);

		DeselectTower();
	}
}
