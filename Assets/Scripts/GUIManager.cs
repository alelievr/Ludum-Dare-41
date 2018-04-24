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

	public GameObject		god;
	GodController gc;


	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		gc = god.GetComponent<GodController>();
		UpdateMinionCount(0);
		UpdateHP(gc.life, gc.maxLife);
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
		hpText.text = "hp: " + gc.life;
		hpSlider.size = gc.life / gc.maxLife;
	}

	public void UpdateCrystalCount(int crystals)
	{
		crystalsText.text = "Crystals: " + crystals;
	}

}
