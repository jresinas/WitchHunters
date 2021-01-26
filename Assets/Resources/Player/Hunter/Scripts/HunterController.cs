using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterController : MonoBehaviour {
    private float BOLT_RELOAD_TIME = 1f;
    public float MAX_LIFE = 10f;
    public float MAX_STAMINA = 10f;
    private float STAMINA_DEFAULT_RECOVER = 0.25f;
    private float STAMINA_IDLE_RECOVER = 0.1f;
    private float STAMINA_ATTACK_SPEND = 0.25f;
    private float STAMINA_RUN_SPEND = 1f;

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
    public GameObject floatingText;
    public GameObject minimap;

    float walkSpeed = 4f;
    float runSpeed = 8f;

    public float life;
    public float stamina;
    float boltLoaded;

    float verticalInput;
    float horizontalInput;
    float verticalViewInput;
    float horizontalViewInput;
    float runInput;

    public bool dead = false;
    public bool meleeAttacking = false;

    // Control not holding attack button
    bool holdAttackButton = false;

    // List of equiped weapons
    string[] weapons = { "Crossbow", "Melee2H" };
    // Current weapon
    int weapon = 1;
    // Next weapon to select
    int nextWeapon = 0;
    // Next movement state (0:idle, 1:walk, 2:run)
    float nextState = 0f;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ChangeWeapon();
        life =  MAX_LIFE;
        stamina = MAX_STAMINA;
        boltLoaded = BOLT_RELOAD_TIME;
    }

    // Update is called once per frame
    void Update() {
        if (!dead) {
            WeaponsPosition();
            Inputs();
            Move();
            ChangeState();
            UpdateStamina(STAMINA_DEFAULT_RECOVER);
        }
    }

    private void UpdateStamina(float value) {
        stamina += value * Time.deltaTime;
        if (stamina > MAX_STAMINA) {
            stamina = MAX_STAMINA;
        }
        if (stamina < 0) {
            stamina = 0;
        }
    }

    // Attach weapons to character movements
    private void WeaponsPosition() {
        crossbow.transform.position = rightHand.transform.position;
        crossbowKeep.transform.position = bag.transform.position;
        axe.transform.position = rightHand.transform.position;
        axe.transform.rotation = rightHand.transform.rotation;
        axeKeep.transform.position = bag.transform.position;
    }


    private IEnumerator BoltReload() {
        yield return new WaitForSeconds(1);
        boltLoaded++;
        if (boltLoaded < BOLT_RELOAD_TIME) {
            StartCoroutine(BoltReload());
        }
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
                SetNextWeapon(-1);
                anim.SetBool("ChangeWeapon", true);
            }
            if (Input.GetButtonDown("Switch Weapon Right")) {
                SetNextWeapon(1);
                anim.SetBool("ChangeWeapon", true);
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
        //transform.LookAt(transform.position + view - new Vector3(0,0,-0.5f));

        // Set MoveRotation animation parameter in function of move and view vectors
        // MoveRotation controls foot blend animations (WalkBlend:WalkBack/WalkLeft/WalkForward/WalkRight and RunBlend:RunBack/RunLeft/RunForward/RunRight)
        Vector3 aux = Vector3.Cross(move, view);
        int sign = (aux.y < 0) ? 1 : -1;
        float angle = Vector3.Angle(move, view);
        anim.SetFloat("MoveRotation", sign * angle / 90);

        if (verticalInput != 0 || horizontalInput != 0) {
            if (runInput <= 0.5 || (nextState == 2 && stamina <= 0.1) || (nextState != 2 && stamina < STAMINA_RUN_SPEND/4)) {
                // Walk
                nextState = 1f;
                rb.MovePosition(rb.position + move * walkSpeed * Time.deltaTime);
            } else {
                // Run
                nextState = 2f;
                rb.MovePosition(rb.position + move * runSpeed * Time.deltaTime);
                UpdateStamina(-STAMINA_RUN_SPEND);
            }
        } else {
            // Idle
            nextState = 0f;
            UpdateStamina(STAMINA_IDLE_RECOVER);
        }
    }

    // Set param State gradually to make smooth transitions between states (idle, walk and run)
    private void ChangeState() {
        anim.SetFloat("StateArm", nextState);
        if (anim.GetFloat("StateFoot") - 0.1f <= nextState) {
            anim.SetFloat("StateFoot", anim.GetFloat("StateFoot") + 0.1f);
        }
        if (anim.GetFloat("StateFoot") + 0.1f >= nextState) {
            anim.SetFloat("StateFoot", anim.GetFloat("StateFoot") - 0.1f);
        }
    }

    // Returns if hands are busy (it is not possible to make new actions with hands)
    private bool HandsBusy() {
        return anim.GetBool("ChangeWeapon") ||
            anim.GetBool("Attack") ||
            anim.GetBool("Hit");
    }

    // Attack controller
    private void Attack() {
        UpdateStamina(-STAMINA_ATTACK_SPEND);
        anim.SetBool("Attack", true);
    }

    // Callback from fire crossbow animation to fire a bolt
    private void FireBolt(AnimationEvent evt) {
        // if (evt.animatorClipInfo.weight > 0.5) {
        if (boltLoaded >= BOLT_RELOAD_TIME) {
            boltLoaded = 0;
            StartCoroutine(BoltReload());
            Vector3 offset = transform.forward * 1.8f + transform.up * 1.2f;
            Instantiate(bolt, transform.position + offset, transform.rotation);
        }
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

        anim.SetLayerWeight(anim.GetLayerIndex(weapons[weapon]), 0f);
        anim.SetLayerWeight(anim.GetLayerIndex(weapons[nextWeapon]), 1f);

        weapon = nextWeapon;
    }

    // Callback from animations to notify it is finished
    private void EndAnimation(string animParamName) {
        anim.SetBool(animParamName, false);
    }

    private void SetMeleeAttacking(int state) {
        meleeAttacking = (state > 0);
    }

    public void DamageReceived(float amount) {
        anim.SetBool("Hit", true);
        anim.SetBool("ChangeWeapon", false);
        anim.SetBool("Attack", false);
        meleeAttacking = false;

        GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        //GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().text = amount.ToString();

        life -= amount;
        if (life <= 0) {
            Dead();
        }
    }

    private void Dead() {
        anim.SetBool("Die", true);
        StopAllCoroutines();
        Destroy(GetComponent<Collider>());
        Destroy(minimap);
        dead = true;
    }

}
