using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	int		crystalCount = 0;

	public static GameManager instance;

	private void Awake()
	{
		instance = this;
	}

	public void Win()
	{
		GUIManager.instance.ShowWinScreen();
	}

	public void Lose()
	{
		GUIManager.instance.ShowLoseScreen();
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void UpdateHP(float life, float maxLife)
	{
		GUIManager.instance.UpdateHP(life, maxLife);
	}

	public void AddCrystal(int count)
	{
		crystalCount += count;
	}

	public bool ConsumeCrystal(int amount)
	{
		if (crystalCount >= amount)
		{
			crystalCount -= amount;
			return true;
		}
		return false;
	}

	private void Update()
	{
		GUIManager.instance.UpdateCrystalCount(crystalCount);
		GUIManager.instance.UpdateMinionCount(0);
	}

}
