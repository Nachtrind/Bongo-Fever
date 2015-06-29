using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
	Rigidbody rigid;
	public Renderer laneRenderer;
	public Vector3 laneCenter;
	float speed;
	float yPos;
	bool reachedEnd;
	public Transform finishLine;

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
		if (!reachedEnd) {
			this.rigid.MovePosition (this.transform.position + new Vector3 (0, 0, 1) * speed * Time.deltaTime);
		}

		if (this.transform.position.z >= finishLine.position.z && !reachedEnd) {
			reachedEnd = true;
			InputManager.instance.ReachedEnd ();
		}
	}


}
