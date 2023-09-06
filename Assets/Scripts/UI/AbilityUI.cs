using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] TMP_Text abilityName;
    [SerializeField] TMP_Text abilityKey;
    PlayerAbilityHolder playerHolder;
    AbilityStatsUI abilityStatsUI;

    // Start is called before the first frame update
    void Start()
    {
        abilityStatsUI=GetComponentInParent<AbilityStatsUI>();
        playerHolder=abilityStatsUI.GetHolder(); 
        if(abilityStatsUI.GetNumChildren()<=playerHolder.NumOfAbilities()){
            
            abilityName.SetText(playerHolder.GetAbilityInList(abilityStatsUI.GetNumChildren()).abilityName.ToString());
        
            abilityKey.SetText(playerHolder.GetKeyInList(abilityStatsUI.GetNumChildren()).ToString());
            abilityStatsUI.IncreaseChildren();
        }
        else{
            Destroy(this.gameObject);
        }
    }


}
