using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using static UnityEngine.UI.CanvasScaler;

public class AIController : Unit
{

    private enum State
    {
        Idle,
        Patrolling,
        Chasing,
        Hurt
    }

    public float attackInterval = 1f;
    public float lookDistance = 10;//our AI can see 10 units away
    private State currentState; //this keeps track of the current state
    private NavMeshAgent agent; //this is our navmesh agent
    private Unit currentEnemy; //current enemy
    private AISpot currentSpot; //the current spot we are focused on
    private AISpot lastSpot; //the new spot selected by the AI

    private int arrayNum = 0;

    public Vector3 aimOffset = new Vector3(0, 1.5f, 0);

    [SerializeField]
    private float timer; //this will keep track of the time within the outpost

    [SerializeField]
    private float attackDis; //this will keep track of the time within the outpost

    [SerializeField]
    private float speedMultiplierChasing;

    [SerializeField]
    private float stunInterval = 1;


    private float defaultSpeed;

    private bool checkTeam = false;

    [SerializeField]
    private AudioClip _idleClip;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        respawnPos = this.transform.position;
        defaultSpeed = agent.speed;

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
            case State.Hurt:
                StartCoroutine(OnHurt());
                break;
            default:
                break;
        }
        ///
    }

    private IEnumerator OnIdle() //handles our idle state
    {
        //when idling, we should probably do some work and look for an outpost
        animator.SetBool("Running", false);
        agent.speed = defaultSpeed;

        if (_audioSource != null) //Play idle audio
        {
            _audioSource.clip = _idleClip;
            _audioSource.loop = true;
            _audioSource.Play();
        }


        if (currentSpot != null)
        {
            lastSpot = currentSpot;
        }
        currentSpot = null;
        while (currentSpot == null)
        {
            if (isAlive)
                LookForSpots(); //if we ever find an outpost, and the currentSpot changes, we will leave this loop
            yield return null;

        }
        SetState(State.Patrolling); //we found an outpost, we now need to move

    }
    private IEnumerator OnPatrolling()
    {
        animator.SetBool("Running", false);
        agent.speed = defaultSpeed;
        agent.SetDestination(currentSpot.transform.position);

        if (_audioSource != null) //Play idle audio
        {
            _audioSource.clip = _idleClip;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        while (currentSpot.currentValue != 1)
        {
            LookForEnemies();
            yield return null;
        }

        //After Value turns 1, he is going to search for a new spot
        SetState(State.Idle);

    }
    private IEnumerator OnChasing()
    {
        ////we have to reset the path of our agent
        agent.ResetPath();
        float attackTimer = attackInterval;
        while (currentEnemy != null && currentEnemy.isAlive)
        {
            attackTimer += Time.deltaTime; //increment our shoot timer each time
            float distanceToEnemy = Vector3.Distance(currentEnemy.transform.position, this.transform.position);
            //if we are too far away or we can't see our enemy, let's move towards them
            //otherwise, if our shoot timer is up, shoot them

            Vector3 enemyPos = new Vector3(currentEnemy.transform.position.x, transform.position.y, currentEnemy.transform.position.z);

            transform.LookAt(enemyPos);

            if (_audioSource != null && !_audioSource.isPlaying) //Play attacking audio
            {
                _audioSource.clip = _runClip;
                _audioSource.loop = true;
                _audioSource.Play();
            }


            if (distanceToEnemy > attackDis || !CanSee(currentEnemy.transform, currentEnemy.transform.position + aimOffset))
            {
                agent.SetDestination(currentEnemy.transform.position);
                agent.speed = defaultSpeed * speedMultiplierChasing;
                animator.SetBool("Running", true);
            }
            else if (attackTimer > attackInterval)
            {
                agent.ResetPath();
                attackTimer = 0;
                Vector3 dir = currentEnemy.transform.position - this.transform.position;
                dir.Normalize();

                LayerMask mask = ~LayerMask.GetMask("Outpost", "Terrain");
                Ray ray = new Ray(GetEyesPosition(), dir); //aim our ray in the direction that we are looking
                RaycastHit hit; //our hit is going to be used as an output of a Raycast
                                //so we need to use a layermask and a layermask is 
                if (Physics.Raycast(ray, out hit, attackDis, mask))
                {
                    //if this is true, we attack the player
                    animator.SetBool("Running", false);
                    animator.SetTrigger("Attack");

                    if (_audioSource != null) //Play attacking audio
                    {
                        _audioSource.Stop();
                        _audioSource.clip = _attackClip;
                        _audioSource.loop = false;
                        _audioSource.Play();
                    }

                    yield return new WaitForSecondsRealtime(1f); //Wait for the animation to complete before actaully reducing life
                    Attack(hit);
                    
                }
                else
                {
                    Vector3 targetPos = GetEyesPosition() + dir; // go a distance forward from the camera direction
                    //ShowLasers(targetPos);
                }

            }
            yield return null;
        }
        health = fullHealth; 
        currentEnemy = null;
        agent.ResetPath();
        agent.speed = defaultSpeed;
        SetState(State.Idle);
    }

    private IEnumerator OnHurt()
    {
        ////Stop moving from the agent until the animation stops playing
        ///
        float stunTimer = 0;
       
        while (stunTimer < stunInterval)
        {
            stunTimer += Time.deltaTime;
            agent.speed = 0;
            yield return null;
        }

        if (!isAlive)
        {
            currentEnemy = null;
        }

        SetState(State.Chasing);

    }

    private void LookForEnemies()
    {
        Collider[] surroundingColliders = Physics.OverlapSphere(this.transform.position, lookDistance);
        foreach (Collider coll in surroundingColliders)
        {
            //how do we know if the element we are colliding with is an enemy?
            //Not on our team.
            Unit unit = coll.GetComponent<Unit>();
            if (unit != null)
            {
                //we also want to check a couplemore things:
                if (unit.tag == "Player" && CanSee(unit.transform, unit.transform.position + aimOffset) && unit.isAlive)
                {
                    Debug.Log("Sees Player");
                    currentEnemy = unit;
                    SetState(State.Chasing);
                    return; //remember: you can return anywhere in a void function and it immediately exits
                }
            }
        }
    }

    private void LookForSpots()
    {
        arrayNum = Random.Range(0, GameManager.Instance.currentSpot.Length);//finds a random spot for the AI to look for

        foreach (AISpot aISpot in GameManager.Instance.currentSpot) //Check if there are any AI Spots with the same team as the AI
        {
            if (aISpot.team == team)
            {
                checkTeam = true;
                break;
            }
        }


        if (checkTeam)
        {
            while (team != GameManager.Instance.currentSpot[arrayNum].team && GameManager.Instance.currentSpot[arrayNum] != lastSpot) //Check if the arrayNum picked is the same team as the AISpot
            {
                arrayNum = Random.Range(0, GameManager.Instance.currentSpot.Length);//finds a random spot for the AI to look for
            }

            currentSpot = GameManager.Instance.currentSpot[arrayNum];

        }
        else
        {
            Debug.Log("No AISpots Found!");
        }

    }

    protected override void OnHit(Unit attacker)
    {
        base.OnHit(attacker);

        if (currentEnemy == null && isAlive)
        {
            currentEnemy = attacker;
        }
        if (isAlive)
        {
            animator.SetTrigger("GotHit");//Animation for getting hit
            SetState(State.Hurt);
        }

    }

    protected override void Respawn()
    {
        
        base.Respawn();
        SetState(State.Idle);
    }

    protected override void Die()
    {
        respawnPos = this.transform.position; //Respawned at the same place he died
        currentSpot = null;
        if (lastSpot != null)
        {
            lastSpot = null;
        }

        StopAllCoroutines();
        agent.ResetPath();
        animator.SetBool("Running", false);
        currentEnemy = null;
        base.Die();

    }

    public void BackToIdle()
    {
        SetState(State.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("VerticalSpeed", agent.velocity.magnitude);

    }
}
