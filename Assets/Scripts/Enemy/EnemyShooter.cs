using UnityEngine;
using UnityEngine.AI;
using Game.Utils;
public class EnemyShooter : MonoBehaviour
{
    [Header("Shooter Specs")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float patrolWaitTime = 2f;
    public float chaseWaitTime = 5f;
    public float patrolRange = 10f;
    public float chaseRange = 5f;
    public float shootingRange = 3f; // Distância para começar a atirar
    public float timeBetweenShots = 2f; // Tempo entre disparos
    public GameObject bulletPrefab; // Prefab do projétil
    public Transform firePoint;

    private NavMeshAgent nav;
    private Transform player;
    private float patrolTimer;
    private float chaseTimer;
    private Vector3 randomDestination;
    private ObjectPooler objectPooler;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        objectPooler = new ObjectPooler(bulletPrefab, 10);
        SetRandomDestination();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < chaseRange)
        {
            ChasePlayer();
            if (Vector3.Distance(transform.position, player.position) < shootingRange)
            {
                // Adicione a lógica de atirar aqui
                Shoot();
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        nav.speed = patrolSpeed;

        if (nav.remainingDistance < nav.stoppingDistance || patrolTimer >= patrolWaitTime)
        {
            patrolTimer = 0f;
            SetRandomDestination();
        }
        else
        {
            patrolTimer += Time.deltaTime;
        }
    }

    void SetRandomDestination()
    {
        float randomRadius = Random.Range(5f, patrolRange);
        Vector3 randomDirection = Random.insideUnitSphere * randomRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, randomRadius, NavMesh.AllAreas);

        randomDestination = hit.position;
        nav.SetDestination(randomDestination);
    }

    void ChasePlayer()
    {
        nav.speed = chaseSpeed;

        nav.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) > chaseRange)
        {
            chaseTimer += Time.deltaTime;

            if (chaseTimer >= chaseWaitTime)
            {
                patrolTimer = 0f;
                SetRandomDestination();
                chaseTimer = 0f;
            }
        }
        else
        {
            chaseTimer = 0f;
        }
    }
    
    void Shoot()
    {
        if (Time.time > timeBetweenShots)
        {
            SpawnProjectile(firePoint.position, firePoint.rotation);
            timeBetweenShots = Time.time + 1f / timeBetweenShots;
        }
    }
    
    private void SpawnProjectile(Vector2 position, Quaternion rotation)
    {
        GameObject projectile = objectPooler.GetPooledObject();

        if (projectile != null)
        {
            projectile.transform.position = position;
            projectile.transform.rotation = rotation;
            projectile.SetActive(true);
        }
    }
}
