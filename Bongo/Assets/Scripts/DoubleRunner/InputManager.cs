using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{

	//Audio Input
	public GameObject audioInputObject;
	MicHandler micIn;
	public float minVolume = 1.0f; //threshhold to ignore 'natural noise'
	public float jumpVolume;

	//Status
	bool reachedEnd;
	
	//Orientation
	public Renderer laneRenderer;
	float laneExtends;
	Vector3 laneCenter;
	float leftMaxX;
	float rightMaxX;
	float lX;
	float rX;
	float distance;
	public float subDistance;
	public float addDistance;
	
	//Groups that run on the left & right
	public List<Runner> lefties;
	public List<Runner> righties;
	public GameObject camDummy;
	
	//Timer
	float timer;
	float inputTime = 0.08f;
	bool acceptInput;
	float nonBangTime = 0.2f;
	
	//Movement
	float timeSinceLastBang;
	public float jumpForce;
	private bool reverse;
	
	//Singleton Instance
	public static InputManager instance = null;
	
	//Movement handling
	private bool groupGrounded;
	public Stage stage;
	
	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) { 
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}
	
	
	// Use this for initialization
	void Start ()
	{
		micIn = (MicHandler)audioInputObject.GetComponent ("MicHandler");
		this.laneCenter = laneRenderer.bounds.center;
		laneExtends = laneRenderer.bounds.extents.x * 0.9f;
		leftMaxX = laneCenter.x - laneExtends;
		rightMaxX = laneCenter.x + laneExtends;
		acceptInput = true;
		reverse = false;
		distance = laneExtends * 0.5f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float l = micIn.loudness;

		if (GameManager.instance.gameState == GameState.GameRunning) {
			if (!reachedEnd) {
				//forward movement
				foreach (Runner runner in lefties) {
					runner.DummyZ = camDummy.transform.position.z;
				}
				foreach (Runner runner in righties) {
					runner.DummyZ = camDummy.transform.position.z;
				}

				//Jump
				if (l > minVolume && l > jumpVolume || Input.GetKeyDown (KeyCode.M)) {
					//Jump Handling
					foreach (Runner runner in lefties) {
						runner.Jump ();
					}
					foreach (Runner runner in righties) {
						runner.Jump ();
					}
				}

				//Left-Right
				if ((l > minVolume || Input.GetKeyDown (KeyCode.Space)) && acceptInput && groupGrounded) {

					acceptInput = false;
					reverse = false;
					timer = 0.0f;
					distance += addDistance; 
					distance = Mathf.Clamp (distance, 0.1f, laneExtends);
					timeSinceLastBang = 0.0f;
					SetXInRunners (distance);

				} else if (timeSinceLastBang > nonBangTime) {
					reverse = true;
				} else {
					timeSinceLastBang += Time.deltaTime;
				}

				//Movement toward edges if nothing happens
				if (reverse && groupGrounded) {
					distance -= subDistance * Time.deltaTime; 
					distance = Mathf.Clamp (distance, 0.1f, laneExtends);
					SetXInRunners (distance);
				}
			}

		}

		//Game Paused (start of a level)
		if (GameManager.instance.gameState == GameState.GamePaused) {

			//Bange the drum to start
			if (l > minVolume && l > jumpVolume || Input.GetKeyDown (KeyCode.M)) {
				GameManager.instance.SetGameState (GameState.GameRunning);
			}

		}

		//InputTimer
		if (!acceptInput && timer > inputTime) {
			acceptInput = true;
		} else if (!acceptInput) {
			timer += Time.deltaTime;
		}

	}
	
	private void SetXInRunners (float _distanceToCenter)
	{
		foreach (Runner runner in lefties) {
			runner.CurrentX = leftMaxX + _distanceToCenter;
		}
		foreach (Runner runner in righties) {
			runner.CurrentX = rightMaxX - _distanceToCenter;
		}
		
	}
	
	public void SetGroupGrounded (bool _grounded)
	{
		this.groupGrounded = _grounded;
	}

	public void RemoveRunner (Runner _runner)
	{
		if (_runner.lefty) {
			this.lefties.Remove (_runner);
		} else {
			this.righties.Remove (_runner);
		}

		if (lefties.Count <= 0 && righties.Count <= 0) {
			Debug.Log ("GAME OVER");
		}
	}

	public void ReachedEnd ()
	{
		reachedEnd = true;
		stage.SetStagePos (lefties, righties);

	}

	/*
	public void AddRunner (Runner _runner)
	{

		if (lefties.Count <= righties.Count) {
			lefties.Add (_runner);
		} else {
			righties.Add (_runner);
		}

	}*/
	
}
