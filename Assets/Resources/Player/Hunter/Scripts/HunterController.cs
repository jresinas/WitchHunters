using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterController : MonoBehaviour {
    Rigidbody rb;
    Animator anim;
    // Empty object in the character back to keep unused weapons
    public GameObject bag;
    // Character right hand
    public GameObject rightHand;
    // Crossbow in character hands
    public GameObject crossbow;
    // Crossbow keep in the character back bag
    public GameObject crossbowKeep;
    // Axe in character hands
    public GameObject axe;
    // Axe keep in the character back bag
    public GameObject axeKeep;
    // Bolts throws from crossbow
    public GameObject bolt;

    float walkSpeed = 3f;
    float runSpeed = 8f;

    float life = 100f;
    float stamina = 100f;

    float verticalInput;
    float horizontalInput;
    float verticalViewInput;
    float horizontalViewInput;
    float runInput;

    // Control not holding attack button
    bool holdAttackButton = false;

    // List of equiped weapons
    string[] weapons = { "Crossbow", "Melee2H" };
    // Current weapon
    int weapon = 0;
    // Next weapon to select
    int nextWeapon = 1;


    string status = "Idle";

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ChangeWeapon();
    }

    // Update is called once per frame
    void Update() {
        WeaponsPosition();
        Inputs();
        Move();
    }

    // Attach weapons to character movements
    private void WeaponsPosition() {
        crossbow.transform.position = rightHand.transform.position;
        crossbowKeep.transform.position = bag.transform.position;
        axe.transform.position = rightHand.transform.position;
        axe.transform.rotation = rightHand.transform.rotation;
        axeKeep.transform.position = bag.transform.position;
    }

    // Inputs controller
    private void Inputs() {
        // Movement and rotation
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        verticalViewInput = Input.GetAxis("VerticalView");
        horizontalViewInput = Input.GetAxis("HorizontalView");
        runInput = Input.GetAxis("Run");

        if (!HandsBusy()) {
            // Change weapon
            if (Input.GetButtonDown("Switch Weapon Left")) {
                //ChangeWeapon(weapon -1);
                SetNextWeapon(-1);
                anim.SetBool("ChangeWeapon", true);
            }
            if (Input.GetButtonDown("Switch Weapon Right")) {
                SetNextWeapon(1);
                anim.SetBool("ChangeWeapon", true);
                //ChangeWeapon(weapon + 1);
            }

            // Attack
            float attackInput = Input.GetAxis("Attack");

            if (attackInput == 1 && !holdAttackButton) {
                Attack();
                holdAttackButton = true;
            }
            if (attackInput == 0) {
                holdAttackButton = false;
            }
        }
    }

    // Movement and rotation controller
    private void Move() {
        // Get move and view vectors
        Vector3 move = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        Vector3 view = new Vector3(horizontalViewInput, 0f, verticalViewInput);

        // If no view vector, character look at movement direction
        if (view == Vector3.zero) {
            view = move;
        }

        // Rotate character to view vector direction
        transform.LookAt(transform.position + view);

        // Set MoveRotation animation parameter in function of move and view vectors
        // MoveRotation controls foot blend animations (WalkBlend:WalkBack/WalkLeft/WalkForward/WalkRight and RunBlend:RunBack/RunLeft/RunForward/RunRight)
        Vector3 aux = Vector3.Cross(move, view);
        int sign = (aux.y < 0) ? 1 : -1;
        float angle = Vector3.Angle(move, view);
        anim.SetFloat("MoveRotation", sign * angle / 90);

        ClearMoveAnimations();
        if (verticalInput != 0 || horizontalInput != 0) {
            if (runInput <= 0.5) {
                // Walk
                //anim.SetBool("Walk", true);
                status = "Walk";
                anim.SetLayerWeight(anim.GetLayerIndex("Walk"), 1f);
                rb.MovePosition(rb.position + move * walkSpeed * Time.deltaTime);
            } else {
                // Run
                //anim.SetBool("Run", true);
                status = "Run";
                anim.SetLayerWeight(anim.GetLayerIndex("Run"), 1f);
                rb.MovePosition(rb.position + move * runSpeed * Time.deltaTime);
            }
        } else {
            // Idle
            status = "Idle";
        }
        anim.SetLayerWeight(anim.GetLayerIndex(weapons[weapon] + status), 1f);
    }

    // Set to false all animations move parameters
    private void ClearMoveAnimations() {
        //anim.SetBool("Walk", false);
        //anim.SetBool("Run", false);
        //anim.SetBool("Aim", false);
        anim.SetLayerWeight(anim.GetLayerIndex("Walk"), 0f);
        anim.SetLayerWeight(anim.GetLayerIndex("Run"), 0f);
        anim.SetLayerWeight(anim.GetLayerIndex("Melee2HWalk"), 0f);
        anim.SetLayerWeight(anim.GetLayerIndex("Melee2HRun"), 0f);
        anim.SetLayerWeight(anim.GetLayerIndex("Melee2HIdle"), 0f);
    }

    // Returns if hands are busy (it is not possible to make new actions with hands)
    private bool HandsBusy() {
        return anim.GetBool("ChangeWeapon") ||
            anim.GetBool("Attack");
    }

    // Attack controller
    private void Attack() {
        anim.SetBool("Attack", true);
        //switch (weapons[weapon]) {
        //    case ("Crossbow"):
        //        break;
        //    case ("Axe"):
        //        break;
        //    default:
        //        break;
        //}
    }

    private void FireBolt() { 
        Vector3 offset = transform.forward*1.5f + transform.up*1.5f + transform.right*0.3f;
        Instantiate(bolt, transform.position+offset, transform.rotation);
    }

    // Select next weapon
    private void SetNextWeapon(int next) {
        nextWeapon += next;
        if (nextWeapon < 0) {
            nextWeapon = weapons.Length - 1;
        } else if (nextWeapon >= weapons.Length) {
            nextWeapon = 0;
        }
    }

    // Callback from animations to keep current weapon and draw new weapon
    private void ChangeWeapon() {
        switch (weapons[weapon]) {
            case ("Crossbow"):
                crossbow.SetActive(false);
                crossbowKeep.SetActive(true);
                break;
            case ("Melee2H"):
                axe.SetActive(false);
                axeKeep.SetActive(true);
                break;
        }

        switch (weapons[nextWeapon]) {
            case ("Crossbow"):
                crossbow.SetActive(true);
                crossbowKeep.SetActive(false);
                break;
            case ("Melee2H"):
                axe.SetActive(true);
                axeKeep.SetActive(false);
                break;
        }

        anim.SetLayerWeight(anim.GetLayerIndex(weapons[weapon]+status), 0f);
        anim.SetLayerWeight(anim.GetLayerIndex(weapons[nextWeapon]+status), 1f);
        weapon = nextWeapon;
    }

    // Callback from animations to notify it is finished
    private void EndAnimation(string animParamName) {
        anim.SetBool(animParamName, false);
    }


    

}
