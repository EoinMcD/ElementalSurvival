using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class a_AirUpdraft : Ability
{
    [SerializeField] float dashUpForce;
    

    public override void Activate(GameObject parent)
    {
        Debug.Log("DASHING");
        PlayerMovement movement = parent.GetComponent<PlayerMovement>();
        Rigidbody rb = parent.GetComponent<Rigidbody>();
        PlayerCam playerCam = parent.GetComponent<PlayerCam>();
        Transform orientation = movement.orientation;

        Vector3 forceToApply = orientation.up * dashUpForce;
        movement.ResetVelocity();
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

}
