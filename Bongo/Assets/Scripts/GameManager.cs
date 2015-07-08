using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState
{
	MainMenu,
	GameRunning,
	GamePaused
}

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	public GameState gameState { get; private set; }

	//In Game Variables
	public float speed;
	public float stageSpeed;
	public float xSpeed;

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
	}


}