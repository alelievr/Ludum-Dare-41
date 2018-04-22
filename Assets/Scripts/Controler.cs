using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controler : MonoBehaviour {

	// Use this for initialization
	public float sensisitivity = 3;
	public float speed = 4;
	public Transform head;
	CharacterController ch;
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		ch = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		head.Rotate(-Input.GetAxis("Mouse Y") * sensisitivity, 0, 0);
		transform.Rotate(0, Input.GetAxis("Mouse X") * sensisitivity, 0);

		ch.Move(transform.right * Input.GetAxis("Horizontal") * speed + transform.forward * Input.GetAxis("Vertical") * speed);
		
	}
}
