using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessages : MonoBehaviour
{
    /* This script was a quick script to load tutorial messages for the player to follow and quickly understand controls. */
  
    public GameObject moveMessage;
    bool moveTrigger = false;
    public GameObject fightMessage;
    bool fightTriggered = false;
    bool fightDone = false;
    public GameObject npcMessage;
    bool npcTrigger = false;
    bool npcDone = false;
    public GameObject infoMessage;
    bool infoTrigger = false;

    public GameObject player;

    public GameObject enemy1;
    public GameObject enemy2;
    

    bool wPress = false;
    bool sPress = false;
    bool aPress = false;
    bool dPress = false;
    bool clicked = false;

    private void Start()
    {
        moveMessage.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w")) { wPress = true; }
        if (Input.GetKeyDown("a")) { aPress = true; }
        if (Input.GetKeyDown("s")) { sPress = true; }
        if (Input.GetKeyDown("d")) { dPress = true; }
        if (Input.GetMouseButtonDown(0)) { clicked = true; }

        if (wPress && aPress && sPress && dPress && clicked) { moveMessage.SetActive(false); moveTrigger = true; }


        if (enemy1 == null && enemy2 == null && fightTriggered) { fightMessage.SetActive(false); fightDone = true; }

        if (npcTrigger && Input.GetKeyDown(KeyCode.Return)) { npcMessage.SetActive(false); npcDone = true; }

        if (infoTrigger && Input.GetKeyDown("i")) { infoMessage.SetActive(false); }
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && moveTrigger && !fightTriggered)
        {
            fightMessage.SetActive(true);
            fightTriggered = true;
        }

        if (other.tag == "Player" && fightDone && !npcTrigger) { npcMessage.SetActive(true); npcTrigger = true; }

        if (other.tag == "Player" && npcDone && !infoTrigger) { infoMessage.SetActive(true); infoTrigger = true; }
    }
}
