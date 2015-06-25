using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

	public Renderer laneRenderer;
	public Vector3 laneCenter;
	float yPos;

	// Use this for initialization
	void Start ()
	{
		this.laneCenter = laneRenderer.bounds.center;
		yPos = this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void FixedUpdate ()
	{
		this.transform.position = new Vector3 (laneCenter.x, yPos, this.transform.position.z);
	}
}
