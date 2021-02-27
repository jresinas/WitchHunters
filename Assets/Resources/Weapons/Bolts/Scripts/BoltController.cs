using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltController : MonoBehaviour
{
    // 0: normal, 1: fire, 2: poison
    public int boltType;
    float damage = 1f;
    float speed = 40f;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + Vector3.forward * speed * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Enemy") {
            EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();
            enemy.DamageReceived(damage, transform.position);

            DestroyBolt();
        }
        if (collider.gameObject.tag == "Interactive" && boltType == 1) {
            //ITrapController tc = collider.GetComponent<ITrapController>();
            //if (tc is ExplosiveBoxController) {
            ExplosiveBoxController ebc = collider.GetComponent<ExplosiveBoxController>();
            if (ebc != null) {
                //ExplosiveBoxController ebc = (ExplosiveBoxController)tc;
                ebc.Explosion();
                DestroyBolt();
            }
        }
        if (collider.gameObject.tag == "House" || collider.gameObject.tag == "Church") {
            DestroyBolt();
        }
    }

    private void DestroyBolt() {
        if (transform.childCount > 0) {
            DetachParticlesChild();
        }
        Destroy(gameObject);
    }


    // If arrow has particles (fire arrow, poison arrow, etc), destroy parent but keep particle alive
    // Particles are into an empty object "ParticleWrapper" to keep the particles right size
    private void DetachParticlesChild() {
        Transform wrapper = transform.GetChild(0);

        if (wrapper != null) {
            ParticleSystem particle = wrapper.GetComponentInChildren<ParticleSystem>();

            if (particle != null) {
                particle.enableEmission = false;
                wrapper.transform.parent = null;
                Destroy(wrapper.gameObject, 5f);
            }
        }
    }
}
