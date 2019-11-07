using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AnimationController : MonoBehaviour
{
    /* Script used for determining the animation needed at a current point. *All sound related code is not mine*. */
    public Animator playerAnim;
    bool isHit = false;
    float timePassedHit = 0f;
    float timePassedSlice;

    public AudioMixerSnapshot Peace;
    public AudioClip[] Footsteps;
    public AudioSource FootstepsSource;
    public AudioClip Woosh;
    public AudioSource WooshSource;
    public AudioClip oof;
    public AudioSource BigOof;

    SkeleBossController skeleton;
    PlayerStats playerStats;

    public EnterSimulation sim;

    private void Start()
    {
        playerStats = gameObject.GetComponent<PlayerStats>();
        skeleton = GameObject.FindGameObjectWithTag("SkeleBoss").GetComponent<SkeleBossController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            playerAnim.SetBool("running", true);
        }
        else { playerAnim.SetBool("running", false); }

        if (Input.GetMouseButtonDown(0) && Input.GetMouseButton(1) != true && Time.time - timePassedSlice >= 1f)
        {
            WooshSource.PlayOneShot(Woosh);
            playerAnim.SetTrigger("attacking");
            timePassedSlice = Time.time;
        }
        if (playerAnim.GetBool("running"))
        {
            int randFootstep = Random.Range(0, Footsteps.Length);
            if (!FootstepsSource.isPlaying)
            {
                FootstepsSource.PlayOneShot(Footsteps[randFootstep]);
            }
        }
        if (playerStats.currentHealth <= 0 && sim.inSimulation == false)
        {
            StartCoroutine(PlayerDie());
        }
    }



    //DETECT ENEMY WEAPONS
    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - timePassedHit >= 1f) { isHit = false; }

        if (!isHit)
        {
            if (other.tag == "enemyweapon")
            {
                if (skeleton.doingAttackSlice)
                {
                    BigOof.PlayOneShot(oof);
                    playerAnim.SetTrigger("GotHit");
                    playerStats.DamagedReceived("melee");
                    isHit = true;
                    timePassedHit = Time.time;
                }
            }

            if (other.tag == "Explosion")
            {
                BigOof.PlayOneShot(oof);
                playerAnim.SetTrigger("GotHit");
                playerStats.DamagedReceived("Explosive");
                isHit = true;
                timePassedHit = Time.time;
            }

            if (other.tag == "Fireorb")
            {
                BigOof.PlayOneShot(oof);
                playerAnim.SetTrigger("GotHit");
                playerStats.DamagedReceived("Fire");
                isHit = true;
                timePassedHit = Time.time;
            }

            
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            BigOof.PlayOneShot(oof);
            playerAnim.SetTrigger("GotHit");
            playerStats.DamagedReceived("laser");
            isHit = true;
            timePassedHit = Time.time;
        }
    }

    IEnumerator PlayerDie()
    {
        playerAnim.SetBool("isDead", true);
        Peace.TransitionTo(2.2f);
        yield return new WaitForSeconds(2.2f);
        Destroy(gameObject);
        RestartLevel();
    }


    void RestartLevel()
    {
        BigOof.PlayOneShot(oof);
        playerAnim.SetBool("isDead", false);
        Application.LoadLevel(Application.loadedLevel);
    }
}
