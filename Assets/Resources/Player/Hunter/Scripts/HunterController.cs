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
    private float STAMINA_MAKE_NOISE_SPEND = 1f;
    private float STAMINA_PUT_TRAP_SPEND = 0f;
    private float PICK_DISTANCE = 1.5f;

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
    //public GameObject floatingText;
    public GameObject blood;
    public GameObject minimap;
    public GameObject shockWave;
    //public GameObject bearTrap;

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
    public int selectedTrap = 0;
    private IObject objectToPick = null;
    // Control not holding attack button
    bool holdAttackButton = false;
    bool holdSelectObjectButton = false;

    // List of equiped weapons
    string[] weapons = { "Crossbow", "Melee2H" };

    public string[] trapsName = { "BearTrap", "Barrel" };
    public int[] trapsNumber = { 2, 0 };
    public GameObject[] trapsPrefab = { };

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
            //if (!Busy()) {
            //    Move();
            //}
            //ChangeState();
            //UpdateStamina(STAMINA_DEFAULT_RECOVER, true);
        }
    }

    private void FixedUpdate() {
        if (!dead) {
            if (!Busy()) {
                Move();
            }
            ChangeState();
            UpdateStamina(STAMINA_DEFAULT_RECOVER, true);
        }
        
    }

    private void UpdateStamina(float value, bool time) {
        stamina += time? value * Time.deltaTime : value;
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

            // Make noise
            if (Input.GetButtonDown("MakeNoise") && stamina >= STAMINA_MAKE_NOISE_SPEND) {
                MakeNoise();
            }

            if (Input.GetButtonDown("PutTrap") && stamina >= STAMINA_PUT_TRAP_SPEND) {
                PutTrap();
            }

            if (Input.GetButtonDown("PickObject")) {
                PickObject();
            }
        }

        float selectObject = Input.GetAxis("SelectObject");
        if (Mathf.Abs(selectObject) == 1 && !holdSelectObjectButton) {
            holdSelectObjectButton = true;
            ChangeSelectedObject((int)selectObject);
        }
        if (selectObject == 0) {
            holdSelectObjectButton = false;
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
                UpdateStamina(-STAMINA_RUN_SPEND, true);
            }
        } else {
            // Idle
            nextState = 0f;
            UpdateStamina(STAMINA_IDLE_RECOVER, true);
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
    private bool Busy() {
        return anim.GetBool("PutTrap") ||
            anim.GetBool("PickObject") ||
            anim.GetBool("MakeNoise");
    }

    // Returns if hands are busy (it is not possible to make new actions with hands)
    private bool HandsBusy() {
        return Busy() ||
            anim.GetBool("ChangeWeapon") ||
            anim.GetBool("Attack") ||
            anim.GetBool("Hit");
    }

    // Attack controller
    private void Attack() {
        UpdateStamina(-STAMINA_ATTACK_SPEND, false);
        anim.SetBool("Attack", true);
    }

    // Callback from fire crossbow animation to fire a bolt
    private void FireBolt(AnimationEvent evt) {
        // if (evt.animatorClipInfo.weight > 0.5) {
        if (boltLoaded >= BOLT_RELOAD_TIME) {
            boltLoaded = 0;
            StartCoroutine(BoltReload());
            Vector3 offset = transform.forward * 1.8f + transform.up * 1.2f;
            GameObject b = Instantiate(bolt, transform.position + offset, transform.rotation);
            Destroy(b, 5f);
        }
    }

    private void MakeNoise() {
        anim.SetBool("MakeNoise", true);
        UpdateStamina(-STAMINA_MAKE_NOISE_SPEND, false);
        GameObject wave = Instantiate(shockWave, transform.position, Quaternion.identity);
        Destroy(wave, 5f);
        Collider[] enemies = Physics.OverlapSphere(transform.position, 15f);
        foreach (Collider enemy in enemies) {
            if (enemy!=null && enemy.gameObject.tag == "Enemy") {
                EnemyController ec = enemy.GetComponent<EnemyController>();
                ec.SetTarget(transform.position, 999);
            }
        }
    }

    private void PutTrap() {
        if (trapsNumber[selectedTrap] > 0) {
            anim.SetBool("PutTrap", true);
            UpdateStamina(-STAMINA_PUT_TRAP_SPEND, false);
            trapsNumber[selectedTrap]--;
        }
    }

    private void PutTrapCallback() {
        Vector3 offset = transform.forward;
        GameObject trap = null;
        if (selectedTrap >= 0) {
            trap = trapsPrefab[selectedTrap];

            if (trap != null) {
                Instantiate(trap, transform.position + offset, Quaternion.identity);
            }
        }
    }

    private void PickObject() {
        GameObject obj = GetNearestObject();
        if (obj != null) {
            IObject o = obj.GetComponent<IObject>();
            if (o != null) {
                transform.LookAt(obj.transform);
                objectToPick = o;
                anim.SetBool("PickObject", true);
            }
        }
    }

    private void PickObjectCallback() {
        if (objectToPick != null) {
            objectToPick.PickObject(this);
            objectToPick = null;
        }
    }

    private void ChangeSelectedObject(int num) {
        if (!anim.GetBool("PutTrap")) {
            selectedTrap = selectedTrap + num;
            if (selectedTrap < 0) {
                selectedTrap = trapsNumber.Length-1;
            }
            if (selectedTrap >= trapsNumber.Length) {
                selectedTrap = 0;
            }
            Debug.Log(selectedTrap);
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
        FinishAllActions();

        Instantiate(blood, transform.position+ new Vector3(0f, 1f, 0f), Quaternion.identity, transform);
        //GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        //text.GetComponent<TextMesh>().text = amount.ToString();

        life -= amount;
        if (life <= 0) {
            Dead();
        }
    }

    private void FinishAllActions() {
        anim.SetBool("ChangeWeapon", false);
        anim.SetBool("Attack", false);
        meleeAttacking = false;
        anim.SetBool("MakeNoise", false);
        anim.SetBool("PutTrap", false);
        anim.SetBool("PickObject", false);
        objectToPick = null;
    }

    private void Dead() {
        anim.SetBool("Die", true);
        StopAllCoroutines();
        Destroy(GetComponent<Collider>());
        Destroy(minimap);
        dead = true;
    }
    
    private GameObject GetNearestObject() {
        float nearest = PICK_DISTANCE;
        GameObject nearestObj = null;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Object");

        foreach (GameObject obj in objs) {
            IObject o = obj.GetComponent<IObject>();
            if (o != null && o.Pickable()) {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < nearest) {
                    nearest = distance;
                    nearestObj = obj;
                }
            }
        }

        return nearestObj;
    }

    //private GameObject[] GetObjects() {
    //    GameObject[] objs = { };

    //    objs = GameObject.FindGameObjectsWithTag("Object");

    //    System.Array.Sort(objs, delegate (GameObject a, GameObject b)
    //    {
    //        return Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, gameObject.transform.position));
    //    });

    //    return objs;
    //}


}

