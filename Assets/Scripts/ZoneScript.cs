using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoneType
{
	Farm,
	// Attack,
}

[ExecuteInEditMode]
public class ZoneScript : MonoBehaviour
{
	public ZoneType	zoneType;

	public int followerNeeded;

	public int followerCount = 0;

	int i = 1;

	//public List< Transform > posTab = new List< Transform >();
	public List< Transform > posTab;

	private void Start()
	{
		posTab = new List< Transform>(GetComponentsInChildren<Transform>());
		followerNeeded = transform.childCount - 1;
	}

	public Vector3 NextFreePos()
	{
		Vector3 pos = posTab[i].position + new Vector3(5, 0, 0);
		i++;
		return (pos);
	}

	public bool EmptySlot()
	{
		followerCount++;
		return posTab.Count >= followerCount;
	}

	void Update () {
		
	}
}
