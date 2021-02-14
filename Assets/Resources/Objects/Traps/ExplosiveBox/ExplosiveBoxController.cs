using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBoxController : MonoBehaviour, ITrapController
{
    private int TRAP_ID = 1;
    private float EXPLOSION_RANGE = 15f;
    private float EXPLOSION_DAMAGE = 5f;

    Collider col;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Pickable() {
        return true;
    }

    public void PickTrap(HunterController hc) {
        if (Pickable()) {
            hc.objs[TRAP_ID].amount++;
            Destroy(gameObject);
        }
    }

    public void Explosion() {
        GameObject explosionEffect = Instantiate(explosion, transform.position, Quaternion.identity);
        SoundManager.instance.PlayAndDestroy("Explosion", transform.position);
        Collider[] enemies = Physics.OverlapSphere(transform.position, EXPLOSION_RANGE);
        foreach (Collider enemy in enemies) {
            if (enemy != null && enemy.gameObject.tag == "Enemy") {
                EnemyController ec = enemy.GetComponent<EnemyController>();
                ec.DamageReceived(EXPLOSION_DAMAGE, transform.position);
            }
        }
        Destroy(explosionEffect, 5f);
        Destroy(gameObject);
    }

}
