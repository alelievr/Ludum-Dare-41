using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(ZoneScript))]
public class ZoneScriptEditor : Editor
{
	ZoneScript	zoneScript;

	private void OnEnable()
	{
		zoneScript = target as ZoneScript;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		for (int i = 0; i < zoneScript.posTab.Count; i++)
		{
			zoneScript.posTab[i].position = EditorGUILayout.Vector3Field("pos #" + i, zoneScript.posTab[i].position);
		}
	}

	void OnSceneGUI()
	{
		if (zoneScript.posTab == null || zoneScript.posTab.Count == 0)
			return ;
		
		Vector3 zonePos = zoneScript.transform.position;
		float radiusMax = zoneScript.posTab.Max(v => (v.position - zonePos).magnitude);

		Handles.CircleHandleCap(0, zonePos, Quaternion.Euler(90, 0, 0), radiusMax, EventType.Repaint);
	}
}