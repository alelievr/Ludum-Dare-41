using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public enum FollowerState
{
	Idle,
	FollowGod,
	MovingToFarm,
	MovingToSpawn,
	MovingToAttack,
	Spawning,
	Farming,
	Attacking,
	Charging,
	MovingToArmory,
	StatingState,
}

[RequireComponent(typeof(NavMeshAgent))]
public class FollowerController : MonoBehaviour
{
	[Header("Attack")]
	float	shoutdist = 50f;
	public float	distanceagro = 2;
	public float	attackRange = 3;
	public float	attacktime = 1;
	public bool		issoldat = false;
	public float	life = 2;

	[Space, Header("Farming")]
	public float	farmDuration = 5;
	
	// [HideInInspector]
	public FollowerState	state = FollowerState.Idle;
	[HideInInspector]
	public float	farmProgress;


	[Space, Header("GUI")]
	public float	progressBarYOffset = 1;
	public float	progressBarHideDistance = 200;

	public GameObject	progressBarPrefab;

	Vector3			dir;
	[HideInInspector] public NavMeshAgent	agent;
	FollowerController fc = null;
	GameObject		Cible = null;
	ZoneScript		zonesc;
	bool 			gotapos = false;

	GameObject		worldCanva;
	GameObject		progressBar;
	Scrollbar		slider;

	Camera			mainCam;

	Vector3 oldDestination;
	FollowerState oldstate;

	public bool		badguys = false;
	Transform		godTrans;
	Utils			env;


	private void Start()
	{
		agent = GetComponent< NavMeshAgent >();
		worldCanva = GameObject.Find("WorldCanva");
		mainCam = Camera.main;
		env = mainCam.GetComponent<Utils>();


		if (worldCanva != null)
		{
			progressBar = Instantiate(progressBarPrefab, worldCanva.transform);
			slider = progressBar.GetComponentInChildren< Scrollbar >();
		}
	}

	// Use this for initialization
	private void OnEnable()
	{
		if (badguys == false)
		{
			GodEvent.listAllFollower.Add(this);
			GodEvent.farmEvent += FarmCallback;
			GodEvent.followEvent += FollowGodCallBack;
			GodEvent.spawnEvent += SpawnCallBack;
			GodEvent.stayEvent += StayCallBack;
		}
		else
		{
			GodEvent.listAllBadGuys.Add(this);
		}
	}

	Vector3 chargedest;

	public bool ToArmCallback(Vector3 pos, ZoneScript zone)
	{
		GodEvent.listAllFollowerFollowing.Remove(this);
		if (issoldat)
		{
			ChargeCallback(pos);
			return true;
		}
		chargedest = pos;
		zonesc = zone;
		Debug.Log("TO ARM!");
		state = FollowerState.MovingToArmory;
		agent.SetDestination(zonesc.transform.position);
		return true;
	}

	public void ChargeCallback(Vector3 pos)
	{	
		Debug.Log("Charge!");
		agent.SetDestination(pos);
		state = FollowerState.Charging;
	}

	void	FollowGodCallBack(Transform gt)
	{
		float dist = Vector3.Distance(transform.position, gt.position);

		if ((state == FollowerState.Idle || state == FollowerState.MovingToAttack || state == FollowerState.Attacking) && dist < shoutdist)
		{
			Debug.Log("FOLLOW THE GOD");
			state = FollowerState.FollowGod;
			godTrans = gt;
			agent.SetDestination(godTrans.position);
			// Debug.DrawLine(transform.position, godTrans.position, Color.red, 1f);

			Debug.DrawLine(transform.position, godTrans.position, Color.red, 1f);
			GodEvent.listAllFollowerFollowing.Add(this);
		}
	}

	void	StayCallBack(Vector3 pos)
	{
		float dist = Vector3.Distance(transform.position, pos);

		if (state == FollowerState.FollowGod && dist < shoutdist)
		{
			Debug.Log("I STAY HERE");
			state = FollowerState.Idle;
			agent.SetDestination(transform.position);
			GodEvent.listAllFollowerFollowing.Remove(this);
		}
	}

	void	FarmCallback(Vector3 godPos, ZoneScript zone)
	{
		zonesc = zone;
		if 	(Vector3.Distance(transform.position, godPos) < shoutdist)
		{
			if (/*(state == FollowerState.Idle || */state == FollowerState.FollowGod/*)*/ && zonesc.EmptySlot())
			{
				Debug.Log("JE SUIS FERMIER");
				dir = new Vector3(godPos.x - transform.position.x, godPos.y - transform.position.y, godPos.z - transform.position.z);
				float dist = dir.magnitude;

				state = FollowerState.MovingToFarm;
				GodEvent.listAllFollowerFollowing.Remove(this);
				agent.SetDestination(zonesc.NextFreePos());
			}
		}
	}

	void searchCibleCallback(GameObject fc)
	{
		if (fc.GetInstanceID() != this.GetInstanceID() && Vector3.Distance(fc.transform.position, transform.position) < distanceagro
			&& (Cible == null || Vector3.Distance(Cible.transform.position, transform.position) >= Vector3.Distance(fc.transform.position, transform.position)))
		{
			Cible = fc;
			state = FollowerState.MovingToAttack;
		}
	}

	void	SpawnCallBack(GodEvent godEvent, ZoneScript zone)
	{
		//	Debug.Log("JE SUIS  PRESK SPAWN");
		zonesc = zone;
		if (state == FollowerState.FollowGod && zonesc.EmptySlot())
		{
			Debug.Log("JE SUIS SPAWN");
			state = FollowerState.MovingToSpawn;
			GodEvent.listAllFollowerFollowing.Remove(this);
			agent.SetDestination(zonesc.NextFreePos());
		}
	}

	void searchCible()
	{
		oldstate = state;
		oldDestination = agent.destination;
		foreach(FollowerController fc in (badguys) ? GodEvent.listAllFollower : GodEvent.listAllBadGuys)
			searchCibleCallback(fc.gameObject);
		if (badguys)
			searchCibleCallback(GodEvent.god.gameObject);
	}

	public void upgradetosoldat()
	{
		issoldat = true;
		if (GetComponent<MeshRenderer>() == null)
			Debug.Log("dafuq");
		GetComponent<MeshRenderer>().material = env.soldatmat;
	}


	float timesincelastime = 0;
	// Update is called once per frame
	void Update ()
	{
		UpdateGUI();

		if (issoldat)
			UpdateSoldat();

		UpdateState();
	}

	void UpdateState()
	{
		switch (state)
		{
			case FollowerState.MovingToFarm:
				if (agent.remainingDistance < agent.stoppingDistance && Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance)
					StartFarming();
				break ;
			case FollowerState.Charging:
				if (agent.remainingDistance < agent.stoppingDistance)
					state = FollowerState.Idle;
				break ;
			case FollowerState.MovingToAttack:
				if (!Cible)
				{
					state = oldstate;
					break ;
				}
				agent.SetDestination(Cible.transform.position);
				if (agent.remainingDistance < attackRange)
					attackcible();
				break ;
			case FollowerState.FollowGod:
				agent.SetDestination(godTrans.position);
				break;
			case FollowerState.MovingToSpawn:
				if (agent.remainingDistance < agent.stoppingDistance)
					StartSpawning();
				break ;
			case FollowerState.MovingToArmory:
				if (agent.remainingDistance < agent.stoppingDistance || agent.remainingDistance < 3)
				{
					upgradetosoldat();
					ChargeCallback(chargedest);
				}
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
				GodController tmp3;
				if (tmp2 = Cible.GetComponent<FollowerController>())
					tmp2.ouch(1);
				else if (tmp3 = Cible.GetComponent<GodController>())
					tmp3.ouch(1);
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
		state = oldstate;
	}

	void StartFarming()
	{
		state = FollowerState.Farming;

		StartCoroutine("UpdateFarming");
	}

	void StartSpawning()
	{
		Debug.Log("start Spawn");
		if (!zonesc)
		{
			state = FollowerState.Idle;
			FollowGodCallBack(godTrans);
		}

		state = FollowerState.Spawning;

		// StartCoroutine("UpdateSpawning");
	}


	IEnumerator UpdateFarming()
	{
		while (true)
		{
			float t = Time.time;
			farmProgress = 0;
			while (Time.time - t < farmDuration)
			{
				farmProgress = ((Time.time - t) / farmDuration);
				yield return new WaitForEndOfFrame();
			}
			farmProgress = 1;

			GameManager.instance.AddCrystal(1);
		}
	}

	void UpdateGUI()
	{
		if (progressBar != null)
		{
			//update progress bar position
			progressBar.transform.position = transform.position + Vector3.up * progressBarYOffset;

			slider.size = farmProgress;

			if (godTrans != null && Vector3.Distance(transform.position, godTrans.position) > progressBarHideDistance)
				progressBar.SetActive(false);
			else
				progressBar.SetActive(farmProgress != 0);
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
		if (badguys)
			GodEvent.listAllBadGuys.Remove(this);
		else
		{
			GodEvent.farmEvent -= FarmCallback;
			GodEvent.followEvent -= FollowGodCallBack;
			GodEvent.stayEvent -= StayCallBack;
			GodEvent.listAllFollower.Remove(this);
			GodEvent.listAllFollowerFollowing.Remove(this);
		}
		GameObject.Destroy(gameObject);
	}

	void UpdateSoldat()
	{
		timesincelastime += Time.deltaTime;
		if (timesincelastime > 2)
		{
			timesincelastime = 0;
			searchCible();
		}
	}
}
