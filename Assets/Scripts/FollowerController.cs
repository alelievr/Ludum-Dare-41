using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public enum FollowerState
{
	Idle,
	MovingToFarm,
	MovingToAttack,
	Farming,
	Attacking,
}

[RequireComponent(typeof(NavMeshAgent))]
public class FollowerController : MonoBehaviour
{
	[Header("Attack")]
	public float	shoutdist = 50f;
	public float	distanceagro = 2;
	public float	attackRange = 3;
	public float	attacktime = 1;
	public bool		issoldat = false;
	public float	life = 2;

	[Space, Header("Farming")]
	public float	farmDuration = 5;
	
	[HideInInspector]
	public FollowerState	state = FollowerState.Idle;
	[HideInInspector]
	public float	farmProgress;

	[Space, Header("GUI")]
	public float	progressBarYOffset = 1;

	public GameObject	progressBarPrefab;

	Vector3			dir;
	NavMeshAgent	agent;
	FollowerController fc = null;
	GameObject		Cible = null;
	ZoneScript		zonesc;
	bool 			gotapos = false;

	GameObject		worldCanva;
	GameObject		progressBar;
	Scrollbar		slider;

	Camera			mainCam;

	Vector3 oldDestination;


	private void Start()
	{
		agent = GetComponent< NavMeshAgent >();
		worldCanva = GameObject.Find("WorldCanva");

		mainCam = Camera.main;

		if (worldCanva != null)
		{
			progressBar = Instantiate(progressBarPrefab, worldCanva.transform);
			slider = progressBar.GetComponentInChildren< Scrollbar >();
		}
	}

	// Use this for initialization
	private void OnEnable()
	{
		GodEvent.farmEvent += FarmCallback;
		GodEvent.listAllFollower.Add(this);
	}

	void	FarmCallback(Vector3 godPos, ZoneScript zone)
	{
		zonesc = zone;

		if (state == FollowerState.Idle && zonesc.EmptySlot())
		{
			dir = new Vector3(godPos.x - transform.position.x, godPos.y - transform.position.y, godPos.z - transform.position.z);
			float dist = dir.magnitude;

			state = FollowerState.MovingToFarm;
			agent.SetDestination(zonesc.NextFreePos());
		}
	}

	void searchCibleCallback(FollowerController fc)
	{
		Debug.Log("dsad3");
		if (fc.GetInstanceID() != this.GetInstanceID() && Vector3.Distance(fc.transform.position, transform.position) < distanceagro
			&& (Cible == null || Vector3.Distance(Cible.transform.position, transform.position) >= Vector3.Distance(fc.transform.position, transform.position)))
		{
			Debug.Log("dsad4");
			Cible = fc.gameObject;
			state = FollowerState.MovingToAttack;
			oldDestination = agent.destination;
		}
	}

	void searchCible()
	{
		Debug.Log("dsad");
		foreach(FollowerController fc in GodEvent.listAllFollower)
		{
			searchCibleCallback(fc);
		}
	}

	float timesincelastime = 0;
	// Update is called once per frame
	void Update ()
	{
		UpdateState();

		UpdateGUI();

		// if (agent.remainingDistance < 2f && state == FollowerState.Moving && gotapos == false)
		// {
		// 	agent.SetDestination(zonesc.NextFreePos());
		// 	gotapos = true;
		// }

		// parti soldat
		
		if (!issoldat)
			return ;
		timesincelastime += Time.deltaTime;
		if (timesincelastime > 2)
		{
			timesincelastime = 0;
			searchCible();
		}
	}

	void UpdateState()
	{
		switch (state)
		{
			case FollowerState.MovingToFarm:
				if (agent.remainingDistance < agent.stoppingDistance)
					StartFarming();
				break ;

			case FollowerState.MovingToAttack:
			agent.SetDestination(Cible.transform.position);
				if (agent.remainingDistance < attackRange)
					attackcible();
				break ;
		}
	}

	void attackcible()
	{
		Debug.Log("startAttacking!!!!!!");

		state = FollowerState.Attacking;

		StartCoroutine("UpdateAttacking");
	}


	IEnumerator UpdateAttacking()
	{
		float t = Time.time;
		Vector3 dirattack = (Cible.transform.position - transform.position).normalized;
		Vector3 originalpos = transform.position;
		float tmp;
		bool asattacked = false;

		while ((tmp = Time.time - t) < attacktime)
		{
			tmp = (tmp - attacktime / 2) / (attacktime / 2);
			tmp = (tmp < 0) ? tmp + 1 : 1 - tmp;
			tmp *= 4;
			if (tmp > 1)
				tmp = 0;
			// Debug.Log(tmp + " " + (Time.time - t));
			if (asattacked == false &&  tmp > 0)
			{
				FollowerController tmp2;
				if (tmp2 = Cible.GetComponent<FollowerController>())
					tmp2.ouch(1);
				asattacked = true;
			}
			transform.position = originalpos + dirattack * tmp;
			yield return new WaitForEndOfFrame();
		}
		{
			if (!Cible)
				Cible = null;
			agent.SetDestination(oldDestination);
		}
		state = FollowerState.Idle;
	}

	void StartFarming()
	{
		Debug.Log("startFarm");
		farmProgress = 0;

		state = FollowerState.Farming;

		StartCoroutine("UpdateFarming");
	}

	IEnumerator UpdateFarming()
	{
		float t = Time.time;

		while (Time.time - t < farmDuration)
		{
			farmProgress = ((Time.time - t) / farmDuration);
			yield return new WaitForEndOfFrame();
		}
		farmProgress = 1;

		state = FollowerState.Idle;
	}

	void UpdateGUI()
	{
		if (progressBar != null)
		{
			//update progress bar position
			progressBar.transform.position = transform.position + Vector3.up * progressBarYOffset;

			slider.size = farmProgress;
		}
	}

	public void ouch(float damage)
	{
		Debug.Log("ouch");
		life -= damage;
		if (life <= 0)
			autodestruct();
	}

	void autodestruct()
	{
		Debug.Log("autodestruct");
		GodEvent.farmEvent -= FarmCallback;
		GodEvent.listAllFollower.Remove(this);
		GameObject.Destroy(gameObject);
	}
}
