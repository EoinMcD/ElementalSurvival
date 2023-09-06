using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSliding : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerObj;

    Rigidbody rb;
    PlayerMovement pm;

    [Header("Sliding")]
    [SerializeField] float maxSlideTime;
    [SerializeField] float slideForce;
    [SerializeField] float slideYScale;
    float startYScale;
    float slideTimer;

    [Header("Input")]
    [SerializeField] KeyCode slideKey = KeyCode.C;
    float horizontalInput;
    float verticalInput;

    bool sliding;
    public bool decreasingSlide;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        startYScale = playerObj.localScale.y;
    }

    private void Update() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(slideKey) &&(pm.isPlayerGrounded()) &&((horizontalInput!=0) || (verticalInput!=0))){
            Debug.Log("Sliding");
            StartSlide();
        }
        if(Input.GetKeyUp(slideKey) && pm.sliding) {
            StopSlide();
        }
    }

    void FixedUpdate() {
        if(pm.sliding) {
            SlidingMovement();
        }
    }

    public void StartSlide() {
        pm.sliding=true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale,playerObj.localScale.z);
        rb.AddForce(Vector3.down*5f, ForceMode.Impulse);

        slideTimer=maxSlideTime;
        decreasingSlide=false;
    }

    void SlidingMovement() {
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right *horizontalInput;    
        if(!pm.OnSlope() || rb.velocity.y > -0.1) {
            

            rb.AddForce(inputDir.normalized*slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }
        else {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDir)*slideForce, ForceMode.Force);
        }
        if(slideTimer <=.4) {
            decreasingSlide=true;
            Debug.Log(pm.moveSpeed);
            if(!(pm.moveSpeed <= 0.05)) {
                pm.moveSpeed-= Time.deltaTime * 10;
            }
        }
    }

    public void StopSlide() {
        pm.sliding=false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale,playerObj.localScale.z);
    }

    
}
