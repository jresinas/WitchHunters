using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterController : MonoBehaviour
{
    float walkSpeed = 3f;
    float runSpeed = 7f;
    Rigidbody rb;
    Animator anim;
    float life = 100f;
    float stamina = 100f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move() {
        float VerticalInput = Input.GetAxis("Vertical");
        float HorizontalInput = Input.GetAxis("Horizontal");
        float RunInput = Input.GetAxis("Run");
        //float speed;

        Vector3 move = new Vector3(HorizontalInput, 0f, VerticalInput).normalized;
        transform.LookAt(transform.position + move);

        // speed = (RunInput <= 0.5) ? walkSpeed : runSpeed;
        ClearMoveAnimations();
        if (RunInput <= 0.5) {
            if (VerticalInput != 0 || HorizontalInput != 0) {
                anim.SetBool("Walk", true);
            }

            anim.SetFloat("WalkAnimationSpeed", move.magnitude);
            rb.MovePosition(rb.position + move * walkSpeed * Time.deltaTime);
        } else {
            if (VerticalInput != 0 || HorizontalInput != 0) {
                anim.SetBool("Run", true);
            }

            rb.MovePosition(rb.position + move.normalized * runSpeed * Time.deltaTime);
        }

    }

    private void ClearMoveAnimations() {
        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);
    }

}
