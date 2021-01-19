using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : MonoBehaviour
{
    private HunterController hc;
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
            EnemyController ec = collider.gameObject.GetComponent<EnemyController>();
            ec.Damage(2);
        }
    }
}
