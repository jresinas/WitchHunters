using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float MAX_LIFE = 10f;
    public float MAX_STAMINA = 10f;
    private float STAMINA_DEFAULT_RECOVER = 0.35f;
    private float STAMINA_MAKE_NOISE_SPEND = 1f;
    private float MAKE_NOISE_RANGE = 15f;

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

    public float life;
    public float stamina;

    public bool dead = false;

    void Awake() {
        DontDestroyOnLoad(gameObject);

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
        UpdateStamina(STAMINA_DEFAULT_RECOVER, true);
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