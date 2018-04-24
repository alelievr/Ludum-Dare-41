using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBackground : MonoBehaviour
{
	public AudioClip	intro;
	public AudioClip	loop;

	AudioSource	audioSource;

	// Use this for initialization
	void Start ()
	{
		audioSource = GetComponent< AudioSource >();
		
		audioSource.clip = intro;
		audioSource.Play();

		StartCoroutine(RunLoop());
	}

	IEnumerator RunLoop()
	{
		yield return new WaitForSeconds(intro.length);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!audioSource.isPlaying)	
		{
			audioSource.clip = loop;
			audioSource.loop = true;
			audioSource.Play();
		}
	}
}
