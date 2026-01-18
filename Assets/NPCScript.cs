using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float stopDistance = 3f;
    public float reachThreshold = 0.5f;
    public float wanderTimer = 5f;
    public float rotationSpeed = 5f;
    public int health;

    public string questID;
    

    private Transform player;
    private NavMeshAgent agent;
    private bool hasDestination;
    private float timer;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        hasDestination = false;
        timer = wanderTimer;
    }

    void Update()
    {
        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Only react if NPC has a quest
        bool canGiveQuest = !string.IsNullOrEmpty(questID);

        if (canGiveQuest && distanceToPlayer <= stopDistance)
        {
            agent.isStopped = true;
            FacePlayer();
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        // Wandering timer
        timer += Time.deltaTime;

        if ((!agent.pathPending && agent.remainingDistance <= reachThreshold) || timer >= wanderTimer)
        {
            hasDestination = false;
            timer = 0f;
        }

        if (!hasDestination)
        {
            SetNewDestination();
        }
    }

    void SetNewDestination()
    {
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
        hasDestination = true;
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude == 0f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist + origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
