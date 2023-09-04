using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string abilityName;
    public AbilityType abilityType;
    public float coolDownTime;
    public float activeTime;
    public float maxSpeed;
    public bool stopInput;

    protected PlayerMovement pm;
    protected Rigidbody rb;
    protected Transform orientation;

    public virtual void Activate(){}

    public void CollectAbility(GameObject parent){
        pm= parent.GetComponent<PlayerMovement>();
        rb = parent.GetComponent<Rigidbody>();
        orientation = pm.orientation;
    }
    
}

public enum AbilityType{
    air,
    water,
    fire,
    earth
}
