using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityHolder : MonoBehaviour
{
    [SerializeField] Ability[] abilityList;
    float coolDownTime;
    float activeTime;

    enum AbilityState{
        ready,
        active,
        cooldown
    }
    AbilityState state = AbilityState.ready;
    PlayerMovement pm;

    [SerializeField] KeyCode key;

    private void Start() {
        pm=GetComponent<PlayerMovement>();
        foreach(Ability ability in abilityList) {
            ability.CollectAbility(gameObject);
        }
    }

    private void Update() {
        switch (state) {
            case AbilityState.ready:
                if(Input.GetKeyDown(key)) {
                    if(abilityList[0].maxSpeed!=0) {
                        pm.SetUseAbility(true,true);
                    }
                    else{pm.SetUseAbility(true,false);}
                    abilityList[0].Activate();
                    state = AbilityState.active;
                    activeTime = abilityList[0].activeTime;
                }
            break;
            case AbilityState.active:
                if(activeTime>0) {
                    activeTime -= Time.deltaTime;
                }
                else {
                    state =AbilityState.cooldown;
                    pm.SetUseAbility(false,false);
                    coolDownTime=abilityList[0].coolDownTime;
                }
            break;
            case AbilityState.cooldown:
                if(coolDownTime>0) {
                    coolDownTime -= Time.deltaTime;
                }
                else {
                    state =AbilityState.ready;
                }
            break;
        }
        
    }
}
