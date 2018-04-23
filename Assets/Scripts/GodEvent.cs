﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GodEvent : MonoBehaviour
{
	float		shoutDist = 50f;
	// Use this for initialization
	public delegate void FarmDelegate(Vector3 pos, ZoneScript zone);
	public static event FarmDelegate farmEvent;

	public delegate void FollowGodDelegate(Transform godTrans);
	public static event FollowGodDelegate followEvent;

	public delegate void StayDelegate(Vector3 pos);
	public static event StayDelegate stayEvent;

	public delegate void SpawnDelegate(GodEvent godEvent, ZoneScript zoneSc);
	public static event SpawnDelegate spawnEvent;
	public GameObject spawnZone;

	public GameObject ArmoryZone;


	public delegate void searchCible(FollowerController fc);
	public static event searchCible cibleEvent;
	public  ParticleSystem pscharge;

	ZoneScript[]		availableZones;

	[HideInInspector] public static List<FollowerController> listAllFollower = new List<FollowerController>();
	[HideInInspector] public static List<FollowerController> listAllBadGuys = new List<FollowerController>();
	[HideInInspector] public static List<FollowerController> listAllFollowerFollowing = new List<FollowerController>();
	[HideInInspector] public static GodController god;

	List<ZoneScript>		AllArmory = new List<ZoneScript>();



	void Start () {
		availableZones = Resources.FindObjectsOfTypeAll< ZoneScript >();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (farmEvent != null)
			{
				Debug.Log("FARRMM");
				ZoneScript targetZone = FindNearestZone(availableZones);
				if (Vector3.Distance(targetZone.transform.position, transform.position) < shoutDist)
					farmEvent(transform.position, targetZone);
			}
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (followEvent != null)
			{
				Debug.Log("COME TO MEEEE");
				followEvent(transform);
			}
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			if (stayEvent != null)
			{
				Debug.Log("STAY HERE");
				stayEvent(transform.position);
			}
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (spawnEvent != null &&  GodEvent.listAllFollowerFollowing.Count >= 30)
			{
				Debug.Log("SPAWN HERE");
				ZoneScript targetZone = Instantiate(spawnZone, transform.position, transform.rotation).GetComponent<ZoneScript>();
				spawnEvent(GetComponent<GodEvent>(), targetZone);
			}
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			if (spawnEvent != null &&  GodEvent.listAllFollowerFollowing.Count >= 30)
			{
				ZoneScript targetZone = Instantiate(ArmoryZone, transform.position, transform.rotation).GetComponent<ZoneScript>();
				AllArmory.Add(targetZone);
				int i = 0;
				int j = 0;
				while(i < listAllFollowerFollowing.Count && j < 30)
				{
					ZoneScript zonesc = targetZone;
					if (GodEvent.listAllFollowerFollowing[i].issoldat == false)
					{
						Debug.Log("JE SUIS ARM");
						GodEvent.listAllFollowerFollowing[i].state = FollowerState.MovingToArmory;
						GodEvent.listAllFollowerFollowing[i].agent.SetDestination(zonesc.NextFreePos());
						GodEvent.listAllFollowerFollowing.Remove(GodEvent.listAllFollowerFollowing[i]);
						j++;
					}
					else
						i++;
				}
			}
		}
		
		if (Input.GetMouseButtonDown(0))
		{
			// RaycastHit hit;
			// if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
			// {
				RaycastHit hit;
				if (AllArmory.Count() > 0
				&& Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
				{
					ZoneScript targetZone = FindNearestZone(AllArmory.ToArray());
					GameObject.Instantiate(pscharge, hit.point, Quaternion.identity);
					Debug.Log(listAllFollowerFollowing.Count);
					int i = 0;
					while(i < listAllFollowerFollowing.Count)
					{
						if (!(listAllFollowerFollowing[i].ToArmCallback(hit.point, targetZone)))
						i++;
					}
					Debug.Log(listAllFollowerFollowing.Count);
				}
				// Debug.Log(listAllFollowerFollowing.Count);
		}
	}
	ZoneScript FindNearestZone(ZoneScript[] Zones)
	{
		Vector3 pos = transform.position;
		return Zones.OrderBy(z => (z.transform.position - pos).sqrMagnitude).First();
	}

	//  void OnDrawGizmos() {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(transform.position, 50f);
    // }


 	void OnGUI()
    {
        // Camera  c = Camera.main;
        // Event   e = Event.current;

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.

        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.EndArea();
    }
}