using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour{
    public HunterController hc;
    // Control not holding attack button
    bool holdAttackButton = false;
    bool holdSelectObjectButton = false;

    void Update() {
        if (!hc.dead && !GameManager.instance.IsPaused()) {
            Inputs();
        }

        // Pause Game
        if (Input.GetButtonDown("Pause")) {
            GameManager.instance.SetPause();
        }
    }

    private void Inputs() {
        if (!hc.Busy()) {
            // Movement and rotation
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalViewInput = Input.GetAxis("VerticalView");
            float horizontalViewInput = Input.GetAxis("HorizontalView");
            float runInput = Input.GetAxis("Run");

            // Get move and view vectors
            Vector3 move = new Vector3(horizontalInput, 0f, verticalInput).normalized;
            Vector3 view = new Vector3(horizontalViewInput, 0f, verticalViewInput);

            if (move != Vector3.zero || view != Vector3.zero) {
                hc.Move(move, view, runInput > 0.5);
            } else {
                hc.Idle();
            }
        }

        if (!hc.HandsBusy()) {
            // Change weapon
            if (Input.GetButtonDown("ChangeWeapon")) {
                hc.ChangeWeapon();
            }

            if (Input.GetButtonDown("SwitchArrow")) {
                hc.ChangeBolt();
            }

            // Attack
            float attackInput = Input.GetAxis("Attack");
            if (attackInput == 1 && !holdAttackButton) {
                hc.Attack();
                holdAttackButton = true;
            }
            if (attackInput == 0) {
                holdAttackButton = false;
            }

            // Make noise
            if (Input.GetButtonDown("MakeNoise")) {
                hc.MakeNoise();
            }

            if (Input.GetButtonDown("UseObject")) {
                hc.UseObject();
            }

            if (Input.GetButtonDown("PickTrap")) {
                hc.PickTrap();
            }
        }

        float selectObject = Input.GetAxis("SelectObject");
        if (Mathf.Abs(selectObject) == 1 && !holdSelectObjectButton) {
            holdSelectObjectButton = true;
            hc.ChangeSelectedObject((int)selectObject);
        }
        if (selectObject == 0) {
            holdSelectObjectButton = false;
        }


        // Switch minimap size
        if (Input.GetButtonDown("ResizeMinimap")) {
            HudController.instance.ResizeMinimap();
        }
      
    }
}
