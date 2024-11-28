using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public CharacterController controller;
    public Animator animator;

    public float speed=6f;
    public float jumpSpeed = 9f;
    public float ySpeed;
    public float turnSmoothTime =0.1f;
    float turnSmoothVelocity;
    
    void Start() {
        animator= GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        ySpeed+=Physics.gravity.y * Time.deltaTime;
        if(Input.GetButtonDown("Jump")) {
            Debug.Log("Jump");
            ySpeed=jumpSpeed;
            // vel.y=ySpeed;
        }


        if(direction.magnitude >= 0.1f) {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothVelocity);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            animator.SetBool("isMoving", true);
            Debug.Log("Walking");
        } else {
            animator.SetBool("isMoving", false);
        }

            controller.SimpleMove(direction*speed*Time.deltaTime);
    }
}