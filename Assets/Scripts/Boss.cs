using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// Make changes to the full health for the boss. It is down to the fact that the boss is setting the health bar value from fullHealth, but fullHealth is not a public variable and is 100 by default for units. the fullHealth variable should be something serializable in Unity (does not need to be public) so that you could specify the fullHealth for the boss in the prefab, and have it set to 1000.
/// 
/// I think the boss behaviour is good, but I would very much like them to have some different movement patterns rather than the base unit and to be a different size and style. It would also be nice if the boss could target and attempt to attack you as well.
/// </summary>
public class Boss : Unit
{
    
    private enum State
    {
        Idle,
        Chasing,
        Stun
    }

    public float attackInterval = 1f;
    public float lookDistance = 10;//our AI can see 10 units away
    private State currentState; //this keeps track of the current state
    private NavMeshAgent agent; //this is our navmesh agent
    private Unit currentEnemy; //current enemy
    private AISpot currentSpot; //the current spot we are focused on
    private AISpot lastSpot; //the new spot selected by the AI
    public Vector3 aimOffset = new Vector3(0, 1.5f, 0);

    [SerializeField]
    private int BossMaxHealth;

    [SerializeField]
    private float timer; //this will keep track of the time within the outpost

    [SerializeField]
    private float attackDis; //this will keep track of the time within the outpost

    [SerializeField]
    private float speedMultiplierChasing;

    [SerializeField]
    private float stunInterval = 1;

    [SerializeField]
    private BossHealthBar bossHealthBar;

    [SerializeField]
    private GameObject stunSprite;

    private float defaultSpeed;

    [SerializeField]
    private AudioClip _idleClip;

    private bool stunned;

    [SerializeField]
    private UnityEvent events;

    [SerializeField]
    private AudioClip[] _stunClip;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        respawnPos = this.transform.position; //Change this to the checkpoint mechanic
        SetState(State.Idle);
        defaultSpeed = agent.speed;
        beDamaged = false;

        BossHealth(BossMaxHealth);
        bossHealthBar.SetMaxHealth(fullHealth);

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
            case State.Chasing:
                StartCoroutine(OnChasing());
                //do some work
                break;
            case State.Stun:
                StartCoroutine(OnStun());
                break;
            default:
                break;
        }
        
    }

    private IEnumerator OnIdle() //handles our idle state
    {
        //when idling, we should probably do some work and look for an outpost
        animator.SetBool("Running", false);

        if (_audioSource != null) //Play idle audio
        {
            _audioSource.clip = _idleClip;
            _audioSource.loop = true;
            _audioSource.Play();
        }
        while(currentEnemy == null)
        {
            LookForEnemies();
            yield return null;
        }


    }
    private IEnumerator OnChasing()
    {
        bossHealthBar.gameObject.SetActive(true);
        ////we have to reset the path of our agent
        agent.ResetPath();
        float attackTimer = attackInterval;
        while (currentEnemy != null && currentEnemy.isAlive && isAlive)
        {
            attackTimer += Time.deltaTime; //increment our shoot timer each time
            float distanceToEnemy = Vector3.Distance(currentEnemy.transform.position, this.transform.position);
            //if we are too far away or we can't see our enemy, let's move towards them
            //otherwise, if our shoot timer is up, shoot them

            Vector3 enemyPos = new Vector3(currentEnemy.transform.position.x, transform.position.y, currentEnemy.transform.position.z);

            transform.LookAt(enemyPos);

            if (_audioSource != null && !_audioSource.isPlaying) //Play attacking audio
            {
                _audioSource.clip = _runClip[Random.Range(0, _runClip.Length)];
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
                        _audioSource.clip = _attackClip[Random.Range(0, _attackClip.Length)];
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

        while (Vector3.Distance(this.transform.position, respawnPos) >= 3)
        {
            agent.SetDestination(respawnPos);

            yield return null;
        }

        agent.ResetPath();
        SetState(State.Idle);
    }

    private IEnumerator OnStun()
    {
        ////Stop moving from the agent until the animation stops playing
        ///

        float stunTimer = 0;
        beDamaged = true;
        stunned = true;
        agent.speed = 0;
        stunSprite.SetActive(true);
        //play stunned animation

        if (_audioSource != null) //Play stun audio
        {
            _audioSource.Stop();
            _audioSource.clip = _stunClip[Random.Range(0, _stunClip.Length)];
            _audioSource.loop = true;
            _audioSource.Play();
        }

        while (stunTimer < stunInterval)
        {
            stunTimer += Time.deltaTime;
            yield return null;
        }

        stunned = false;
        stunSprite.SetActive(false);
        beDamaged = false;

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
                //we also want to check a couple more things:
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

    protected override void OnHit(Unit attacker)
    {
        base.OnHit(attacker);

        if (currentEnemy == null && isAlive)
        {
            currentEnemy = attacker;
        }
        if (isAlive && !stunned)
        {
            animator.SetTrigger("GotHit");//Animation for getting hit
            SetState(State.Chasing);

        }
    }

    public void GotStunned()
    {
        SetState(State.Stun);
    }

    public void BackToIdle()
    {
        SetState(State.Idle);
    }

    protected override void Respawn()
    {

        base.Respawn();
        SetState(State.Idle);
    }

    protected override void Die()
    {
        base.Die();

        StopAllCoroutines();
        agent.ResetPath();
        animator.SetBool("Running", false);
        currentEnemy = null;
        stunned = false;
        stunSprite.SetActive(false);

        events.Invoke();

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("VerticalSpeed", agent.velocity.magnitude);
        bossHealthBar.SetHealth(health);
    }
}
