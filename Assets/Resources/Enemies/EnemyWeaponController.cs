using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    private EnemyController ec;
    // Start is called before the first frame update
    void Start()
    {
        ec = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "ChurchDoor" && ec.meleeAttacking) {
            ec.AttackImpact(collider);
        }
    }
}
