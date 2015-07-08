using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum GameState
{
	MainMenu,
	GameRunning,
	GamePaused,
	GameOver,
	GameWon
}

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	public GameState gameState { get; private set; }

	//In Game Variables
	public float speed;
	public float stageSpeed;
	public float xSpeed;
	AudioSource audioS;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) { 
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		gameState = GameState.GamePaused;
		InitGame ();
		audioS = this.GetComponent<AudioSource> (); 
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

	public void SetGameState (GameState gameState)
	{
		this.gameState = gameState;
		//TODO: Handle Changes accordingly

		//Game Running
		if (gameState == GameState.GameRunning) {
			audioS.Play ();
			GameObject text = GameObject.Find ("LevelStart");
			text.SetActive (false);
		}

		//Game Over
		if (gameState == GameState.GameOver) {
			GameObject gameOver = GameObject.Find ("GameOver");
			Debug.Log (gameOver);
			gameOver.GetComponent<Text> ().enabled = true;
		}


	}


}