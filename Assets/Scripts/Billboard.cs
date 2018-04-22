using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	Camera	cam;

	void Start ()
	{
		cam = Camera.main;
	}
	
	void Update ()
	{
		transform.LookAt(transform.position + cam.transform.rotation * Vector3.back, cam.transform.rotation * Vector3.up);
	}
}
