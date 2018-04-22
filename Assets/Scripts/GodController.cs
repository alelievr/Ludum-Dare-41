using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GodController : MonoBehaviour
{
	public float		speed;
	public Transform	head;

	NavMeshAgent	agent;

	void Start ()
	{
		agent = GetComponent< NavMeshAgent >();
	}
	
	void Update ()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical"));

		input = Quaternion.AngleAxis(head.eulerAngles.y, Vector3.up) * input;

		agent.Move(input.normalized * speed);
	}
}
