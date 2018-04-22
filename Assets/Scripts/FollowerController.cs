using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowerController : MonoBehaviour {

	bool	waiting = true;
	public float shoutdist = 50f;
	
	bool			goToDestination = false;
	Vector3			dir;
	Vector3			destination;
	NavMeshAgent	agent;
	ZoneScript		zonesc;
	bool 			gotapos = false;

	private void Start()
	{
		agent = GetComponent< NavMeshAgent >();
	}

	// Use this for initialization
	private void OnEnable()
	{
		GodEvent.farmEvent += BuildFarm;
	}

	void	BuildFarm(Vector3 godPos, GameObject zone)
	{
		zonesc = zone.GetComponent<ZoneScript>();
		if (waiting == true && zonesc.EmptySlot() == true)
		{
			dir = new Vector3(godPos.x - transform.position.x, godPos.y - transform.position.y, godPos.z - transform.position.z);
			float dist = dir.magnitude;
			if (dist < shoutdist)
			{
				waiting = false;
				destination = godPos;
				// goToDestination = true;
				agent.SetDestination(destination);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (agent.remainingDistance < 2f && waiting == false && gotapos == false)
		{
			agent.SetDestination(zonesc.NextFreePos());
			gotapos = true;
		}
		// if (goToDestination == true)
		// {
			// VA LABAAA
			// Debug.DrawLine(transform.position, destination, Color.blue ,1f);
		// }
	}
}
