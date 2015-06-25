using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Dirty class for testing
public class RunnerDummy : MonoBehaviour
{

	public GameObject audioInputObject;
	public float speed;
	public float minVolume = 1.0f; //threshhold to ignore 'natural noise'
	public float maxVolume;
	float inputTimer;
	public float inputPauseTime;
	bool acceptInput;
	MicHandler micIn;
	float lastBangTime;
	List<float> lastBangs;
	public GameObject left;
	public GameObject right;
	public GameObject top;
	public GameObject bottom;
	float distanceLeftRight;
	float distanceTopBottom;
	public int maxBPM;
	public int minBPM;
	Vector3 target;

	// Use this for initialization
	void Start ()
	{
		micIn = (MicHandler)audioInputObject.GetComponent ("MicHandler");
		distanceLeftRight = Mathf.Abs (right.transform.position.z - left.transform.position.z);
		distanceTopBottom = Mathf.Abs (top.transform.position.y - bottom.transform.position.y);
		lastBangs = new List<float> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float l = micIn.loudness;
		if ((l > minVolume || Input.GetKeyDown (KeyCode.Space)) && acceptInput) {
			Debug.Log ("Loudness: " + l);
			acceptInput = false;

			//Horizontal Movement <--->
			if (lastBangs.Count > 0) {
				if (lastBangs.Count < 5) {
					lastBangs.Add (Time.time);
				} else {
					lastBangs.RemoveAt (0);
					lastBangs.Add (Time.time);
				}
				this.target = new Vector3 (this.transform.position.x, this.transform.position.y, this.CalcZPosition (this.CalcAverageBeatsPerMinute ()));
			} else {
				lastBangs.Add (Time.time);
			}

			//Vertical Movement
			this.target = new Vector3 (this.target.x, this.CalcYPosition (l), this.target.z);


		}

		if (!acceptInput && inputTimer < inputPauseTime) {
			inputTimer += Time.deltaTime;
		} else if (!acceptInput && inputTimer > inputPauseTime) {
			inputTimer = 0;
			acceptInput = true;
		}
	}//END Update


	void FixedUpdate ()
	{
		Vector3 direction = target - this.transform.position;
		direction = direction.normalized;

		this.GetComponent<Rigidbody> ().MovePosition (this.transform.position + direction.normalized * speed * Time.deltaTime);

	}

	public int CalcBeatsPerMinute (float timeA, float timeB)
	{
		return Mathf.RoundToInt (60.0f / (timeA - timeB));
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
	
	public float CalcZPosition (int currentBPM)
	{
		//Debug.Log ("BPM:" + currentBPM);
		float percentage = Mathf.Clamp ((float)(currentBPM - minBPM), 0.0f, (float)(maxBPM - minBPM)) / (float)(maxBPM - minBPM);
		//Debug.Log ("Percentage:" + percentage);
		return percentage * distanceLeftRight + left.transform.position.z;
	}

	public float CalcYPosition (float currentVolume)
	{
		float percentage = Mathf.Clamp ((float)(currentVolume - minVolume), 0.0f, (float)(maxVolume - minVolume)) / (float)(maxVolume - minVolume);
		return percentage * distanceTopBottom + bottom.transform.position.z;
	}



}
