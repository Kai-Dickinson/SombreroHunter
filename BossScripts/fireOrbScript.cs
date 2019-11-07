using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireOrbScript : MonoBehaviour
{
    Transform player;
    Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerPos = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Move the object towards the player position if it hasn't reached it already
        if (gameObject.transform.position != playerPos)
        {
            Vector3 newPos = Vector3.MoveTowards(gameObject.transform.position, playerPos, 3f * Time.deltaTime);
            gameObject.transform.position = newPos;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
