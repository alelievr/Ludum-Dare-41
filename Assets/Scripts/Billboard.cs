using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	public bool flipX;

	Camera	cam;

	void Start ()
	{
		cam = Camera.main;
	}
	
	void Update ()
	{
		transform.LookAt(transform.position + cam.transform.rotation * ((flipX) ? Vector3.forward : Vector3.back), cam.transform.rotation * Vector3.up);
	}
}
