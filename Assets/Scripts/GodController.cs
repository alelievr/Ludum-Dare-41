using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GodController : MonoBehaviour
{
	public float		speed;
	public Transform	head;
	public float		maxLife = 100;
	[HideInInspector]
	public float		life;

	NavMeshAgent	agent;

	private void Awake()
	{
		life = maxLife;
	}

	void Start ()
	{
		GodEvent.god = this;
		agent = GetComponent< NavMeshAgent >();
	}
	
	void Update ()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

		input = Quaternion.AngleAxis(head.eulerAngles.y, Vector3.up) * input;

		agent.Move(input.normalized * speed);

		if (life <= 0)
		{
			GameManager.instance.Lose();
		}
		// GameManager.instance.UpdateHP(life, maxLife); a remmettre

		// transform.eulerAngles = new Vector3(transform.eulerAngles.x, head.eulerAngles.y, transform.eulerAngles.z);
	}

	public void ouch(float damage)
	{
		life -= damage;
		if (life <= 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
