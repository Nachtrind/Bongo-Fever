using UnityEngine;
using System.Collections;

public class BongoSounds : MonoBehaviour
{

	public AudioClip jump;
	public AudioClip landing;
	public AudioClip death;
	AudioSource aSource;

	// Use this for initialization
	void Start ()
	{
		this.aSource = this.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Jump ()
	{
		aSource.clip = jump;
		aSource.Play ();
		Debug.Log ("JUMPSOUND");
	}

	public void Landing ()
	{
		aSource.clip = landing;
		aSource.Play ();
	}

	public void Death ()
	{
		aSource.clip = death;
		aSource.Play ();
	}

}
