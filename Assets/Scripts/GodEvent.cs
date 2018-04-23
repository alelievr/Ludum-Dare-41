using System.Collections;
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

	public delegate void searchCible(FollowerController fc);
	public static event searchCible cibleEvent;

	ZoneScript[]		availableZones;

	[HideInInspector] public static List<FollowerController> listAllFollower = new List<FollowerController>();
	[HideInInspector] public static List<FollowerController> listAllBadGuys = new List<FollowerController>();
	[HideInInspector] public static GodController god;

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
				ZoneScript targetZone = FindNearestZone();
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
	}

	ZoneScript FindNearestZone()
	{
		Vector3 pos = transform.position;
		return availableZones.OrderBy(z => (z.transform.position - pos).sqrMagnitude).First();
	}

	//  void OnDrawGizmos() {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(transform.position, 50f);
    // }
}
