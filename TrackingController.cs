using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingController : MonoBehaviour
{

    /* This script is used to track the clues the player receives from the environment. It was used only for one instance of this mechanic. */
    infoPanList infoPanel;
    public float approach = 2f;
    Transform target;
    public GameObject trackUI;

    //private int triggerCount = 0;

    public bool SliceKnown = false;
    int SliceCount = 0;
    public bool ThrowKnown = false;
    int ThrowCount = 0;
    public bool FireKnown = false;
    int FireCount = 0;
    public bool ChargeKnown = false;
    int ChargeCount = 0;

    public bool sliceTold = false;
    public bool fireTold = false;
    public bool chargeTold = false;
    public bool throwTold = false;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        infoPanel = GameObject.FindGameObjectWithTag("infoPanelLogic").GetComponent<infoPanList>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if ((distance <= approach) && SliceCount < 1 && gameObject.tag == "SliceClue")
        {
            StartCoroutine(TrackingTrigg());
        }
        if ((distance <= approach) && FireCount < 1 && gameObject.tag == "FireClue")
        {
            StartCoroutine(TrackingTrigg());
        }
        if ((distance <= approach) && ChargeCount < 1 && gameObject.tag == "ChargeClue")
        {
            StartCoroutine(TrackingTrigg());
        }
        if ((distance <= approach) && ThrowCount < 1 && gameObject.tag == "ThrowClue")
        {
            StartCoroutine(TrackingTrigg());
        }

        if (SliceKnown && !sliceTold) { infoPanel.attackFound = true; infoPanel.attackName = "Slice - the target has a large sharp weapon"; sliceTold = true; SliceCount++; }
        if (ThrowKnown && !throwTold) { infoPanel.attackFound = true; infoPanel.attackName = "Thrown explosive - watch out for an explosive"; throwTold = true; ThrowCount++; }
        if (FireKnown && !fireTold) { infoPanel.attackFound = true; infoPanel.attackName = "Fire - it seems the target has a flame projectile"; fireTold = true; FireCount++; }
        if (ChargeKnown && !chargeTold) { infoPanel.attackFound = true; infoPanel.attackName = "Charge - the target can charge quickly if you get too far away";  chargeTold = true; ChargeCount++; }
    }



    IEnumerator TrackingTrigg()
    {
        if (gameObject.tag == "SliceClue") { SliceKnown = true; SliceCount++; }
        if (gameObject.tag == "ThrowClue") { ThrowKnown = true; ThrowCount++; }
        if (gameObject.tag == "FireClue") { FireKnown = true; FireCount++; }
        if (gameObject.tag == "ChargeClue") { ChargeKnown = true; ChargeCount++; }

        trackUI.SetActive(true);
        yield return new WaitForSeconds(6);
        trackUI.SetActive(false);
    }
}
