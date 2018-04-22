using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GodController : MonoBehaviour
{
	public float	speed;

	NavMeshAgent	agent;

	void Start ()
	{
		agent = GetComponent< NavMeshAgent >();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical"));

		agent.Move(input.normalized * speed);
	}
}
