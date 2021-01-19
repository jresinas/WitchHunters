using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    float life = 5;
    private Animator anim;
    private Collider collider;
    public bool dead = false;
    public bool busy = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float amount) {
        busy = true;
        anim.SetBool("Hit", true);
        life -= amount;
        if (life <= 0) {
            Dead();
        }
    }

    private void Dead() {
        anim.SetBool("Die", true);
        Destroy(collider);
        dead = true;
    }

    private void EndAnimation(string animParamName) {
        anim.SetBool(animParamName, false);
        busy = false;
    }
}
