using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : MonoBehaviour
{
    private PlayerController pc;
    private PlayerWeaponController wc;
    private float damage = 3f;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponentInParent<PlayerController>();
        wc = GetComponentInParent<PlayerWeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Enemy" && wc.meleeAttacking) {
            EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();
            enemy.DamageReceived(damage, transform.position);
            SoundManager.instance.Play("Melee2HImpact", pc.audioHands);
        }
    }
}
