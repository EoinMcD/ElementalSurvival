using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class a_AirUpdraft : Ability
{
    [SerializeField] float dashUpForce;
    

    public override void Activate()
    {
        Debug.Log("DASHING");

        Vector3 forceToApply = orientation.up * dashUpForce;
        pm.ResetVelocity();
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

}
