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
	bool			issoldat = false;
	GameObject		Cible = null;
	ZoneScript		zonesc;
	bool 			gotapos = false;

	GameObject		worldCanva;
	GameObject		progressBar;
	Scrollbar		slider;

	Camera			mainCam;

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
		GodEvent.cibleEvent += searchCibleCallback;
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
		if (fc.GetInstanceID() != this.GetInstanceID() && Vector3.Distance(fc.transform.position, transform.position) < distanceagro)
		{
			fc.Cible = this.gameObject;
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
		
		timesincelastime += Time.deltaTime;
		if (issoldat && timesincelastime > 2)
		{
			timesincelastime = 0;
			searchCibleCallback(this);
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
		}
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
}
