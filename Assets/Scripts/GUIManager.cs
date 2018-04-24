using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
	public GameObject	loseScreen;
	public GameObject	winSceen;

	[Space]
	public Text			hpText;
	public Text			crystalsText;
	public Text			minionCountText;
	public Scrollbar	hpSlider;

	public static GUIManager	instance;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		UpdateMinionCount(0);
		UpdateHP(1, 1);
		UpdateCrystalCount(0);
	}

	public void ShowWinScreen()
	{
		loseScreen.SetActive(true);
	}

	public void ShowLoseScreen()
	{
		loseScreen.SetActive(true);
	}

	public void UpdateMinionCount(int count)
	{
		Debug.Log(GodEvent.listAllFollowerFollowing.Count);
		minionCountText.text = "Minions: " + GodEvent.listAllFollowerFollowing.Count;
	}

	public void UpdateHP(float hp, float maxHP)
	{
		hpText.text = "hp: " + hp;
		hpSlider.size = hp / maxHP;
	}

	public void UpdateCrystalCount(int crystals)
	{
		crystalsText.text = "Crystals: " + crystals;
	}

}
