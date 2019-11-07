using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOverScript : MonoBehaviour
{
    /* This script is used to determine the end of the tutorial area. */

    public GameObject winMessage;
    public GameObject skeletonBoss;
    float timeStamp;

    // Update is called once per frame
    void Update()
    {
        if (skeletonBoss == null)
        {

            GameOver();
        }
        else { timeStamp = Time.time; }
    }

    void GameOver()
    {
        Debug.Log("YOU WIN!");
        winMessage.SetActive(true);
        if (Time.time - timeStamp >= 4.0f) { RestartLevel(); }
    }

    void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
