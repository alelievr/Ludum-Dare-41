using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneScript : MonoBehaviour {

	public int followerNeeded = 30;
	static int i = 0;
	static int j = 0;
	Vector3[] posTab;
	public Transform[] trans;

	void Start () {
		int x = 0;
		posTab = new Vector3[followerNeeded];
		trans = GetComponentsInChildren<Transform>();
		foreach (Transform tr in trans)
		{
			posTab[x] = tr.position;
			x++;
		}
	}
	
	public Vector3 NextFreePos()
	{
		Vector3 pos = posTab[i];
		Debug.Log(i);
		i++;
		return (pos);
	}

	public bool EmptySlot()
	{
		if (j < followerNeeded)
		{
			j++;
			return (true);
		}
		else 
			return (false);
	}

	void Update () {
		
	}

	// void OnDrawGizmos() 
	// {
	// 	foreach(Vector3 pos in posTab)
	// 	{
    //   	  Gizmos.color = Color.red;
    //   	  Gizmos.DrawSphere(pos, 1);
	// 	}
    // }
}
