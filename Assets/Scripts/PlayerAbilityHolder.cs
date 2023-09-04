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

    [SerializeField] KeyCode[] abilityKeys;
    [SerializeField] ArrayList abilitiesOnCooldown = new ArrayList();

    private void Start() {
        pm=GetComponent<PlayerMovement>();
        foreach(Ability ability in abilityList) {
            ability.CollectAbility(gameObject);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.G)) {
            for(int i=0 ;i< abilitiesOnCooldown.Count;i++) {
                Debug.Log(abilitiesOnCooldown[i]);
            }
        }
        switch (state) {
            case AbilityState.ready:
                for(int i=0 ;i< abilityKeys.Length;i++) {
                    Debug.Log(abilityList[i].useAbilityCoolDownTime);
                    if(Input.GetKeyDown(abilityKeys[i]) && abilityList[i].useAbilityCoolDownTime ==abilityList[i].maxAbilityCoolDownTimer ) {
                        Debug.Log("USING ABILITY " + abilityKeys[i]);
                        if(abilityList[i].maxSpeed!=0) {
                            pm.SetUseAbility(true,true);
                        }
                        else{pm.SetUseAbility(true,false);}
                        abilityList[i].Activate();
                        AbilityCoolDown(abilityList[i]);
                        state = AbilityState.active;
                        activeTime = abilityList[i].activeTime;
                    }
                    
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

    void AbilityCoolDown(Ability ability){
        abilitiesOnCooldown.Add(ability);
        StopCoroutine("AbilityCoolDownCoRoutine");
        StartCoroutine("AbilityCoolDownCoRoutine");
    }   

    IEnumerator AbilityCoolDownCoRoutine() {
        while(abilitiesOnCooldown.Count!=0) {
            foreach(Ability ability in abilitiesOnCooldown.ToArray()) {
                ability.maxAbilityCoolDownTimer-=1;
                Debug.Log(ability.maxAbilityCoolDownTimer);
                if(ability.maxAbilityCoolDownTimer<=0) {
                    ability.maxAbilityCoolDownTimer=ability.useAbilityCoolDownTime;
                    abilitiesOnCooldown.Remove(ability);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}
