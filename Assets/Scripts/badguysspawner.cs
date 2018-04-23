using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class badguysspawner : MonoBehaviour {

	// Use this for initialization
	public float timebeforespawn;
	public int numberofspam;
	public Vector3 cibleofspam;
	float timesincelast = 0;
	GodController god;
	public GameObject badguys;

	void Start () {
		god = Camera.Main.GetComponentsInParents<GodController>().
	}
	
	// Update is called once per frame
	void Update () {
		timesincelast += Time.deltaTime;
		if (timesincelast > timebeforespawn)
		{
			timesincelast = 0;
			for (int i = 0; i < numberofspam; i++)
			{
				GameObject tmp =GameObject.Instantiate(badguys, transform.position, Quaternion.identity);
				tmp.agent.setdestination(cibleofspam);
			}
		}
	}
}
