using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : Unit
{

    private enum State
    {
        Idle,
        Patrolling,
        Chasing
    }

    public float attackInterval = 0.5f;
    public float lookDistance = 10;//our AI can see 10 units away
    private State currentState; //this keeps track of the current state
    private NavMeshAgent agent; //this is our navmesh agent
    private Unit currentEnemy; //current enemy
    private AISpot currentSpot; //the current outpost we are focused on
    [SerializeField] GameObject goalObject;


    // Start is called before the first frame update
    void Start()
    {
        //base.Start();
        agent = GetComponent<NavMeshAgent>();
        respawnPos = this.transform.position; //Change this to the checkpoint mechanic
        SetState(State.Idle);

    }

    private void SetState(State newState)
    {
        //what we want to do here is look at the newstater, compare it to the enumvalues, and then figure out what to do based on that.
        //set state will only be called when a state changes
        currentState = newState;
        StopAllCoroutines();//stop the previous coroutines so they aren't operating at the same time
        switch (currentState)
        {
            case State.Idle:
                StartCoroutine(OnIdle());
                //do some work
                break;
            case State.Patrolling:
                StartCoroutine(OnPatrolling());
                //do some work
                break;
            case State.Chasing:
                StartCoroutine(OnChasing());
                //do some work
                break;
            default:
                break;
        }
        ///
    }

    private IEnumerator OnIdle() //handles our idle state
    {
        //when idling, we should probably do some work and look for an outpost
        currentSpot = null;
        while (currentSpot == null)
        {
            if (isAlive)
                LookForSpots(); //if we ever find an outpost, and the currentSpot changes, we will leave this loop
            yield return null;
        }
        SetState(State.Patrolling); //we found an outpost, we now need to move
        //this will change
    }
    private IEnumerator OnPatrolling()
    {
        agent.SetDestination(currentSpot.transform.position);
        if (currentSpot.currentValue == 1)
        {
            LookForSpots();
            yield return null;

        }
        //while (!(currentSpot.team == Team && currentSpot.currentValue == 1))
        //{
        //    //look for enemies
        //    LookForEnemies();
        //}//we move towards an outpost as long as we are not the team possessing it
        //currentSpot = null;
        //SetState(State.Idle);
    }
    private IEnumerator OnChasing()
    {
        ////we have to reset the path of our agent
        //agent.ResetPath();
        //float shootTimer = 0;
        //while (currentEnemy.isAlive)
        //{
        //    shootTimer += Time.deltaTime; //increment our shoot timer each time
        //    float distanceToEnemy = Vector3.Distance(currentEnemy.transform.position, this.transform.position);
        //    //if we are too far away or we can't see our enemy, let's move towards them
        //    //otherwise, if our shoot timer is up, shoot them
        //    if (distanceToEnemy > lookDistance || !CanSee(currentEnemy.transform, currentEnemy.transform.position + aimOffset))
        //    {
        //        agent.SetDestination(currentEnemy.transform.position);
        //    }
        //    else if (shootTimer > shootInterval)
        //    {
        //        agent.ResetPath();
        //        shootTimer = 0;
        //        Vector3 dir = currentEnemy.transform.position - this.transform.position;
        //        dir.Normalize();

        //        LayerMask mask = ~LayerMask.GetMask("Outpost", "Terrain");
        //        Ray ray = new Ray(GetEyesPosition(), dir); //aim our ray in the direction that we are looking
        //        RaycastHit hit; //our hit is going to be used as an output of a Raycast
        //                        //so we need to use a layermask and a layermask is 
        //        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        //        {
        //            //if this is true, we hit something
        //            ShootAt(hit);
        //        }
        //        else
        //        {
        //            Vector3 targetPos = GetEyesPosition() +
        //                dir * DISTANCE_LASER_IF_NO_HIT; // go a distance forward from the camera direction
        //            ShowLasers(targetPos);
        //        }
        //    }
            yield return null;
        //}
        //currentEnemy = null;
        //SetState(State.Idle);
    }
    private void LookForEnemies()
    {
        //Collider[] surroundingColliders = Physics.OverlapSphere(this.transform.position, lookDistance);
        //foreach (Collider coll in surroundingColliders)
        //{
        //    //how do we know if the element we are colliding with is an enemy?
        //    //Not on our team.
        //    Unit unit = coll.GetComponent<Unit>();
        //    if (unit != null)
        //    {
        //        //we also want to check a couplemore things:
        //        if (unit.Team != Team && CanSee(unit.transform, unit.transform.position + aimOffset) && unit.isAlive)
        //        {
        //            currentEnemy = unit;
        //            SetState(State.Chasing);
        //            return; //remember: you can return anywhere in a void function and it immediately exits
        //        }
        //    }
        //}
    }

    private void LookForSpots()
    {
        int r = Random.Range(0, GameManager.Instance.currentSpot.Length);//find a random spot for the AI to look for
        if (team == GameManager.Instance.currentSpot[r].team && GameManager.Instance.currentSpot[r].currentValue != 1)
        {
            currentSpot = GameManager.Instance.currentSpot[r];
        }
    }



    // Update is called once per frame
    void Update()
    {
       //agent.destination = goalObject.transform.position;
    }
}
