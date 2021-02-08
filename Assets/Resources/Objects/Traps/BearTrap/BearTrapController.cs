using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrapController : MonoBehaviour, ITrapController
{
    private int TRAP_ID = 0;

    Collider col;
    Animator anim;
    AudioSource audioSource;
    bool ready = true;
    GameObject enemyTrapped;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider) {
        if (ready && collider.gameObject.tag == "Enemy") {
            SoundManager.instance.Play("BearTrap", audioSource);
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

    public void PickTrap(HunterController hc) {
        //hc.traps.Add("BearTrap");
        if (Pickable()) {
            hc.objs[TRAP_ID].amount++;
            Destroy(gameObject);
        }
    }


}
