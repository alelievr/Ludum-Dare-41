using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetecontroller : MonoBehaviour {

	public float sensisitivity = 3;
	public Transform corp;
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;		
	}
	
	// Update is called once per frame
	void Update () {
		// transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)), Vector3.up);
		transform.Rotate(-Input.GetAxis("Mouse Y") * sensisitivity,0, 0);
		corp.Rotate(0, Input.GetAxis("Mouse X") * sensisitivity, 0);
	}
}
