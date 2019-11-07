using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    /* Script for the movement and what was meant to become a shooting mechanic, which was not needed. */

    public float turnSmoothing = 15f; // A smoothing value for turning the player.

    float movementSpeed = 6f;
    public Transform cam;

    Vector2 input;

    public float fireRate = 0f; //Values to be used for RayCasting
    private float firingTime = 0f;

    // Update is called once per frame
    void Update ()
    {
        MovementLogic();
        CombatMechs();
    }

    /// <summary>
    /// Logic for movement and camera rotation.
    /// </summary>
    void MovementLogic()
    {
        //Player Movement Setion
        float hAxis = Input.GetAxis("Horizontal"); //Gets input of x and y
        float vAxis = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(hAxis, vAxis, 0); //Create new value of vector3
        input = Vector3.ClampMagnitude(input, 1); //Clamp maximum value of xyz to 1

        Vector3 camF = cam.forward;
        camF.y = 0;
        Vector3 camR = cam.right;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        transform.position += (camF * input.y + camR * input.x) * Time.deltaTime * movementSpeed;


        //Rotating player in direction of its last movement
        if (hAxis != 0 || vAxis != 0)
        {
            Vector3 targetDirection = new Vector3(hAxis, 0f, vAxis);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            // Create a rotation that is an increment closer to the target rotation from the player's rotation.
            Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);

            // Change the players rotation to this new rotation.
            GetComponent<Rigidbody>().MoveRotation(newRotation);
        }

        //End of player Movement Section
    }


    /// <summary>
    /// Combat logic to tell when an attack type takes place, and calls the Shoot() function.
    /// </summary>
    void CombatMechs()
    {
        //Start of Combat Logic
        if (Input.GetMouseButton(1) && Input.GetMouseButtonDown(0))
        {   //Detect range attack
            if (fireRate == 0)
            {
                Shoot();
            }
            if (Input.GetMouseButtonDown(0) && Time.time > firingTime)
            {
                firingTime = Time.time + 1 / fireRate;
                Shoot();
            }
            Debug.Log("Range Attack");

        }
        //else if (Input.GetMouseButtonDown(0))
        //{   //Detect Melee attack
        //    Debug.Log("Melee Attack"); //Needs wait logic to wait 2s, before reacting.

        //}

        //End of Combat Logic
    }

    /// <summary>
    /// Logic for shooting rayCasts to detect hits with the range attack
    /// </summary>
    void Shoot()
    {

    }




}
