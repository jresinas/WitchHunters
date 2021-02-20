using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : MonoBehaviour
{
    private HunterController hc;
    private WeaponsController wc;
    private float damage = 3f;

    // Start is called before the first frame update
    void Start()
    {
        hc = GetComponentInParent<HunterController>();
        wc = GetComponentInParent<WeaponsController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Enemy" && wc.meleeAttacking) {
            EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();
            enemy.DamageReceived(damage, transform.position);
            SoundManager.instance.Play("Melee2HImpact", hc.audioHands);
        }
    }
}
