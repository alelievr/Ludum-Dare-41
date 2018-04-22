using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowerController : MonoBehaviour {

	bool	waiting = true;
	public float shoutdist = 20f;
	
	bool			goToDestination = false;
	Vector3			dir;
	Vector3			destination;
	NavMeshAgent	agent;

	private void Start()
	{
		agent = GetComponent< NavMeshAgent >();
	}

	// Use this for initialization
	private void OnEnable()
	{
		GodEvent.farmEvent += BuildFarm;
	}

	void	BuildFarm(Vector3 godPos)
	{
		if (waiting == true)
		{
			
			dir = new Vector3(godPos.x - transform.position.x, godPos.y - transform.position.y, godPos.z - transform.position.z);
			float dist = dir.magnitude;
			if (dist < shoutdist)
			{
				waiting = false;
				destination = godPos;
				goToDestination = true;
				agent.SetDestination(destination);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (goToDestination == true)
		{
			// VA LABAAA
			Debug.DrawLine(transform.position, destination, Color.blue ,1f);
		}
	}
}
