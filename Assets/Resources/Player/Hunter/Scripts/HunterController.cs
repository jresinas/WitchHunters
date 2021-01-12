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

    float verticalInput;
    float horizontalInput;
    float runInput;
    float aimInput;
    string[] weapons = { "Unarmed", "Crossbow" };
    int weapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Busy()) {
            Inputs();
            Move();
        }
    }

    private void Inputs() {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        runInput = Input.GetAxis("Run");
        aimInput = Input.GetAxis("Aim");

        if (Input.GetButtonDown("Switch Weapon Left")) {
            ChangeWeapon(weapon -1);
        }
        if (Input.GetButtonDown("Switch Weapon Right")) {
            ChangeWeapon(weapon + 1);
        }
    }

    private void ChangeWeapon(int nextWeapon) {
        anim.SetBool(weapons[weapon], false);
        anim.SetLayerWeight(weapon, 0f);


        if (nextWeapon < 0) {
            weapon = weapons.Length - 1;
        } else if (nextWeapon >= weapons.Length) {
            weapon = 0;
        } else {
            weapon = nextWeapon;
        }

        Debug.Log(weapon);

        anim.SetLayerWeight(weapon, 1f);
        anim.SetBool(weapons[weapon], true);

    }

    private void Move() {
        Vector3 move = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        transform.LookAt(transform.position + move);

        // float speed = (RunInput <= 0.5) ? walkSpeed : runSpeed;
        ClearMoveAnimations();
        if (aimInput <= 0.5) {
            if (runInput <= 0.5) {
                // Walk
                if (verticalInput != 0 || horizontalInput != 0) {
                    anim.SetBool("Walk", true);
                }

                anim.SetFloat("WalkAnimationSpeed", move.magnitude);
                rb.MovePosition(rb.position + move * walkSpeed * Time.deltaTime);
            } else {
                // Run
                if (verticalInput != 0 || horizontalInput != 0) {
                    anim.SetBool("Run", true);
                }

                rb.MovePosition(rb.position + move * runSpeed * Time.deltaTime);
            }
        } else {
            // Aim
            anim.SetBool("Aim", true);
        }
    }

    private void ClearMoveAnimations() {
        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);
        anim.SetBool("Aim", false);
    }

    private void ToggleParameter(string parameterName) {
        if (anim.GetBool(parameterName)) {
            anim.SetBool(parameterName, false);
        } else {
            anim.SetBool(parameterName, true);
        }
    }

    private bool Busy() {
        return anim.GetBool("Crossbow");
    }

}
