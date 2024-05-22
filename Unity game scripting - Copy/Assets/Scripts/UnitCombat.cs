using UnityEngine;
using UnityEngine.AI;

public class UnitCombat : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Attack")]
    public float attackRange = 10f;
    public float attackRate = 1f;
    public float bulletSpeed = 20f;
    public float damage = 10f;
    public GameObject bulletPrefab;
    public bool canMoveWhileShooting = true; // Determines if the unit can move while shooting

    [Header("Movement")]
    public float detectionRange = 20f; // Range within which enemies detect units
    public float circleDistance = 5f; // Distance at which enemies circle their targets
    public float circleSpeed = 3f; // Speed at which enemies circle their targets
    private NavMeshAgent navMeshAgent;
    private Transform target;

    private float nextAttackTime = 0f;
    private float lastAttackTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (CompareTag("Unit"))
        {
            FindTarget();
            if (target != null)
            {
                if (canMoveWhileShooting || (navMeshAgent != null && navMeshAgent.velocity.magnitude == 0))
                {
                    AttackTarget();
                }
            }
        }
        else if (CompareTag("Enemy"))
        {
            FindAndMoveToTarget();
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.position) <= attackRange)
                {
                    if (!canMoveWhileShooting)
                    {
                        if (navMeshAgent != null)
                        {
                            navMeshAgent.isStopped = true;
                        }
                    }
                    else
                    {
                        CircleTarget();
                    }
                    AttackTarget();
                }
                else
                {
                    if (navMeshAgent != null)
                    {
                        navMeshAgent.isStopped = false;
                    }
                }
            }
            else
            {
                if (Time.time - lastAttackTime > 2f)
                {
                    if (navMeshAgent != null)
                    {
                        navMeshAgent.isStopped = false;
                    }
                }
            }
        }
    }

    void FindTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                target = hitCollider.transform;
                return;
            }
        }
        target = null;
    }

    void FindAndMoveToTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        float shortestDistance = Mathf.Infinity;
        Transform nearestUnit = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Unit"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestUnit = hitCollider.transform;
                }
            }
        }

        if (nearestUnit != null && shortestDistance <= detectionRange)
        {
            target = nearestUnit;
            if (navMeshAgent != null)
            {
                navMeshAgent.SetDestination(target.position);
            }
        }
        else
        {
            target = null;
        }
    }

    void CircleTarget()
    {
        if (target != null && navMeshAgent != null)
        {
            Vector3 direction = (transform.position - target.position).normalized;
            Vector3 circlePoint = target.position + direction * circleDistance;
            navMeshAgent.SetDestination(circlePoint);
            navMeshAgent.speed = circleSpeed;
        }
    }

    void AttackTarget()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / attackRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (target != null && bulletPrefab != null)
        {
            lastAttackTime = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Seek(target, damage, bulletSpeed);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
