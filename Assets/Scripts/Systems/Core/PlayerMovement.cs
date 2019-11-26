using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    public float turnSpeed = 1;
    
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 newDirection = Vector3.zero;

    private Animator anim;

    public bool stopMovement = false;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopMovement)
        {
            return;
        }

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            anim.SetFloat("moveSpeed", moveDirection.magnitude);

            newDirection = Vector3.RotateTowards(gameObject.transform.forward, moveDirection,turnSpeed,0);
            gameObject.transform.rotation = Quaternion.LookRotation(newDirection);

            /*if (Input.GetButton("Jump")) // Haha no more jump
            {
                moveDirection.y = jumpSpeed;
            }*/
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void StopMovement(bool stopMove)
    {
        stopMovement = stopMove;
        anim.SetBool("stopped", stopMove);
        if(stopMove)
            anim.SetFloat("moveSpeed", 0);
    }
}
