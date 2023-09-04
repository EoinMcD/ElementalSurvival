using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    [Header("Base Ability")]
    public string abilityName;
    public AbilityType abilityType;
    public float coolDownTime;
    public float useAbilityCoolDownTime;
    public float activeTime;
    public float maxSpeed;
    public bool stopInput;

    [System.NonSerialized] public float maxAbilityCoolDownTimer;
    protected PlayerMovement pm;
    protected PlayerStats pStats;
    protected Rigidbody rb;
    protected Transform orientation;

    public virtual void Activate(){}

    public void CollectAbility(GameObject parent){
        pm= parent.GetComponent<PlayerMovement>();
        pStats= parent.GetComponent<PlayerStats>();
        rb = parent.GetComponent<Rigidbody>();
        orientation = pm.orientation;
        maxAbilityCoolDownTimer = useAbilityCoolDownTime;
    }

    
}

public enum AbilityType{
    air,
    water,
    fire,
    earth
}
