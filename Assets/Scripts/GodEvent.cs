using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodEvent : MonoBehaviour {

	// Use this for initialization
	public delegate void BuildFarm(Vector3 pos);
	public static event BuildFarm farmEvent;


	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (farmEvent != null)			
				farmEvent(transform.position);
		}
	}
}
  