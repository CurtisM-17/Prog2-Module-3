using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDrillTower : Towers {
	public int cashIncrement = 3;

	protected override bool Attack() {
		// Generate money instead of attacking
		TowerUpgrades.IncrementCash(Mathf.FloorToInt(cashIncrement));

		return true;
	}
}
