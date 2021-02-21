using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour {
    public float BOLT_RELOAD_TIME = 1f;
    private float STAMINA_ATTACK_MELEE2H_SPEND = 0.5f;
    private float STAMINA_ATTACK_CROSSBOW_SPEND = 0.1f;

    Animator anim;
    PlayerController pc;

    // Empty object in the character back to keep unused weapons
    public GameObject bag;
    // Character right hand
    public GameObject rightHand;

    public bool meleeAttacking = false;
    // List of available weapons
    public PlayerWeapon[] weapons;
    // Current equiped weapon
    public int selectedWeapon = 0;
    // List of available bolts
    public PlayerBolt[] bolts;
    // Current bolt selected
    int selectedBolt = 0;
    // Time elapsed since starting reload bolt (in seconds) 
    public float boltReloadProgress;

    private void Awake() {
        pc = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        boltReloadProgress = BOLT_RELOAD_TIME;
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        WeaponsPosition();
        BoltReload();
    }

    public void ChangeWeapon() {
        anim.SetBool("ChangeWeapon", true);
    }

    public void ChangeBolt() {
        selectedBolt = (selectedBolt + 1) % bolts.Length;
    }

    // Attach weapons to character movements
    private void WeaponsPosition() {
        foreach (PlayerWeapon pw in weapons) {
            pw.handObject.transform.position = rightHand.transform.position;
            pw.bagObject.transform.position = bag.transform.position;
        }
    }

    // Callback from animations to keep current weapon and draw new weapon
    private void ChangeWeaponCallback() {
        UnequipWeapon(selectedWeapon);
        selectedWeapon = (selectedWeapon + 1) % weapons.Length;
        EquipWeapon(selectedWeapon);
    }

    public void UnequipWeapon(int w) {
        anim.SetLayerWeight(anim.GetLayerIndex(weapons[w].weapon.type.ToString()), 0f);
        weapons[w].handObject.SetActive(false);
        weapons[w].bagObject.SetActive(true);
    }

    public void EquipWeapon(int w, bool sound = true) {
        anim.SetLayerWeight(anim.GetLayerIndex(weapons[w].weapon.type.ToString()), 1f);
        weapons[w].handObject.SetActive(true);
        weapons[w].bagObject.SetActive(false);
        if (sound) SoundManager.instance.Play(weapons[w].weapon.equipSound, pc.audioHands);
    }

    private void SetMeleeAttacking(int state) {
        meleeAttacking = (state > 0);
    }

    // Callback from fire crossbow animation to fire a bolt
    private void FireBolt(AnimationEvent evt) {
        // if (evt.animatorClipInfo.weight > 0.5) {
        if (boltReloadProgress >= BOLT_RELOAD_TIME && bolts[selectedBolt].amount > 0) {
            boltReloadProgress = 0;
            Vector3 offset = transform.forward * (1.2f + pc.nextState * 0.25f) + transform.up * 1.2f + transform.right * 0.1f;
            GameObject b = Instantiate(bolts[selectedBolt].bolt.prefab, transform.position + offset, transform.rotation);
            bolts[selectedBolt].amount--;
            Destroy(b, 5f);
        }
    }

    // Attack controller
    public void Attack() {
        switch (weapons[selectedWeapon].weapon.type) {
            case WeaponType.Melee:
                anim.SetBool("Attack", true);
                SoundManager.instance.Play("Melee2HAttack", pc.audioHands, 0.3f);
                pc.UpdateStamina(-STAMINA_ATTACK_MELEE2H_SPEND);
                break;
            case WeaponType.Range:
                if (boltReloadProgress >= BOLT_RELOAD_TIME && bolts[selectedBolt].amount > 0) {
                    anim.SetBool("Attack", true);
                    SoundManager.instance.Play("CrossbowAttack", pc.audioHands);
                    pc.UpdateStamina(-STAMINA_ATTACK_CROSSBOW_SPEND);
                }
                break;
        }
    }

    private void BoltReload() {
        if (boltReloadProgress < BOLT_RELOAD_TIME) {
            boltReloadProgress += Time.deltaTime;
        }
    }

    public void StopChangeWeapon() {
        anim.SetBool("ChangeWeapon", false);
    }

    public void StopAttack() {
        anim.SetBool("Attack", false);
        meleeAttacking = false;
    }
}
