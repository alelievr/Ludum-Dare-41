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
	FollowerController fc = null;
	bool issoldat = false;
	public float distanceagro = 2;
	GameObject Cible = null;
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
		GodEvent.cibleEvent += searchCible;
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

	void searchCible(FollowerController fc)
	{
		if (fc.GetInstanceID() != this.GetInstanceID() && Vector3.Distance(fc.transform.position, transform.position) < distanceagro)
		{
			fc.Cible = this.gameObject;
		}
	}

	float timesincelastime = 0;
	// Update is called once per frame
	void Update () {
		if (agent.remainingDistance < 2f && waiting == false && gotapos == false)
		{
			agent.SetDestination(zonesc.NextFreePos());
			gotapos = true;
		}
		timesincelastime += Time.deltaTime;
		if (issoldat && timesincelastime > 2)
		{
			timesincelastime = 0;
			searchCible(this);
		}
		// if (goToDestination == true)
		// {
			// VA LABAAA
			// Debug.DrawLine(transform.position, destination, Color.blue ,1f);
		// }
	}
}
