using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    [SerializeField] float walkSpeed=7f;
    [SerializeField] float sprintSpeed=14f;
    [SerializeField] float groundDrag=8f;
    [SerializeField] float slideSpeed=14f;
    
    public bool sliding;

    [Space]
    [SerializeField] Transform orientation;

    [Header("Jumping")]
    [SerializeField] float jumpForce=14f;
    [SerializeField] float jumpCooldown=0.3f;
    [SerializeField] float airMultiplier=0.4f;
    [SerializeField] float airDrag=.66f;
    [SerializeField] int maxJumps=1;
    float numJumps;
    bool readyToJump;

    [Header("Crouching")]
    [SerializeField] float crouchSpeed=3.5f;
    [SerializeField] float crouchYScale=.5f;
    float startYScale;

    [Header("Ground Check")]
    [SerializeField] float playerHeight=2;
    bool grounded;
    
    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Slope Handling")]
    [SerializeField] float maxSlopeAngle=40f;
    [SerializeField] float speedIncreaseMultiplier=1.5f;
    [SerializeField] float slopeIncreaseMultiplier=2.5f;
    RaycastHit slopeHit;
    bool exitingSlope;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    Rigidbody rb;
    [Space]
    [SerializeField] MovementState moveState;
    PlayerSliding ps;

    public enum MovementState {
        walking,
        sprinting,
        air,
        sliding,
        crouching
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<PlayerSliding>();
        rb.freezeRotation = true;

        
        readyToJump = true;
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        MyInput();

        SpeedControl();
        StateHandler();
        IsGrounded();
        

        



       
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    bool IsGrounded() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .1f);
        if(grounded) {
            rb.drag = groundDrag;
        }
        else { rb.drag = airDrag; }
        if(grounded && readyToJump) {
            numJumps = maxJumps;
        }
        return grounded;
    }

    


    void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if(((Input.GetKey(jumpKey)&&grounded) || (Input.GetKeyDown(jumpKey) && moveState==MovementState.air)) && numJumps>=1 && readyToJump) {
            
            Jump();

            
        }

        if(Input.GetKeyDown(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if(Input.GetKeyUp(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            
        }
    }

    void MovePlayer() {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if(OnSlope() && !exitingSlope) {
            rb.AddForce(GetSlopeMoveDirection(moveDir) * moveSpeed * 20, ForceMode.Force);

            if(rb.velocity.y > 0) {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        
        else if(grounded) {
            rb.AddForce(moveDir.normalized * moveSpeed *10, ForceMode.Force);
            
        } 
        else if(!grounded) {
            rb.AddForce(moveDir.normalized * moveSpeed  * airMultiplier*10, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    void SpeedControl() {
        if(OnSlope() && !exitingSlope) {
            if(rb.velocity.magnitude > moveSpeed) {
                rb.velocity= rb.velocity.normalized * moveSpeed;
            }
        }
        else {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if(flatVel.magnitude > moveSpeed) {
                Vector3 limitVel= flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitVel.x,rb.velocity.y,limitVel.z);
            }
        }
    }

    void Jump() {
        readyToJump = false;
        numJumps--;
        exitingSlope = true;
        Debug.Log("JUMPING");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(sliding) {
            rb.AddForce(transform.up * jumpForce*1.5f, ForceMode.Impulse);
            rb.AddForce(transform.forward *30f,ForceMode.Impulse);
        }
        else {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        Invoke(nameof(ResetJump),jumpCooldown);
    }

        

    void ResetJump() {
        readyToJump =true;
        exitingSlope = false;
    }


    void StateHandler() {
        if(sliding) {
            moveState = MovementState.sliding;
            if(!ps.decreasingSlide) {
                moveSpeed = slideSpeed;
            }
        }
        else if(Input.GetKey(crouchKey)) {
            moveState = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if(grounded && Input.GetKey(sprintKey)) {
            moveState = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if(grounded) {
            moveState = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else {
            moveState = MovementState.air;
        }
    }

    public bool OnSlope() {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit , playerHeight * .5f +.3f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle <maxSlopeAngle && angle !=0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction) {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;

    }

    public bool isPlayerGrounded(){
        return grounded;
    }
}
