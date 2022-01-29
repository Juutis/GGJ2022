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

    private bool alive = true;

    private CharacterAnimator charAnim;
    private MeleeFighter melee;
    private RangedFighter ranged;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        host = GetComponent<TargetEntity>();
        charAnim = GetComponentInChildren<CharacterAnimator>();
        refreshTimer = refreshInterval;

        melee = GetComponent<MeleeFighter>();
        ranged = GetComponent<RangedFighter>();
    }

    public void SetTarget(TargetEntity newTarget)
    {
        currentTarget = newTarget;
        if (ranged != null) ranged.SetTarget(newTarget);
        if (melee != null) melee.SetTarget(newTarget);
    }

    public void ClearTarget()
    {
        refreshTimer = 0f;
        currentTarget = null;
        if (ranged != null) ranged.SetTarget(null);
        if (melee != null) melee.SetTarget(null);
    }

    public void Kill() {
        alive = false;
        agent.enabled = false;
    }

    void Update()
    {
        if (!alive) return;

        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.transform.position);
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

        if(agent) {
            if(agent.velocity.sqrMagnitude > 0.1f) {
                charAnim.SetMoving(true);
            } else {
                charAnim.SetMoving(false);
            }
        }
    }

}
