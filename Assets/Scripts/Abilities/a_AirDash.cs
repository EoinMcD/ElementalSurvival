using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class a_AirDash : Ability
{
    [SerializeField] float dashVelocity;
    [SerializeField] float dashUpForce;
    [SerializeField] float dashMaxSpeed;
    [SerializeField] bool allowAllDirection;
    

    public override void Activate(GameObject parent)
    {
        Debug.Log("DASHING");
        PlayerMovement movement = parent.GetComponent<PlayerMovement>();
        Rigidbody rb = parent.GetComponent<Rigidbody>();
        Transform orientation = movement.orientation;
        PlayerCam playerCam = parent.GetComponent<PlayerCam>();

        Vector3 direction = GetDirection(orientation);
        Vector3 forceToApply = direction * dashVelocity + orientation.up * dashUpForce;
        movement.SetMaxSpeed(dashMaxSpeed,activeTime);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    Vector3 GetDirection(Transform forwardT) {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();
        if(allowAllDirection) {
            direction = forwardT.forward * verticalInput + forwardT.right* horizontalInput;
        }
        else {
            direction = forwardT.forward;
        }

        return direction.normalized;
    }
}
