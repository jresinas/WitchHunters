//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class HunterController1 : MonoBehaviour {
//    public float BOLT_RELOAD_TIME = 1f;
//    public float MAX_LIFE = 10f;
//    public float MAX_STAMINA = 10f;
//    private float STAMINA_DEFAULT_RECOVER = 0.35f;
//    private float STAMINA_WALK_SPEND = 0.1f;
//    private float STAMINA_ATTACK_MELEE2H_SPEND = 0.5f;
//    private float STAMINA_ATTACK_CROSSBOW_SPEND = 0.1f;
//    private float STAMINA_RUN_SPEND = 1f;
//    private float STAMINA_MAKE_NOISE_SPEND = 1f;
//    private float PICK_DISTANCE = 1.8f;
//    private float MAKE_NOISE_RANGE = 15f;

//    Rigidbody rb;
//    Animator anim;
//    // Audio source for foots noises
//    public AudioSource audioFoots;
//    // Audio source for no-foots noises
//    public AudioSource audioHands;
//    // Empty object in the character back to keep unused weapons
//    public GameObject bag;
//    // Character right hand
//    public GameObject rightHand;
//    // Object which represent player in minimap
//    public GameObject minimap;
//    // Blood particle effect
//    public GameObject blood;
//    // Make noise wave particle effect
//    public GameObject shockWave;

//    private float walkSpeed = 4f;
//    private float runSpeed = 8f;
//    public float life;
//    public float stamina;

//    public bool dead = false;
//    public bool meleeAttacking = false;

//    // List of available weapons
//    public PlayerWeapon[] weapons;
//    // Current equiped weapon
//    public int selectedWeapon = 0;

//    // List of inventory objects
//    public PlayerObject[] objs;
//    // Current inventory object selected
//    public int selectedObj = 0;

//    // List of available bolts
//    public GameObject[] bolts;
//    // Current bolt selected
//    int bolt = 0;
//    // Time elapsed since starting reload bolt (in seconds) 
//    public float boltReloadProgress;

//    // Trap selected to pick from the floor
//    private ITrapController trapToPick = null;

//    // Next movement state (0:idle, 1:walk, 2:run)
//    float nextState = 0f;


//    private void Awake() {
//        DontDestroyOnLoad(gameObject);
//        rb = GetComponent<Rigidbody>();
//        anim = GetComponent<Animator>();
//        life = MAX_LIFE;
//        stamina = MAX_STAMINA;
//        boltReloadProgress = BOLT_RELOAD_TIME;
//    }

//    // Start is called before the first frame update
//    void Start() {
        
//    }

//    // Update is called once per frame
//    void Update() {
//        WeaponsPosition();
//        BoltReload();
//    }

//    private void FixedUpdate() {
//        if (!dead) {
//            ChangeState();
//            UpdateStamina(STAMINA_DEFAULT_RECOVER, true);
//        }
        
//    }

//    private void UpdateStamina(float value, bool continuous = false) {
//        stamina += continuous ? value * Time.deltaTime : value;
//        if (stamina > MAX_STAMINA) {
//            stamina = MAX_STAMINA;
//        }
//        if (stamina < 0) {
//            stamina = 0;
//        }
//    }

//    // Attach weapons to character movements
//    private void WeaponsPosition() {
//        foreach (PlayerWeapon pw in weapons) {
//            pw.handObject.transform.position = rightHand.transform.position;
//            pw.bagObject.transform.position = bag.transform.position;
//        }
//    }


//    private void BoltReload() {
//        if (boltReloadProgress < BOLT_RELOAD_TIME) {
//            boltReloadProgress += Time.deltaTime;
//        }
//    }

//    public void Idle() {
//        nextState = 0f;
//    }

//    // Movement and rotation controller
//    public void Move(Vector3 move, Vector3 view, bool run) {
//        // If no view vector, character look at movement direction
//        if (view == Vector3.zero) {
//            view = move;
//        }

//        // Rotate character to view vector direction
//        transform.LookAt(transform.position + view);
//        //transform.LookAt(transform.position + view - new Vector3(0,0,-0.5f));

//        // Set MoveRotation animation parameter in function of move and view vectors
//        // MoveRotation controls foot blend animations (WalkBlend:WalkBack/WalkLeft/WalkForward/WalkRight and RunBlend:RunBack/RunLeft/RunForward/RunRight)
//        Vector3 aux = Vector3.Cross(move, view);
//        int sign = (aux.y < 0) ? 1 : -1;
//        float angle = Vector3.Angle(move, view);
//        anim.SetFloat("MoveRotation", sign * angle / 90);

//        if (move != Vector3.zero) {
//            if (!run || (nextState == 2 && stamina <= 0.1) || (nextState != 2 && stamina < STAMINA_RUN_SPEND/4)) {
//                // Walk
//                nextState = 1f;
//                rb.MovePosition(rb.position + move * walkSpeed * Time.deltaTime);
//                UpdateStamina(STAMINA_WALK_SPEND, true);
//            } else {
//                // Run
//                nextState = 2f;
//                rb.MovePosition(rb.position + move * runSpeed * Time.deltaTime);
//                UpdateStamina(-STAMINA_RUN_SPEND, true);
//            }
//        } else {
//            // Idle
//            Idle();
//        }
//    }

//    // Set param State gradually to make smooth transitions between states (idle, walk and run)
//    private void ChangeState() {
//        anim.SetFloat("StateArm", nextState);
//        if (anim.GetFloat("StateFoot") - 0.1f <= nextState) {
//            anim.SetFloat("StateFoot", anim.GetFloat("StateFoot") + 0.1f);
//        }
//        if (anim.GetFloat("StateFoot") + 0.1f >= nextState) {
//            anim.SetFloat("StateFoot", anim.GetFloat("StateFoot") - 0.1f);
//        }
//    }

//    // Returns if hands are busy (it is not possible to make new actions with hands)
//    public bool Busy() {
//        return anim.GetBool("PutTrap") ||
//            anim.GetBool("PickTrap") ||
//            anim.GetBool("MakeNoise");
//    }

//    // Returns if hands are busy (it is not possible to make new actions with hands)
//    public bool HandsBusy() {
//        return Busy() ||
//            anim.GetBool("ChangeWeapon") ||
//            anim.GetBool("Attack") ||
//            anim.GetBool("Hit");
//    }

//    // Attack controller
//    public void Attack() {
//        switch (weapons[selectedWeapon].weapon.type) {
//            case WeaponType.Melee:
//                anim.SetBool("Attack", true);
//                SoundManager.instance.Play("Melee2HAttack", audioHands, 0.3f);
//                UpdateStamina(-STAMINA_ATTACK_MELEE2H_SPEND);
//                break;
//            case WeaponType.Range:
//                if (boltReloadProgress >= BOLT_RELOAD_TIME) {
//                    anim.SetBool("Attack", true);
//                    SoundManager.instance.Play("CrossbowAttack", audioHands);
//                    UpdateStamina(-STAMINA_ATTACK_CROSSBOW_SPEND);
//                }
//                break;
//        }
//    }

//    // Callback from fire crossbow animation to fire a bolt
//    private void FireBolt(AnimationEvent evt) {
//        // if (evt.animatorClipInfo.weight > 0.5) {
//        if (boltReloadProgress >= BOLT_RELOAD_TIME) {
//            boltReloadProgress = 0;
//            Vector3 offset = transform.forward * (1.2f +nextState*0.25f) + transform.up * 1.2f + transform.right*0.1f;
//            GameObject b = Instantiate(bolts[bolt], transform.position + offset, transform.rotation);
//            Destroy(b, 5f);
//        }
//    }

//    public void MakeNoise() {
//        if (stamina >= STAMINA_MAKE_NOISE_SPEND) {
//            anim.SetBool("MakeNoise", true);
//            UpdateStamina(-STAMINA_MAKE_NOISE_SPEND);
//            GameObject wave = Instantiate(shockWave, transform.position, Quaternion.identity);
//            Destroy(wave, 5f);
//            Collider[] enemies = Physics.OverlapSphere(transform.position, MAKE_NOISE_RANGE);
//            foreach (Collider enemy in enemies) {
//                if (enemy != null && enemy.gameObject.tag == "Enemy") {
//                    EnemyController ec = enemy.GetComponent<EnemyController>();
//                    ec.SetTarget(transform.position, 999);
//                }
//            }
//        }
//    }

//    public void UseObject() {
//        if (objs[selectedObj] != null && objs[selectedObj].amount > 0) {
//            if (objs[selectedObj].isTrap()) {
//                PutTrap();
//            } else if (objs[selectedObj].isPotion()) {
//                DrinkPotion();
//            } else {
//                Debug.Log("Object type undefined");
//            }
//        }
//    }

//    private void PutTrap() {
//        anim.SetBool("PutTrap", true);
//    }

//    private void DrinkPotion() {

//    }

//    private void PutTrapCallback() {
//        Vector3 offset = transform.forward;

//        if (objs[selectedObj] != null && objs[selectedObj].isTrap() && objs[selectedObj].amount > 0) { 
//            GameObject trap = ((Trap)objs[selectedObj].obj).prefab;

//            if (trap != null) {
//                Instantiate(trap, transform.position + offset, Quaternion.identity);
//                objs[selectedObj].amount--;
//            }
//        }
//    }

//    public void PickTrap() {
//        GameObject trap = GetNearestTrap();
//        if (trap != null) {
//            ITrapController tc = trap.GetComponent<ITrapController>();
//            if (tc != null) {
//                transform.LookAt(trap.transform);
//                trapToPick = tc;
//                anim.SetBool("PickTrap", true);
//            }
//        }
//    }

//    private void PickTrapCallback() {
//        if (trapToPick != null) {
//            trapToPick.PickTrap(this);
//            trapToPick = null;
//        }
//    }

//    public void ChangeSelectedObject(int num) {
//        if (!anim.GetBool("PutTrap")) {
//            selectedObj = selectedObj + num;
//            if (selectedObj < 0) {
//                selectedObj = objs.Length-1;
//            }
//            if (selectedObj >= objs.Length) {
//                selectedObj = 0;
//            }
//            SoundManager.instance.Play("SelectObject", audioHands);
//        }
//    }

//    public void ChangeWeapon() {
//        anim.SetBool("ChangeWeapon", true);
//    }

//    public void ChangeBolt() {
//        bolt = (bolt + 1) % bolts.Length;
//    }

//    // Callback from animations to keep current weapon and draw new weapon
//    private void ChangeWeaponCallback() {
//        UnequipWeapon(selectedWeapon);
//        selectedWeapon = (selectedWeapon + 1) % weapons.Length;
//        EquipWeapon(selectedWeapon);
//    }

//    public void UnequipWeapon(int w) {
//        anim.SetLayerWeight(anim.GetLayerIndex(weapons[w].weapon.type.ToString()), 0f);
//        weapons[w].handObject.SetActive(false);
//        weapons[w].bagObject.SetActive(true);
//    }

//    public void EquipWeapon(int w, bool sound = true) {
//        anim.SetLayerWeight(anim.GetLayerIndex(weapons[w].weapon.type.ToString()), 1f);
//        weapons[w].handObject.SetActive(true);
//        weapons[w].bagObject.SetActive(false);
//        if (sound) SoundManager.instance.Play(weapons[w].weapon.equipSound, audioHands);
//    }


//    // Callback from animations to notify it is finished
//    private void EndAnimation(string animParamName) {
//        anim.SetBool(animParamName, false);
//    }

//    private void SetMeleeAttacking(int state) {
//        meleeAttacking = (state > 0);
//    }

//    public void DamageReceived(float amount) {
//        anim.SetBool("Hit", true);
//        FinishAllActions();

//        Instantiate(blood, transform.position+ new Vector3(0f, 1f, 0f), Quaternion.identity, transform);
//        //GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
//        //text.GetComponent<TextMesh>().text = amount.ToString();

//        life -= amount;
//        if (life <= 0) {
//            Dead();
//        }
//    }

//    private void FinishAllActions() {
//        anim.SetBool("ChangeWeapon", false);
//        anim.SetBool("Attack", false);
//        meleeAttacking = false;
//        anim.SetBool("MakeNoise", false);
//        anim.SetBool("PutTrap", false);
//        anim.SetBool("PickTrap", false);
//        trapToPick = null;
//    }

//    private void Dead() {
//        anim.SetBool("Die", true);
//        StopAllCoroutines();
//        Destroy(GetComponent<Collider>());
//        Destroy(minimap);
//        dead = true;
//    }
    
//    private GameObject GetNearestTrap() {
//        float nearest = PICK_DISTANCE;
//        GameObject nearestTrap = null;
//        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");

//        foreach (GameObject trap in traps) {
//            ITrapController tc = trap.GetComponent<ITrapController>();
//            if (tc != null && tc.Pickable()) {
//                float distance = Vector3.Distance(transform.position, trap.transform.position);
//                if (distance < nearest) {
//                    nearest = distance;
//                    nearestTrap = trap;
//                }
//            }
//        }

//        return nearestTrap;
//    }

//    private void Step(AnimationEvent evt) {
//        if (evt.animatorClipInfo.weight > 0.4) {
//            SoundManager.instance.Play("Step", audioFoots);
//        }
//    }

//    public void EnableSound(bool sound) {
//        AudioSource[] audioSources = GetComponents<AudioSource>();
//        foreach (AudioSource audioSource in audioSources) {
//            audioSource.enabled = sound;
//        }
//    }

//    public void EnableListen(bool listen) {
//        GetComponent<AudioListener>().enabled = listen;
//    }


//}

