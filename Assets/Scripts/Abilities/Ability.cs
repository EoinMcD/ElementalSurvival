using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string abilityName;
    public AbilityType abilityType;
    public float coolDownTime;
    public float activeTime;

    public virtual void Activate(GameObject parent){}
    
}

public enum AbilityType{
    air,
    water,
    fire,
    earth
}
