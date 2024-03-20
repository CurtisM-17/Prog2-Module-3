using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public TMPro.TextMeshProUGUI currentSelection;
    public static Villager SelectedVillager { get; private set; }

    public Villager[] villagers;

    public static void SetSelectedVillager(Villager villager)
    {
        if(SelectedVillager != null)
        {
            SelectedVillager.Selected(false);
        }
        SelectedVillager = villager;
        SelectedVillager.Selected(true);
    }

    public void SelectVillagerFromDropdown(int index) {

        if (index == 0 && SelectedVillager != null) {
            // De-select
            SelectedVillager.Selected(false);
            SelectedVillager = null;
            return;
        }

        SetSelectedVillager(villagers[index-1]);
    }

    private void Update()
    {
        if(SelectedVillager != null)
        {
            currentSelection.text = SelectedVillager.GetType().ToString();
        }
    }
}
