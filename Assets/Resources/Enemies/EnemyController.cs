using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    // Time for refresh pathfinding
    private float SEARCH_TIME = 0.5f;
    // Navmesh speed coeficient conversor
    private float NAVMESH_SPEED_COEFICIENT = 3.5f;

    private int cyclesSearching = 0;

    // Script for specific behavior 
    private IEnemy self;
    public Animator anim;
    private Collider col;
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
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        church = GameObject.Find("ChurchDoor");
        player = GameObject.FindWithTag("Player");

        StartCoroutine(FindPath());
    }

    // Update is called once per frame
    void Update() {
        WeaponPosition();
        if (!dead) {
            Decision();
        } else {
            meleeAttacking = false;
        }
    }

    // Assign target to NavMeshAgent
    public IEnumerator FindPath() {
        FindTarget();

        yield return new WaitForSeconds(SEARCH_TIME);
        StartCoroutine(FindPath());
    }

    private void FindTarget() {
        if (InLineOfSight(player)) {
            SetTarget(player.transform.position, self.CYCLES_SEARCHING);
        } else {
            if (cyclesSearching > 0 && agent.remainingDistance > agent.stoppingDistance) {
                cyclesSearching--;
            } else {
                SetTarget(church.transform.position, 0);
            }
        }
    }

    private bool InLineOfSight(GameObject obj) {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, obj.transform.position - transform.position, out hit)) {
            if (hit.collider.gameObject == player) {
                return true;
            }
        }
        return false;
    }

    public void SetTarget(Vector3 target, int cycles) {
        agent.SetDestination(target);
        if (cycles >= 0) {
            cyclesSearching = cycles;
        }
    }

    // Attach weapons to character movements
    private void WeaponPosition() {
        weapon.transform.position = weaponGrip.transform.position;
        weapon.transform.rotation = weaponGrip.transform.rotation;
    }

    // Main flow for enemy action decision
    private void Decision() {
        if (!Busy()) {
            if (agent.remainingDistance > agent.stoppingDistance) {
                self.IsMoving();
            } else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
                //if (agent.destination == player.transform.position || agent.destination == church.transform.position) {
                    self.IsArrived(agent.destination);
                //}
            }
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
        StopAllCoroutines();
        Destroy(col);
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
