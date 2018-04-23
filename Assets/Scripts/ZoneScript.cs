using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoneType
{
	Farm,
	FollowerSpawn,
	// Attack,
}

[ExecuteInEditMode]
public class ZoneScript : MonoBehaviour
{
	public ZoneType	zoneType;
	public GameObject	follower;

	public int followerNeeded;

	public int followerCount = 0;
	public float spawnDelay = 5f;

	int i = 1;

	//public List< Transform > posTab = new List< Transform >();
	[HideInInspector]
	public List< Transform > posTab;

	IEnumerator	Spawn()
	{
		while (true)
		{
				Instantiate(follower, transform.position , transform.rotation);
			yield return new WaitForSeconds(spawnDelay);
		}
	}

	private void Start()
	{
		posTab = new List< Transform>(GetComponentsInChildren<Transform>());
		followerNeeded = transform.childCount - 1;

		Debug.Log("les chiffer " + posTab.Count + "ledezim" + followerNeeded);
		if (zoneType == ZoneType.FollowerSpawn)
			StartCoroutine(Spawn());
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

	void Update () {
		
	// switch (zoneType)
	// {
	// 	case ZoneType.FollowerSpawn :
	// 		break;
		
	// }
	}
}
