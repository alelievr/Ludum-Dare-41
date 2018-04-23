using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public int followerNeeded;

	public int followerCount = 0;

	int i = 1;

	//public List< Transform > posTab = new List< Transform >();
	[HideInInspector]
	public List< Transform > posTab;

	// IEnumerator	SpawnProtect()
	// {
	// 	yield return new WaitForSeconds(1f);
	// 	if (posTab.Count != followerCount)
	// 		Destroy(this.gameObject);
	// 	yield break;
	// }

	private void Start()
	{
		posTab = new List< Transform>(GetComponentsInChildren<Transform>());
		followerNeeded = transform.childCount - 1;
		// if (zoneType == ZoneType.FollowerSpawn)
			// StartCoroutine(SpawnProtect());
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
		// if (zoneType == ZoneType.FollowerSpawn)
	}
}
