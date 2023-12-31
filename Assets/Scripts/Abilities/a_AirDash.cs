using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class a_AirDash : Ability
{
    [Header("Specific Ability")]
    [SerializeField] float dashVelocity;
    [SerializeField] float dashUpForce;
    [SerializeField] bool allowAllDirection;
    

    public override void Activate()
    {
        Debug.Log("DASHING");

        Vector3 direction = GetDirection(orientation);
        Vector3 forceToApply = direction * dashVelocity + orientation.up * dashUpForce;
        rb.AddForce(forceToApply, ForceMode.Impulse);
        
    }

    Vector3 GetDirection(Transform forwardT) {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();
        if(allowAllDirection && (verticalInput!=0||horizontalInput!=0) ) {
            direction = forwardT.forward * verticalInput + forwardT.right* horizontalInput;
        }
        else if(allowAllDirection) {
             direction = forwardT.forward;
        }
        else {
            direction = forwardT.forward;
        }

        return direction.normalized;
    }
}
