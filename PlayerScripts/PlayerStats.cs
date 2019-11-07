using UnityEngine;

public class PlayerStats : MonoBehaviour {

    /* Script to determine what has hit the player and their health as a result. */
    SkeleBossController skeleton;

    //Stats
    public float maxHealth = 100;
    public float currentHealth = 100;

    public const int meleeDmg = 45;


	// Use this for initialization
	void Start () {
        skeleton = GameObject.FindWithTag("SkeleBoss").GetComponent<SkeleBossController>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void DamagedReceived(string incoming)
    {
        if (incoming == "laser")
        {
            Debug.Log("Enemy laser hit");
            currentHealth -= 10;
        }

        if (incoming == "melee")
        {
            Debug.Log("melee damage taken");
            currentHealth -= skeleton.BaseAttackDamage;
        }

        if (incoming == "Explosive")
        {
            Debug.Log("Explosive damage taken");
            currentHealth -= (int)(skeleton.BaseAttackDamage * 1.5);
        }

        if (incoming == "Fire")
        {
            Debug.Log("Explosive damage taken");
            currentHealth -= (int)(skeleton.BaseAttackDamage * 1.2);
        }

    }
    
}
