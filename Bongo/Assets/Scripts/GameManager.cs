using UnityEngine;
using System.Collections;
using System.Collections.Generic;
	
public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	//In Game Variables
	public float speed;
	public float xSpeed;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) { 
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		InitGame ();
	}

	void InitGame ()
	{
		/*if (Application.loadedLevelName != "MainMenu") {
			LoadLevel ("MainMenu");
		}*/
	}

	public void LoadLevel (string levelName)
	{
		Application.LoadLevel (levelName);
	}

}