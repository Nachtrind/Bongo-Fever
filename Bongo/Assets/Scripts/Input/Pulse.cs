using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour
{

	float currentScale;
	public float maxScale;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (currentScale < maxScale) {
			currentScale += Time.deltaTime;
			this.transform.localScale = new Vector3 (currentScale, currentScale, currentScale);
		} else {
			Destroy (this);
		}
	}
}
