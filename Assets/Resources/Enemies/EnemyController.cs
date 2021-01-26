using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    // Time for refresh pathfinding
    private float SEARCH_TIME = 1f;
    // Navmesh speed coeficient conversor
    private float NAVMESH_SPEED_COEFICIENT = 3.5f;

    // Script for specific behavior 
    private IEnemy self;
    public Animator anim;
    private Collider collider;
    private NavMeshAgent agent;
    private GameObject church;
    private GameObject player;
    // Enemy weapon for attack
    public GameObject weapon;
    // Enemy body part which weapon is attached
    public GameObject weaponGrip;
    public GameObject floatingText;
    public GameObject minimap;

    public Vector3 target;
    public bool dead = false;
    public bool meleeAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<IEnemy>();
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        church = GameObject.Find("ChurchDoor");
        player = GameObject.FindWithTag("Player");

        StartCoroutine(FindTarget());
    }

    // Update is called once per frame
    void Update() {
        WeaponPosition();
        Decision();
    }

    // Assign target to NavMeshAgent
    public IEnumerator FindTarget() {
        target = self.GetTarget(player, church);
        if (target != Vector3.zero) {
            agent.SetDestination(target);
        }

        yield return new WaitForSeconds(SEARCH_TIME);
        StartCoroutine(FindTarget());
    }

    // Attach weapons to character movements
    private void WeaponPosition() {
        weapon.transform.position = weaponGrip.transform.position;
        weapon.transform.rotation = weaponGrip.transform.rotation;
    }

    // Main flow for enemy action decision
    private void Decision() {
        if (!dead && !Busy()) {
            if (agent.remainingDistance > agent.stoppingDistance) {
                self.IsMoving();
            } else {
                self.IsArrived();
            }
        } else if (dead) {
            
        } else if (!Busy()) {
            agent.isStopped = true;
        } else {
            agent.isStopped = true;
        }
    }

    // Enemy move
    public void Move() {
        agent.isStopped = false;
        anim.SetBool("Walk", true);
        agent.speed = self.speed / NAVMESH_SPEED_COEFICIENT;
    }

    // Enemy attack impacts on a player
    public void AttackImpact(Collider collider) {
        HunterController player = collider.GetComponent<HunterController>();
        player.DamageReceived(self.meleeDamage);
    }

    // Enemy receive damage
    public void DamageReceived(float amount) {
        anim.SetBool("Hit", true);

        GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        //GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().text = amount.ToString();

        self.life -= amount;
        if (self.life <= 0) {
            Dead();
            //dead = true;
        }
    }

    // Enemy die
    private void Dead() {
        anim.SetBool("Die", true);
        meleeAttacking = false;
        StopAllCoroutines();
        Destroy(collider);
        Destroy(agent);
        Destroy(minimap);
        dead = true;
    }

    // Rerturns if enemy is busy (can't make any action)
    public bool Busy() {
        return anim.GetBool("Attack") ||
            anim.GetBool("Hit") ||
            anim.GetBool("Die");
    }

    // Callback from animations to notify it is finished
    private void EndAnimation(string animParamName) {
        anim.SetBool(animParamName, false);
    }

    // Callback from animation attack when enemy is attacking
    public void SetMeleeAttacking(int state) {
        meleeAttacking = (state > 0); 
    }
}
