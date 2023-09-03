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

    [SerializeField] KeyCode key;

    private void Update() {
        switch (state) {
            case AbilityState.ready:
                if(Input.GetKeyDown(key)) {
                    abilityList[0].Activate(gameObject);
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
