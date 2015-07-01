using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour
{

	/// Use InputManager instead
	/// (rewrite because of 'pause handling', jump handling)


	//Audio Input
	public GameObject audioInputObject;
	MicHandler micIn;
	public float minVolume = 1.0f; //threshhold to ignore 'natural noise'
	public float maxVolume;
	public float jumpVolume;


	//Orientation
	public Renderer laneRenderer;
	float laneExtends;
	Vector3 laneCenter;
	float leftMaxX;
	float rightMaxX;
	float lX;
	float rX;
	float distance;

	//Groups that run on the left & right
	public List<Runner> lefties;
	public List<Runner> righties;
	public GameObject camDummy;

	//Timer
	float timer;
	float inputTime = 0.08f;
	bool acceptInput;

	//BPM stuffz
	public int maxBPM;
	public int minBPM;
	List<float> lastBangs;
	float timeSinceLastBang;
	public float jumpForce;

	//Singleton Instance
	public static InputHandler instance = null;

	//Movement handling
	private bool groupGrounded;

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
		lastBangs = new List<float> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//forward movement
		foreach (Runner runner in lefties) {
			runner.DummyZ = camDummy.transform.position.z;
		}
		foreach (Runner runner in righties) {
			runner.DummyZ = camDummy.transform.position.z;
		}


		float l = micIn.loudness;
		if (l > minVolume && l > jumpVolume || Input.GetKeyDown (KeyCode.M) && acceptInput) {
			//Jump Handling
			foreach (Runner runner in lefties) {
				runner.Jump ();
			}
			foreach (Runner runner in righties) {
				runner.Jump ();
			}
		}

		if ((l > minVolume || Input.GetKeyDown (KeyCode.Space)) && acceptInput) {
			acceptInput = false;
			timer = 0.0f;
			timeSinceLastBang = 0.0f;

			if (lastBangs.Count > 0) {


				//only check the 5 last bangs
				if (lastBangs.Count < 5) {
					lastBangs.Add (Time.time);
				} else {
					lastBangs.RemoveAt (0);
					lastBangs.Add (Time.time);
				}

				int bpm = this.CalcAverageBeatsPerMinute ();
				this.SetXInRunners (this.CalcDistanceToCenter (bpm));
			
			} else {
				lastBangs.Add (Time.time);
			}
		} else if (acceptInput) {
			timeSinceLastBang += Time.deltaTime;
		}

		//RhythmInputTimer
		if (!acceptInput && timer > inputTime) {
			acceptInput = true;
		} else if (!acceptInput) {
			timer += Time.deltaTime;
		}

	}

	void FixedUpdate ()
	{

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

	private int CalcAverageBeatsPerMinute ()
	{
		float sum = 0;
		int times = 0;
		for (int i = 1; i < lastBangs.Count; i++) {
			sum += lastBangs [i] - lastBangs [i - 1];
			times++;
		}
		float average = sum / times;
		
		return Mathf.RoundToInt (60.0f / average);
	}

	private float CalcDistanceToCenter (int currentBPM)
	{
		float percentage = Mathf.Clamp ((float)(currentBPM - minBPM), 0.1f, (float)(maxBPM - minBPM)) / (float)(maxBPM - minBPM);
		return percentage * laneExtends;
	}

	public void SetInAir (bool _inAir)
	{
		this.groupGrounded = _inAir;
	}

}
