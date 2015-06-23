using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetDummy : MonoBehaviour
{


	public Transform spherepulse;
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
	public int maxBPM;
	public int minBPM;
	Vector3 target;
	
	// Use this for initialization
	void Start ()
	{
		micIn = (MicHandler)audioInputObject.GetComponent ("MicHandler");
		lastBangs = new List<float> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float l = micIn.loudness;
		if (l > minVolume && acceptInput) {
			Instantiate (spherepulse, transform.position, transform.rotation);
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
	

}
