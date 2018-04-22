using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FollowerController))]
public class FollowerControllerEditor : Editor
{

	FollowerController fc;

	private void OnEnable()
	{
		fc = target as FollowerController;
	}

	void OnSceneGUI()
	{
		Handles.Label(fc.transform.position + Vector3.up, "ST: " + fc.state);
	}

}