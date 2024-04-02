using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportTower : Towers {
    public float healRate = 75f;

	protected override bool Attack() {
        // Heal all towers instead of attacking
        foreach (TowerSelections towerSelection in EnemyBehaviour.towers)
        {
            if (towerSelection.towerInPlace == null) continue;
            if (towerSelection.towerInPlace.health <= 0) continue;
            if (towerSelection.towerInPlace.gameObject == gameObject) continue;

            towerSelection.towerInPlace.health += healRate;
        }

        return true;
	}
}
