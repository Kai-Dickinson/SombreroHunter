using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour {
    public const int maxHealth = 100;
    public float currHealth = maxHealth;

    private bool isHit = false;

    Animator enemyAnim;

    private void Start()
    {
        enemyAnim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (currHealth <= 0) {
            enemyAnim.SetBool("dead", true);
            StartCoroutine(EnemyDeath());
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHit == false)
        {
            
            if (other.tag == "weapon")
            {
                enemyAnim.SetTrigger("hit");
                MinionDamage("melee");
                isHit = true;
            }
        }

    }

    private IEnumerator OnTriggerExit(Collider other)
    {
        yield return new WaitForSeconds(1);
        isHit = false;
    }


    void MinionDamage(string incoming)
    {
        if (incoming == "melee") {
            Debug.Log("melee damage taken");
            currHealth -= PlayerStats.meleeDmg;
        }

    }

    IEnumerator EnemyDeath() {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

}
