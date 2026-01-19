using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public NavMeshAgent agent;
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public int damage = 10;

    void Start()
    {
        currentHP = maxHP;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
        {
            agent.SetDestination(player.position);
        }
        else if (distanceToPlayer <= attackRange)
        {
            agent.Stop();
            Attack();
        }
    }

    void Attack()
    {
        Debug.Log("Boss attacks the player for " + damage + " damage!");
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log(gameObject.name + " took " + amount + " damage. HP: " + currentHP);

        if (IsDead())
        {
            Die();
        }
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    void Die()
    {
        Debug.Log("You won! The boss has been defeated!");
        Destroy(gameObject);
    }
}