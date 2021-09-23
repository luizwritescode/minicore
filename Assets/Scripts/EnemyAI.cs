using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ShootableEntity))]
public class EnemyAI : MonoBehaviour
{
    public ShootableEntity shootableEnt;

    public Transform player;
    public Transform gunEnd;

    public LayerMask whatIsGround, whatIsPlayer;

    //AI Controllers
    public NavMeshAgent agent;

    [SerializeField]
    Transform destination;
    

    //Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public string currentState = "off";


    //VFX
    ParticleSystem arrivalDust;

    private void Awake()
    {
        shootableEnt = GetComponent<ShootableEntity>();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        currentState = "on";
    }
    // Start is called before the first frame update
    void Start()
    {   
        agent = this.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("nav mesh agent component is not attached to " + gameObject.name);
        }
        

        arrivalDust = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

    }

    private void Patrolling()
    {
        currentState = "patrolling";

        if(!walkPointSet) SearchWalkPoint();

        if (walkPointSet && agent.isOnNavMesh) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        currentState = "searching for next destination";
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //How does this Raycast validate the walk point? I have no clue
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        currentState = "chasing";
        agent.SetDestination(player.position); 
    }

    private void AttackPlayer()
    {
        

        //Make sure enemy doesnt move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            currentState = "attacking";

            //Attack code here
            Rigidbody rb = Instantiate(projectile, gunEnd.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            //End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        currentState = "waiting for next attack";
    }

    private void ResetAttack() { alreadyAttacked = false; }

    private void DestroyEnemy()
    {
        currentState = "dead";
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    public bool IsAttacking() 
    { 
        if(currentState == "attacking" ) return true;
        else return false;
    }
}
