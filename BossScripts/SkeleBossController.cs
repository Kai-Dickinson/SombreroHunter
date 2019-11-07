using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;


public class SkeleBossController : MonoBehaviour
{
    public float approach = 8f;
    Transform target;
    NavMeshAgent agent;

    public int BaseAttackDamage;
    public int currHealth; //Current boss health

    private bool isHit = false; //Check if boss has been hit
    public bool isBattle = false; //Check if the boss is still in battle
    public float timePassed; //Time passed since given action
    bool timeTriggered = false; //Check if player has left area

    //Attacking logic variables
    float betweenSlice = 0.0f; //Time after each type of attack
    float betweenCharge = 0.0f;
    float betweenSpecial = 0.0f;
    bool lastSpecFire = false; //Check which special was used last

    //Throw attack variables
    float timeAfterThrow = 0.0f;
    bool throwDone = false;
    Vector3 playerPosition;
    public GameObject explosionPrefab;

    //Fire attack variables
    public GameObject firePrefab;
    public GameObject firingPoint;

    TrackingController clueObject;

    public bool doingAttackSlice = false; //Let the game know it is attacking
    public bool doingAttackCharge = false;

    public AudioMixerSnapshot Peace;
    public AudioSource Victory;
    public AudioClip win;
    public AudioSource SwooshSource;
    public AudioClip Swoosh;
    public AudioSource FlameSource;
    public AudioClip Flames;

    //Animator for the skeleton
    public Animator enemyAnim;
    //Player animator
    Animator playerAnim;
    public SkeletonBossScript Skeleton; //Boss of type skeletonbossscript

    EnterSimulation simScript;

    // Use this for initialization
    private void Awake()
    {
        //Create new instance of the boss
        Skeleton = new SkeletonBossScript();
        //Set health and base attack damage
        currHealth = Skeleton.Health;
        BaseAttackDamage = Skeleton.AttackDamage;
    }

    void Start()
    {
        //Fetch the animator and play animator
        enemyAnim = gameObject.GetComponent<Animator>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        //Fetch transform for the player and the NavMesh attatched to this object
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        clueObject = GameObject.FindGameObjectWithTag("SliceClue").GetComponent<TrackingController>();

        simScript = GameObject.FindGameObjectWithTag("Player").GetComponent<EnterSimulation>();
    }

    // Update is called once per frame
    void Update()
    {
        //DEATH LOGIC
        if (currHealth <= 0)
        {
            enemyAnim.SetBool("Death", true);
            Victory.clip = win;
            Victory.Play();
            Peace.TransitionTo(12);
            StartCoroutine(EnemyDeath());
        }

        float distance = Vector3.Distance(target.position, transform.position); //Find distance to player
        bool isClose = false; //Logic for whether the player close enough for battle
        
        if (distance <= approach) { isClose = true; timeTriggered = false; } //Trigger the isClose boolean
        if (isClose) { isBattle = true; } //Battle triggered if close enough

        //ONLY MOVE IF NOT DEAD
        if (currHealth >= 0)
        {
            //If close or battling and not being hit
            if ((isClose || isBattle) && !isHit)
            {
                
                distance = Vector3.Distance(target.position, transform.position); //Recalculate distance
                if (distance > approach) { isClose = false; if (isBattle && !timeTriggered) { timePassed = Time.time; timeTriggered = true; } } //Check if out of range

                //If battling but not close and 3s has passed, fight ends.
                if (isBattle && !isClose)
                {
                    if (Time.time - timePassed >= 3.0f) { isBattle = false; Debug.Log("NOT FIGHTING"); }
                    //Wait 5 seconds before stopping pursuit
                }

                //If battling but not close enough, use the charge attack every 8 seconds.
                if (isBattle && !isClose && Time.time - betweenCharge >= 8f)
                { 
                    for (int i = 0; i < Skeleton.ChosenAttacks.Count; i++)
                    {
                        if (!simScript.inSimulation || (simScript.inSimulation && clueObject.chargeTold))
                        {
                            if (Skeleton.ChosenAttacks[i] == "ChargeAttack")
                            {
                                ChargeAttack();
                                doingAttackCharge = true;
                                if (!simScript.inSimulation) { clueObject.ChargeKnown = true; }
                            }
                            
                        }
                        
                    }
                }

                //If battling and the distance is within double stopping distance, slice every 5s
                if (isBattle && distance <= agent.stoppingDistance * 2 && Time.time - betweenSlice >= 5f)
                {
                    for (int i = 0; i < Skeleton.ChosenAttacks.Count; i++)
                    {
                        if (!simScript.inSimulation || (simScript.inSimulation && clueObject.sliceTold))
                        {
                            if (Skeleton.ChosenAttacks[i] == "SwipeAttack")
                            {
                                SwipeAttack();
                                SwooshSource.PlayOneShot(Swoosh);
                                doingAttackSlice = true;
                                if (!simScript.inSimulation) { clueObject.SliceKnown = true; }
                                
                            }
                        }
                        
                    }
                }

                //If battling and close and 5 times stopping distance use a special attack in rotation
                if (isBattle && isClose && distance > agent.stoppingDistance * 5 && Time.time - betweenSpecial >= 10f)
                {
                    bool bothAttack = false;
                    bool isFire = false;
                    for (int i = 0; i < Skeleton.ChosenAttacks.Count; i++)
                    {
                        for (int j = 0; j < Skeleton.ChosenAttacks.Count; j++)
                        {
                            if (Skeleton.ChosenAttacks[j] == "FireAttack" && Skeleton.ChosenAttacks[i] == "ThrowAttack")
                            {
                                bothAttack = true;
                            }
                            if (Skeleton.ChosenAttacks[i] == "FireAttack") { isFire = true; }
                        }
                        
                    }
                    if (!isFire)
                    {
                        if (!simScript.inSimulation || (simScript.inSimulation && clueObject.throwTold))
                        {
                            ThrowAttack();
                            if (!simScript.inSimulation) { clueObject.ThrowKnown = true; }
                        }
                        
                        
                    }

                    else if (isFire && !bothAttack)
                    {
                        if (!simScript.inSimulation || (simScript.inSimulation && clueObject.fireTold))
                        {
                            FireAttack();
                            if (!simScript.inSimulation) { clueObject.FireKnown = true; }
                        } 
                    }

                    if (bothAttack)
                    {
                        if (lastSpecFire)
                        {
                            if (!simScript.inSimulation || (simScript.inSimulation && clueObject.throwTold))
                            {
                                ThrowAttack();
                                if (!simScript.inSimulation) { clueObject.ThrowKnown = true; }
                            }
                        }
                        else
                        {
                            if (!simScript.inSimulation || (simScript.inSimulation && clueObject.fireTold))
                            {
                                FireAttack();
                                if (!simScript.inSimulation) { clueObject.FireKnown = true; }
                            }
                        }
                    }
                }

                //Set of explosion after 1s of the special being used
                if (Time.time - timeAfterThrow >= 1f && throwDone)
                {
                    Instantiate(explosionPrefab, playerPosition, Quaternion.identity);

                    throwDone = false;
                }

                //Wait for slice animation to end, then set boolean to false
                if (Time.time - betweenSlice >= 3f && doingAttackSlice) { doingAttackSlice = false; }
                //Wait 1s for charge to end then set false
                if (Time.time - betweenCharge >= 1f && doingAttackCharge){ doingAttackCharge = false; }

                //If charge attack has been set off, move to new position till time runs out
                if (doingAttackCharge)
                {
                    Vector3 newPos = Vector3.MoveTowards(gameObject.transform.position, playerPosition, 10f * Time.deltaTime);
                    gameObject.transform.position = newPos;
                }
                //When not using sword, it can move and face the target
                if (!doingAttackSlice) {
                    enemyAnim.SetBool("Moving", true);
                    agent.SetDestination(target.position);
                    FaceTarget();
                }
                
                //When in distance for slice, stop moving
                if (distance < agent.stoppingDistance * 2)
                {
                    agent.velocity = Vector3.zero;
                }
                else
                {
                    agent.speed = 5;
                    agent.acceleration = 15;
                }

            }
            else { enemyAnim.SetBool("Moving", false); agent.velocity = Vector3.zero; }
        }

    }
    //FACE THE TARGET
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    //DRAW AREA OF APPROACH
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, approach);
    }



    //ANIMATIONS AND COLLISION DETECTION
    private void OnTriggerEnter(Collider other)
    {
        if (isHit == false)
        {
            if (other.tag == "weapon")
            {
                enemyAnim.SetTrigger("GotHit");
                TakeDamage("melee");
                isHit = true;
            }
        }

    }

    //Waits 1s after each hit, to stop the enemy being hit multiple times for each pass of a weapon
    private IEnumerator OnTriggerExit(Collider other)
    {
        yield return new WaitForSeconds(1);
        isHit = false;
    }

    //Damage logic for player hiting the boss
    void TakeDamage(string incoming)
    {
        if (incoming == "melee")
        {
            Debug.Log("melee damage taken");
            currHealth -= PlayerStats.meleeDmg;
        }

    }

    //Wait for death and then destroy
    IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(2.2f);
        Destroy(gameObject);
    }


    //ALL ATTACKS

    public void ChargeAttack()
    {
        Debug.Log("Charge!!!");
        betweenCharge = Time.time; //Set current time when charge begins
        playerPosition = target.position; //Set new player position
    }

    public void FireAttack()
    {
        Debug.Log("Fire!!!");
        FlameSource.PlayOneShot(Flames);
        enemyAnim.SetTrigger("Fire"); //Set animation off
        betweenSpecial = Time.time; //Set current time when attack begins
        lastSpecFire = true; //Keep track of last special attack
        //Instantiate the fire prefab
        Instantiate(firePrefab, new Vector3(firingPoint.transform.position.x, firingPoint.transform.position.y + 3, firingPoint.transform.position.z), Quaternion.identity);
    }

    public void SwipeAttack()
    {
        Debug.Log("Swipe!!!");
        enemyAnim.SetTrigger("Sliced");
        betweenSlice = Time.time; //Set current time when attack begins
    }

    public void ThrowAttack()
    {
        Debug.Log("Throw!!!");
        enemyAnim.SetTrigger("Fire");
        betweenSpecial = Time.time; //Set current time when attack begins
        lastSpecFire = false; //keep track of which special was used last
        throwDone = true; 
        timeAfterThrow = Time.time;
        playerPosition = target.position;
    }



}
