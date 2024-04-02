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

	public static int playerCash = 10;

	[Header("Prices")]
	public int sniperPrice;
	public int turretPrice;
	public int supportPrice;
	public int oilDrillPrice;

	[Header("Object References")]
	public GameObject purchaseMenu;
	public GameObject[] towerObjects;
	public GameObject healthBarPrefab;
	public GameObject bulletPrefab;

	public static TowerSelections selectedTower;
	static TextMeshProUGUI cashDisplay;
	static TextMeshProUGUI descriptor;
	static TextMeshProUGUI scoreDisplay;

	float playerScore = 0;

	private void Start() {
		descriptor = purchaseMenu.transform.Find("Descriptor").gameObject.GetComponent<TextMeshProUGUI>();
		cashDisplay = GameObject.FindGameObjectWithTag("MoneyDisplay").GetComponent<TextMeshProUGUI>();
		scoreDisplay = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();

		MatchCashDisplay();
	}

	private void Update() {
		playerScore += (Time.deltaTime * 0.5f) * 5;

		scoreDisplay.text = "Score: " + Mathf.FloorToInt(playerScore).ToString();
	}

	bool purchaseMenuOpen = true; // Upgrade menu if false, purchase menu if true

	public void SelectTower(TowerSelections tower) {
		if (selectedTower != null) DeselectTower();
		selectedTower = tower;

		purchaseMenu.SetActive(true);

		purchaseMenuOpen = tower.towerInPlace == null; // True if occupied

		purchaseMenu.transform.Find("Purchase").gameObject.SetActive(purchaseMenuOpen);
		purchaseMenu.transform.Find("Destroy").gameObject.SetActive(!purchaseMenuOpen);

		if (!purchaseMenuOpen) descriptor.text = "";
		else ResetDescriptorText();
	}

	static void ResetDescriptorText() {
		descriptor.text = "CONSTRUCT";
	}

	public void DeselectTower() {
		selectedTower.DeselectTower();
		purchaseMenu.SetActive(false);

		selectedTower = null;
	}

	public void SelectTowerPurchase(int purchaseIndex) {
		int price = GetTowerPrice(purchaseIndex);
		if (!IncrementCash(-price)) return; // Can't afford

		purchaseMenu.SetActive(false);

		GameObject towerPrefab = towerObjects[purchaseIndex];

		GameObject towerObject = Instantiate(towerPrefab, selectedTower.gameObject.transform); // Put inside plot
		towerObject.transform.Translate(new Vector3(0, 0.75f, 0));

		selectedTower.towerInPlace = towerObject.GetComponent<Towers>(); // Keep track of the tower script in the selection script & mark as occupied

		selectedTower.towerInPlace.bulletPrefab = bulletPrefab;
		selectedTower.towerInPlace.Setup(healthBarPrefab);

		DeselectTower();
	}

	public void DestroyTower() { // Clicked the menu button
		if (selectedTower.towerInPlace == null) return;
		purchaseMenu.SetActive(false);

		// Refund 50% of tower price if its health is above 50% of max health
		if (selectedTower.towerInPlace.health >= selectedTower.towerInPlace.maxHealth / 2.0) {
			// Get tower type enum index, get price, divide by 2, round up, convert to int
			IncrementCash(Mathf.CeilToInt(GetTowerPrice((int)selectedTower.towerInPlace.towerType)/2f));
		}

		// Remove tower
		Destroy(selectedTower.towerInPlace.gameObject);
		DeselectTower();
	}

	/// Menu button effects
	public void HoverOverButton(int purchaseIndex) {
		switch (purchaseIndex) {
			case 0:
				descriptor.text = "SNIPER: Low rate of fire <color=green>($" + sniperPrice.ToString() + ")</color>";
				break;
			case 1:
				descriptor.text = "TURRET: High rate of fire, slightly reduced damage rates <color=green>($" + turretPrice.ToString() + ")</color>";
				break;
			case 2:
				descriptor.text = "SUPPORT: Heals all other purchased towers at an interval but does not defend <color=green>($" + supportPrice.ToString() + ")</color>";
				break;
			case 3:
				descriptor.text = "OIL DRILL: Generates 2x money but does not defend <color=green>($" + oilDrillPrice.ToString() + ")</color>";
				break;
			case 4:
				descriptor.text = "If this tower still has over half its health, you will be refunded 50% of its purchase price.";
				break;
			default:
				ResetDescriptorText();
				break;
		}
	}

	int GetTowerPrice(int towerIndex) {
		switch(towerIndex) {
			case 0:
				return sniperPrice;
			case 1:
				return turretPrice;
			case 2:
				return supportPrice;
			case 3:
				return oilDrillPrice;
			default:
				return 0;
		}
	}

	public void StopButtonHover() {
		ResetDescriptorText();
	}

	/// Player cash
	static void MatchCashDisplay() {
		cashDisplay.text = " $" + playerCash.ToString();
	}

	public static bool IncrementCash(int increment) {
		if (playerCash + increment < 0) return false;

		playerCash = Mathf.Clamp(playerCash + increment, 0, 20); // Put a limit on how much cash can be held at once

		MatchCashDisplay();

		return true;
	}
}
