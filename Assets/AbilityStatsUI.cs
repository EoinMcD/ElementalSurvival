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

    public void AddStat(PlayerAbilityHolder holder) {
        if(playerHolder==null) {
            playerHolder=holder;
        }
        numChildren++;
        Instantiate(abilityPrefab,this.gameObject.transform);
    }

    public PlayerAbilityHolder GetHolder() {
        return playerHolder;
    }

    public int GetNumChildren() {
        return numChildren-1;
    }

}
