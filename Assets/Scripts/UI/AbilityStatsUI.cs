using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityStatsUI : MonoBehaviour
{
    [SerializeField] GameObject abilityPrefab;
    PlayerAbilityHolder playerHolder;
    AbilityUI[] abilityUi;
    int numChildren=0;

    public void AddStat(PlayerAbilityHolder holder)
    {
        Debug.Log("AbilityStatsUI");
        if (playerHolder == null)
        {
            playerHolder = holder;
        }
        CreateAbilityUI();

    }

    public void IncreaseChildren() {
        numChildren++;
    }

    void CreateAbilityUI() {
        Instantiate(abilityPrefab, this.gameObject.transform);
        
    }

    public PlayerAbilityHolder GetHolder() {
        return playerHolder;
    }

    public int GetNumChildren() {
        return numChildren;
    }

}
