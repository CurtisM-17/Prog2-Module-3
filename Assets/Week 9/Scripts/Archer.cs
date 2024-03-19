using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Villager
{
	public GameObject arrow;
	public Transform spawnpoint;

	public override void Attack() {
		base.Attack();
		destination = transform.position; // Stop moving

		Instantiate(arrow, spawnpoint.position, spawnpoint.rotation);
	}

	public override Chest.ChestType CanOpen() {
		return Chest.ChestType.Archer;
	}
}
