using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
	public enum ChestType { Villager, Merchant, Archer }
    public ChestType chestType;

	public Animator animator;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Villager>(out Villager v)) {
            if (chestType != ChestType.Villager && v.CanOpen() != chestType) return; // Must match chest type

			animator.SetBool("IsOpened", true);
		}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("IsOpened", false);
    }
}
