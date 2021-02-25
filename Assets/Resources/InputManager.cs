using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour{
    public PlayerController pc;
    public PlayerWeaponController wc;
    public PlayerObjectController oc;
    public PlayerMoveController mc;
    // Control not holding attack button
    bool holdAttackButton = false;
    bool holdSelectObjectButton = false;

    void Update() {
        if (!pc.dead && !GameManager.instance.IsPaused()) {
            switch (GameManager.instance.inputMode) {
                case 0:
                    break;
                case 1:
                    NormalInputs();
                    break;
                case 2:
                    SpecialInputs();
                    break;
            }
        }

        // Pause Game
        if (Input.GetButtonDown("Pause")) {
            GameManager.instance.SetPause();
        }
    }

    private void NormalInputs() {
        if (!pc.Busy()) {
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
                mc.Move(move, view, runInput > 0.5);
            } else {
                mc.Idle();
            }
        }

        if (!pc.HandsBusy()) {
            // Change weapon
            if (Input.GetButtonDown("ChangeWeapon")) {
                wc.ChangeWeapon();
            }

            if (Input.GetButtonDown("SwitchArrow")) {
                wc.ChangeBolt();
            }

            // Attack
            float attackInput = Input.GetAxis("Attack");
            if (attackInput == 1 && !holdAttackButton) {
                wc.Attack();
                holdAttackButton = true;
            }
            if (attackInput == 0) {
                holdAttackButton = false;
            }

            // Make noise
            if (Input.GetButtonDown("MakeNoise")) {
                pc.MakeNoise();
            }

            if (Input.GetButtonDown("UseObject")) {
                oc.UseObject();
            }

            if (Input.GetButtonDown("PickTrap")) {
                oc.PickTrap();
            }
        }

        float selectObject = Input.GetAxis("SelectObject");
        if (Mathf.Abs(selectObject) == 1 && !holdSelectObjectButton) {
            holdSelectObjectButton = true;
            oc.ChangeSelectedObject((int)selectObject);
        }
        if (selectObject == 0) {
            holdSelectObjectButton = false;
        }


        // Switch minimap size
        if (Input.GetButtonDown("ResizeMinimap")) {
            HudController.instance.ResizeMinimap();
        }
      
    }

    private void SpecialInputs() {
        if (Input.GetButtonDown("Submit")) {
            GameManager.instance.MakeSelection();
        }
    }
}
