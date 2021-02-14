using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : MonoBehaviour
{
    private HunterController hc;
    private float damage = 3f;

    // Start is called before the first frame update
    void Start()
    {
        hc = GetComponentInParent<HunterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Enemy" && hc.meleeAttacking) {
            EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();
            enemy.DamageReceived(damage, transform.position);
            SoundManager.instance.Play("Melee2HImpact", hc.audioHands);
        }
    }
}
