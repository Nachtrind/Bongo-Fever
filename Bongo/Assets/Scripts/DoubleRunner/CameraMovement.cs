using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
	Rigidbody rigid;
	public Renderer laneRenderer;
	public Vector3 laneCenter;
	float speed;
	float yPos;

	// Use this for initialization
	void Start ()
	{
		this.laneCenter = laneRenderer.bounds.center;
		yPos = this.transform.position.y;
		this.rigid = GetComponent<Rigidbody> ();
		speed = GameManager.instance.speed;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (speed == 0.0f) {
			speed = GameManager.instance.speed;
		}
	}

	void FixedUpdate ()
	{
		this.rigid.MovePosition (this.transform.position + new Vector3 (0, 0, 1) * speed * Time.deltaTime);
	}
}
