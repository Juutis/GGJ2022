using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetEntityNavigation : MonoBehaviour
{
    private NavMeshAgent agent;

    private float refreshTimer = 0;
    private float refreshInterval = 0.5f;

    private float distance = 20f;

    private TargetEntity host;

    private TargetEntity currentTarget;

    public TargetEntity CurrentTarget { get { return currentTarget; } }

    private Vector3 currentDestination;

    [SerializeField]
    private bool wander = false;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        host = GetComponent<TargetEntity>();
        refreshTimer = refreshInterval;
    }

    public void SetTarget(TargetEntity newTarget)
    {
        currentTarget = newTarget;
    }

    public void ClearTarget()
    {
        refreshTimer = 0f;
        currentTarget = null;
    }

    void Update()
    {
        if (currentTarget != null)
        {
            //refreshTimer += Time.deltaTime;
            //if (refreshTimer >= refreshInterval) {
            agent.SetDestination(currentTarget.transform.position);
//            Debug.Log($"Target for {host} is now {currentTarget}");
            //}
        }
        else if (wander)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                refreshTimer += Time.deltaTime;
                if (refreshTimer >= refreshInterval)
                {
                    Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
                    randomDirection += host.Position;
                    NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, distance, -1);
                    agent.SetDestination(navHit.position);
                    //Debug.Log($"Random wander pos for {host} is now {navHit.position}");
                    refreshTimer = 0f;
                }
            }
        }
    }

}
