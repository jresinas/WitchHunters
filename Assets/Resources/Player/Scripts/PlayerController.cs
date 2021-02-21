using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float MAX_LIFE = 10f;
    public float MAX_STAMINA = 10f;
    private float STAMINA_DEFAULT_RECOVER = 0.35f;
    private float STAMINA_WALK_SPEND = 0.1f; 
    private float STAMINA_RUN_SPEND = 1f;
    private float STAMINA_MAKE_NOISE_SPEND = 1f;
    private float MAKE_NOISE_RANGE = 15f;

    Rigidbody rb;
    Animator anim;
    PlayerWeaponController wc;
    PlayerObjectController oc;
    // Audio source for foots noises
    public AudioSource audioFoots;
    // Audio source for no-foots noises
    public AudioSource audioHands;
    // Object which represent player in minimap
    public GameObject minimap;
    // Blood particle effect
    [SerializeField] GameObject blood;
    // Make noise wave particle effect
    [SerializeField] GameObject shockWave;

    private float walkSpeed = 4f;
    private float runSpeed = 8f;
    public float life;
    public float stamina;

    public bool dead = false;

    // Next movement state (0:idle, 1:walk, 2:run)
    public float nextState = 0f;


    void Awake() {
        DontDestroyOnLoad(gameObject);

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        wc = GetComponent<PlayerWeaponController>();
        oc = GetComponent<PlayerObjectController>();
        
        life = MAX_LIFE;
        stamina = MAX_STAMINA;
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void FixedUpdate() {
        if (!dead) {
            ChangeState();
            UpdateStamina(STAMINA_DEFAULT_RECOVER, true);
        }   
    }

    public void UpdateStamina(float value, bool continuous = false) {
        stamina += continuous ? value * Time.deltaTime : value;
        if (stamina > MAX_STAMINA) {
            stamina = MAX_STAMINA;
        }
        if (stamina < 0) {
            stamina = 0;
        }
    }

    public void Idle() {
        nextState = 0f;
    }

    // Movement and rotation controller
    public void Move(Vector3 move, Vector3 view, bool run) {
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

        if (move != Vector3.zero) {
            if (!run || (nextState == 2 && stamina <= 0.1) || (nextState != 2 && stamina < STAMINA_RUN_SPEND/4)) {
                // Walk
                nextState = 1f;
                rb.MovePosition(rb.position + move * walkSpeed * Time.deltaTime);
                UpdateStamina(STAMINA_WALK_SPEND, true);
            } else {
                // Run
                nextState = 2f;
                rb.MovePosition(rb.position + move * runSpeed * Time.deltaTime);
                UpdateStamina(-STAMINA_RUN_SPEND, true);
            }
        } else {
            // Idle
            Idle();
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
    public bool Busy() {
        return anim.GetBool("PutTrap") ||
            anim.GetBool("PickTrap") ||
            anim.GetBool("MakeNoise");
    }

    // Returns if hands are busy (it is not possible to make new actions with hands)
    public bool HandsBusy() {
        return Busy() ||
            anim.GetBool("ChangeWeapon") ||
            anim.GetBool("Attack") ||
            anim.GetBool("Hit");
    }

    public void MakeNoise() {
        if (stamina >= STAMINA_MAKE_NOISE_SPEND) {
            anim.SetBool("MakeNoise", true);
            UpdateStamina(-STAMINA_MAKE_NOISE_SPEND);
            GameObject wave = Instantiate(shockWave, transform.position, Quaternion.identity);
            Destroy(wave, 5f);
            Collider[] enemies = Physics.OverlapSphere(transform.position, MAKE_NOISE_RANGE);
            foreach (Collider enemy in enemies) {
                if (enemy != null && enemy.gameObject.tag == "Enemy") {
                    EnemyController ec = enemy.GetComponent<EnemyController>();
                    ec.Lure(transform.position);
                }
            }
        }
    }



    // Callback from animations to notify it is finished
    private void EndAnimation(string animParamName) {
        anim.SetBool(animParamName, false);
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
        wc.StopChangeWeapon();
        wc.StopAttack();
        oc.StopPutTrap();
        oc.StopPickTrap();
        anim.SetBool("MakeNoise", false);
        
    }

    private void Dead() {
        anim.SetBool("Die", true);
        StopAllCoroutines();
        Destroy(GetComponent<Collider>());
        Destroy(minimap);
        dead = true;
    }

    private void Step(AnimationEvent evt) {
        if (evt.animatorClipInfo.weight > 0.4) {
            SoundManager.instance.Play("Step", audioFoots);
        }
    }

    public void EnableSound(bool sound) {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        foreach (AudioSource audioSource in audioSources) {
            audioSource.enabled = sound;
        }
    }

    public void EnableListen(bool listen) {
        GetComponent<AudioListener>().enabled = listen;
    }


}

