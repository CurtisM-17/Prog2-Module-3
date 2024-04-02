using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelections : MonoBehaviour
{
	/// <summary>
	/// Each plot has this script; for selecting/deselecting/handling purchases of each plot
	/// </summary>

	public Color hover;
	Color standard;

	public float hoverSizeMultiplier = 1.1f;
	Vector3 standardSize;

	SpriteRenderer sr;

	[System.NonSerialized] public Towers towerInPlace;

	TowerUpgrades purchaseSystem;

	private void Start() {
		sr = GetComponent<SpriteRenderer>();

		standard = sr.color;
		standardSize = transform.localScale;

		purchaseSystem = GameObject.FindGameObjectWithTag("TowerSystem").GetComponent<TowerUpgrades>();
	}

	/// Mouse Events
	private void OnMouseEnter() {
		sr.color = hover;
		transform.localScale = standardSize * hoverSizeMultiplier;
	}

	private void OnMouseExit() {
		if (TowerUpgrades.selectedTower == this) return;

		sr.color = standard;
		transform.localScale = standardSize;
	}

	private void OnMouseDown() {
		purchaseSystem.SelectTower(this);
	}

	public void DeselectTower() {
		sr.color = standard;
		transform.localScale = standardSize;
	}
}
