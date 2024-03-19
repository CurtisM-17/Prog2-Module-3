using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : Villager
{
	public override Chest.ChestType CanOpen() {
		return Chest.ChestType.Merchant;
	}
}
