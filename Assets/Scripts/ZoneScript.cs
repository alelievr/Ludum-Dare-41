using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ZoneType
{
	Farm,
	FollowerSpawn,
	Armory,
}

[ExecuteInEditMode]
public class ZoneScript : MonoBehaviour
{
	public ZoneType	zoneType;
	public GameObject	follower;

	public int followerNeeded;

	public int followerCount = 0;
	public float spawnDelay = 5f;

	[Space]
	public GameObject	selectedGO;

	Transform		godTransform;

	int				i = 1;
	bool			selected;

	ParticleSystem	selectedParticles;

	//public List< Transform > posTab = new List< Transform >();
	[HideInInspector]
	public List< Transform > posTab;

	private void Start()
	{
		posTab = new List< Transform>(GetComponentsInChildren<Transform>());
		followerNeeded = transform.childCount - 1;

		Debug.Log("les chiffer " + posTab.Count + "ledezim" + followerNeeded);
		if (zoneType == ZoneType.FollowerSpawn)
			StartCoroutine(Spawn());

		godTransform = GameObject.Find("God").transform;

		selectedParticles = GetComponent< ParticleSystem >();
	}

	IEnumerator	Spawn()
	{
		while (true)
		{
			var go = Instantiate(follower, transform.position , transform.rotation);
			var agent = go.GetComponent< NavMeshAgent >();
			agent.Move(go.transform.forward * 2);

			yield return new WaitForSeconds(spawnDelay);
		}
	}

	public Vector3 NextFreePos()
	{
		Vector3 pos = posTab[i].position + new Vector3(2, 0, 0);
		i++;
		return (pos);
	}

	public bool EmptySlot()
	{
		followerCount++;
		return posTab.Count > followerCount;
	}

	void Update ()
	{
		selected = Vector3.Distance(godTransform.position, transform.position) < GodEvent.farmEventMaxDistance;

		if (selectedGO != null)
			selectedGO.SetActive(selected);

		if (selectedParticles != null)
		{
			var emi = selectedParticles.emission;
			emi.enabled = selected;
		}
	}
}
