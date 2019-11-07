using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossScript
{

    //Creation values
    public int Health;
    public int AttackDamage;
    public List<string> AvailAttacks = new List<string> {"ChargeAttack", "FireAttack", "ThrowAttack"};
    public List<string> ChosenAttacks = new List<string>();
    public int index;
    public int indexTwo;

    public SkeletonBossScript()
    {
        ChosenAttacks.Add("SwipeAttack"); //Default Swipe attack added

        //Creates enemy with random health, damage, defence and attacks
        Health = Random.Range(200, 400);
        AttackDamage = Random.Range(10, 30);
        int index = Random.Range(0, AvailAttacks.Count);
        int indexTwo = Random.Range(0, AvailAttacks.Count);
        while (indexTwo == index) { indexTwo = Random.Range(0, AvailAttacks.Count); }

        ChosenAttacks.Add(AvailAttacks[index]);
        ChosenAttacks.Add(AvailAttacks[indexTwo]);
    }
    

}
