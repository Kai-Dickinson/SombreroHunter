using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanel : MonoBehaviour
{
    public GameObject infoPanel;
    private bool isOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("i") && !isOpen)
        {
            infoPanel.SetActive(true);
            isOpen = true;
        }
        else if (Input.GetKeyDown("i") && isOpen)
        {
            infoPanel.SetActive(false);
            isOpen = false;
        }
    }
}
