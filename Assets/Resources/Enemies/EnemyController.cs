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
    //public GameObject floatingText;
    public GameObject blood;
    public GameObject minimap;

    public GameObject ragdoll;

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
            self.IsSeeingPlayer(player);
        } else {
            if (cyclesSearching > 0 && agent.remainingDistance > agent.stoppingDistance) {
                cyclesSearching--;
                self.IsSearchingPlayer(cyclesSearching);
            } else {
                SetTarget(church.transform.position, 0);
                self.IsGoingToChurch();
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
            if (agent.remainingDistance > agent.stoppingDistance && !FootBusy()) {
                self.IsMoving();
            } else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
                if (Vector3.Distance(player.transform.position, transform.position) <= agent.stoppingDistance || 
                    Vector3.Distance(church.transform.position, transform.position) <= agent.stoppingDistance) {
                    self.IsArrived(agent.destination);
                }
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

    public void Trapped(Vector3 position) {
        anim.SetBool("Trap", true);
        DamageReceived(2);
        transform.position = position;
    }

    // Enemy receive damage
    public void DamageReceived(float amount) {
        self.IsDamaged(amount);
        anim.SetBool("Hit", true);

        Instantiate(blood, transform.position+ new Vector3(0f, 1f, 0f), Quaternion.identity, transform);
        //GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        //text.GetComponent<TextMesh>().text = amount.ToString();

        self.life -= amount;
        if (self.life <= 0) {
            Dead();
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
        

        /*
        GameObject rd = Instantiate(ragdoll, transform.position, transform.rotation);
        Rigidbody[] rbs = rd.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbs) {
            //rb.AddForce(-rb.transform.forward * 26f, ForceMode.Impulse);
            rb.AddForceAtPosition(-rd.transform.forward * 15f, -rd.transform.forward * 15f, ForceMode.Impulse);
            //rb.AddExplosionForce(50f, transform.position, 15f);
        }
        */

        Destroy(gameObject);
    }

    // Rerturns if enemy is busy (can't make any action)
    public bool Busy() {
        return anim.GetBool("Attack") ||
            anim.GetBool("Hit") ||
            anim.GetBool("Die");
    }

    public bool FootBusy() {
        return anim.GetBool("Trap");
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
