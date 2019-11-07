using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public Transform cameraPivot;
	float mouseInput = 0;

    // Update is called once per frame
    void LateUpdate ()
    {
        mouseInput += Input.GetAxis("Mouse X") * Time.deltaTime * 180f;
        cameraPivot.rotation = Quaternion.Euler(0,mouseInput,0);
    }


}
