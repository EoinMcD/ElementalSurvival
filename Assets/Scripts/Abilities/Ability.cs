using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
To add a new ability :
Create new script a_NAMEOFABILITY : Ability
If adding new variables add header = [Header("Specific Ability")]

Ability class will already have PlayerMovement, PlayerStats, Rigidbody and Orientation from player
Overrride Activate class to give functionality for ability (a_AirUpdraft.cs for simple example)

Add [CreateAssetMenu] to above the class
Right click in asset menu and create scriptable object for new ability and set stats

You should now be able to attach to player ability holder script to give ability
*/

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
    protected PlayerAbilityHolder holder;

    public virtual void Activate(){}

    public void CollectAbility(GameObject parent){
        pm= parent.GetComponent<PlayerMovement>();
        pStats= parent.GetComponent<PlayerStats>();
        rb = parent.GetComponent<Rigidbody>();
        holder=parent.GetComponent<PlayerAbilityHolder>();
        orientation = pm.orientation;
        maxAbilityCoolDownTimer = useAbilityCoolDownTime;
    }

    public KeyCode GetAbilityKey(Ability ability){
        return holder.GetKeyForAbility(ability);
    }

    
}

public enum AbilityType{
    air,
    water,
    fire,
    earth
}
