using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{

    protected Animator animator;

    protected int fullHealth = 100;
    [SerializeField]
    protected int health; //the current health value of our unit 

    [SerializeField]
    protected int team; //Variable to determine if the unit is AI or player

    [SerializeField]
    int damage;

    [SerializeField]
    bool canRespawn = true;

    [SerializeField]
    protected bool beDamaged = true;

    protected const float DISTANCE_SHOT_IF_NO_HIT = 500.0f;

    private Eye[] eyes = new Eye[2]; //Add the Eye script to the AI and we can get the Eyes from the player

    public float viewAngle; //the angle in which the AI can see other objects, changes for different AI. Default 80

    internal bool isAlive = true;
    protected Vector3 respawnPos;//set pos based on the players check point
    
    [SerializeField]
    private float respawnTime = 2.0f;

    #region AudioSettings
    [SerializeField]
    protected AudioClip _gotHitClip;
    [SerializeField]
    protected AudioClip _dieClip;
    [SerializeField]
    protected AudioClip _attackClip;
    [SerializeField]
    protected AudioClip _runClip;

    protected AudioSource _audioSource;
    #endregion

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();

        eyes = GetComponentsInChildren<Eye>();

        _audioSource = GetComponent<AudioSource>();

        respawnPos = this.transform.position; //At the start this should be the respawn position
    }

    protected virtual void OnHit(Unit attacker)
    {
        if (beDamaged)
        {
            health -= attacker.damage; //take some damage from the attacker
            if (_audioSource != null && isAlive)
            {
                _audioSource.clip = _gotHitClip;
                _audioSource.loop = false;
                _audioSource.PlayDelayed(0.1f);
            }


        }
        if (health <= 0)
        {
            Die();
        }
    }

    protected bool CanSee(Transform target, Vector3 targetPosition)
    {
        Vector3 startPos = GetEyesPosition();//where we do get the starting position of our vision?
        Vector3 dir = targetPosition - startPos;
        //We now need to change if our angle is greater than the viewing angle, and, if so, return false
        if (Vector3.Angle(transform.forward, dir) > viewAngle)
            return false;

        Ray ray = new Ray(startPos, dir);
        LayerMask mask = ~LayerMask.GetMask("AISpot");//make sure that we don't care about our AISpot when looking for enemies
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            if (hit.transform != target)
            {
                return false;
            }
        }
        return true;
    }

    protected Vector3 GetEyesPosition()
    {
        return (eyes[0].transform.position + eyes[1].transform.position) / 2.0f;
    }

    protected void Attack(RaycastHit hit)
    {
        //we only want to shoot at units 
        Unit unit = hit.transform.GetComponent<Unit>(); //let's see if we get a unit
        if (unit != null)
        {
            //do some work
            if (unit.team != team)
            {
                unit.OnHit(this);//we are telling a unit that we have done some damage to it
            }

        }
    }

    protected virtual void Die()
    {
        if (!isAlive)
            return; //this is a mistake clearly because we are already dead
        gameObject.layer = LayerMask.NameToLayer("Dead");

        isAlive = false;


        animator.SetBool("Dead", true);

        if (_audioSource != null)
        {
            _audioSource.loop = false;
            _audioSource.clip = _dieClip;
            _audioSource.PlayDelayed(0.5f);
        }

        if (canRespawn)
        {
            Invoke("Respawn", respawnTime);
        }
    }
    public void PublicDie()
    {
        Die();
    }

    public void OnTrapHit(int trapDamage)
    {
        //Add Take hit animation
        health -= trapDamage; //take some damage from the attacker
        if (health <= 0)
        {
            Die();
        }
    }


    protected virtual void Respawn()
    {
        animator.SetBool("Dead", false);
        isAlive = true;
        gameObject.layer = LayerMask.NameToLayer("Alive");
        health = fullHealth;
        this.transform.position = respawnPos; //set player position to the respawn position
    }

    // Update is called once per frame
    void Update()
    {

    }
}
