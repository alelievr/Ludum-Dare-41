using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
	bool		win = false;


	void Start ()
	{
		GameManager.instance.AddCrystal(300);
	}

	private void Update()
	{
		if (GodEvent.listAllBadGuys.All(b => b == null) || GodEvent.listAllBadGuys.Count == 0)
		{
			GameManager.instance.Win();
			win = true;
		}

		if (win && Input.GetKeyDown(KeyCode.Return))
			SceneManager.LoadScene("level 1");
	}
}
