using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour, IObject
{
    Collider col;
    Animator anim;
    bool ready = true;
    GameObject enemyTrapped;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider) {
        if (ready && collider.gameObject.tag == "Enemy") {
            ready = false;
            anim.SetBool("Active", true);
            EnemyController ec = collider.GetComponent<EnemyController>();
            ec.Trapped(transform.position);
            enemyTrapped = collider.gameObject;
        }
    }


    public bool Pickable() {
        return enemyTrapped == null || enemyTrapped.GetComponent<EnemyController>().dead;
    }

    public void PickObject(HunterController hc) {
        //hc.traps.Add("BearTrap");
        hc.trapsNumber[0]++;
        Destroy(gameObject);
    }


}
