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
	public int 		maxspawned = 30;
	public float		timbeforebegin = 0;
	List<FollowerController> allCurrentEntity = new List<FollowerController>();

	void Start () {
		god = Camera.main.GetComponentInParent<GodController>();
	}
	
	// Update is called once per frame
	void Update () {
		timbeforebegin -= Time.deltaTime;
		if (timbeforebegin > 0)
			return ;
		timesincelast += Time.deltaTime;
		if (timesincelast > timebeforespawn)
		{
			allCurrentEntity.RemoveAll(item => item == null);
			timesincelast = 0;
			for (int i = 0; i < numberofspam && allCurrentEntity.Count < maxspawned; i++)
			{
				GameObject tmp = GameObject.Instantiate(badguys, transform.position, Quaternion.identity);
				tmp.GetComponent<FollowerController>().ChargeCallback(cibleofspam);
				allCurrentEntity.Add(tmp.GetComponent<FollowerController>());
				tmp.GetComponent<FollowerController>().god = god;
			}
			if (allCurrentEntity.Count == maxspawned)
			{
				foreach(FollowerController fc in allCurrentEntity)
					fc.badguyfocusgod = true;
			}
		}
	}
}
