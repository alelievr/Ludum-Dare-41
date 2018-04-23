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

	public delegate void SpawnDelegate(GodEvent godEvent, ZoneScript zoneSc);
	public static event SpawnDelegate spawnEvent;
	public GameObject spawnZone;

	public delegate void searchCible(FollowerController fc);
	public static event searchCible cibleEvent;
	public  ParticleSystem pscharge;

	ZoneScript[]		availableZones;

	[HideInInspector] public static List<FollowerController> listAllFollower = new List<FollowerController>();
	[HideInInspector] public static List<FollowerController> listAllBadGuys = new List<FollowerController>();
	[HideInInspector] public static List<FollowerController> listAllFollowerFollowing = new List<FollowerController>();
	[HideInInspector] public int money = 200;
	[HideInInspector] public int moneygainby5s = 20;
	[HideInInspector] public int soldatupgradecost = 50;
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
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (spawnEvent != null &&  GodEvent.listAllFollowerFollowing.Count > 30)
			{
				Debug.Log("SPAWN HERE");
				ZoneScript targetZone = Instantiate(spawnZone, transform.position, transform.rotation).GetComponent<ZoneScript>();
				spawnEvent(GetComponent<GodEvent>(), targetZone);
			}
<<<<<<< HEAD
			if (Input.GetMouseButtonDown(0))
=======
		}
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
>>>>>>> cb67ac2382d7b171bd37b8fe71ffa85dca50a9f7
			{
				RaycastHit hit;
				if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
				{
					GameObject.Instantiate(pscharge, hit.point, Quaternion.identity);
					Debug.Log(listAllFollowerFollowing.Count);
					int i = 0;
					while(i < listAllFollowerFollowing.Count)
					{
						if (listAllFollowerFollowing[i].ChargeCallback(hit.point) == false)
									i++;
					}
					Debug.Log(listAllFollowerFollowing.Count);
				}
<<<<<<< HEAD
=======
				Debug.Log(listAllFollowerFollowing.Count);
			}
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			int i = 0;
			while(i < listAllFollowerFollowing.Count && i < 10)
			{
				if (money - soldatupgradecost < 0)
					break ;
				money -= soldatupgradecost;
				listAllFollowerFollowing[i].upgradetosoldat();
				i++;
>>>>>>> cb67ac2382d7b171bd37b8fe71ffa85dca50a9f7
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
<<<<<<< HEAD
}
=======

	void OnGUI()
    {
        // Camera  c = Camera.main;
        // Event   e = Event.current;

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.


        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("money: " + money);
        GUILayout.EndArea();
    }
}
>>>>>>> cb67ac2382d7b171bd37b8fe71ffa85dca50a9f7
