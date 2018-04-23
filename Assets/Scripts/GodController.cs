using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GodController : MonoBehaviour
{
	public float		speed;
	public Transform	head;
	public float life = 100;

	NavMeshAgent	agent;

	void Start ()
	{
		GodEvent.god = this;
		agent = GetComponent< NavMeshAgent >();
	}
	
	void Update ()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical"));

		input = Quaternion.AngleAxis(head.eulerAngles.y, Vector3.up) * input;

		agent.Move(input.normalized * speed);
	}

	public void ouch(float damage)
	{
		life -= damage;
		if (life <= 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
