using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour
{

	//Components
	private Rigidbody rigid;

	//Movement Variables
	public float speed;
	float xSpeed;
	float jumpForce;
	private bool inAir;
	private bool doubleAir;
	private float _currentX;
	private float _dummyZ;

	//Status Variables
	private bool dead;
	private bool reachedEnd;

	//Group offset for a more natural movement
	private float xOffset;
	private float zOffset;

	//Timer
	float offsetTimer;
	float offsetTime;

	// Use this for initialization
	void Start ()
	{
		rigid = this.GetComponent<Rigidbody> ();
		speed = GameManager.instance.speed;
		xSpeed = GameManager.instance.xSpeed;
		jumpForce = 5.0f;
		offsetTime = Random.Range (5.0f, 10.0f);
		offsetTimer = offsetTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.IsGrounded ();

		if (offsetTimer > offsetTime) {
			this.xOffset = Random.Range (-0.5f, 0.5f);
			this.zOffset = Random.Range (-0.3f, 0.3f);
			offsetTimer = 0;
		} else {
			offsetTimer += Time.deltaTime;
		}

	}

	void FixedUpdate ()
	{
		Vector3 target = new Vector3 (_currentX + xOffset, this.transform.position.y, _dummyZ + zOffset);
		Vector3 direction = target - this.transform.position;
		direction = direction.normalized;
		Debug.Log (xSpeed);
		this.transform.position = new Vector3 (this.transform.position.x + (direction.x * xSpeed * Time.deltaTime), this.transform.position.y, _dummyZ + zOffset);
	}

	public void Jump ()
	{
		if (IsGrounded () || !inAir) {
			rigid.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			inAir = true;
			doubleAir = false;
		} else if (!doubleAir) {
			rigid.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			doubleAir = true;
		}
	}

	public bool IsGrounded ()
	{
		bool grounded = false;
		Vector3 down = transform.TransformDirection (-Vector3.up);
		if (Physics.Raycast (transform.position, down, 0.25f)) {
			grounded = true;
		}
		return grounded;
	}

	public float CurrentX {
		set { 
			this._currentX = value; 
		}
		get { 
			return this._currentX; 
		}
	}

	public float DummyZ {
		set { 
			this._dummyZ = value; 
		}
		get { 
			return this._dummyZ; 
		}
	}
}
