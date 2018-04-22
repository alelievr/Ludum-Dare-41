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

	public int followerNeeded = 30;

	public int followerCount = 0;

	static int i = 0;

	public List< Transform > posTab = new List< Transform >();

	public Vector3 NextFreePos()
	{
		Vector3 pos = posTab[i].position;
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
