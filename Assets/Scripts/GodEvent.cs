using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodEvent : MonoBehaviour {

	// Use this for initialization
	public delegate void BuildFarm(Vector3 pos, GameObject zone);
	public static event BuildFarm farmEvent;

	public GameObject	farm;

	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (farmEvent != null)		
			{
				GameObject newFarm = Instantiate(farm, transform.position, transform.rotation);
				farmEvent(transform.position , newFarm);
			}
		}
	}
}
  