using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour
{

	//Components
	private Rigidbody rigid;

	//Movement Variables
	public float speed;
	float jumpForce;
	private bool inAir;
	private bool doubleAir;
	private float _currentX;

	//Status Variables
	private bool dead;
	private bool reachedEnd;

	//Timer


	// Use this for initialization
	void Start ()
	{
		rigid = this.GetComponent<Rigidbody> ();
		speed = 10.0f;
		jumpForce = 5.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.IsGrounded ();
	}

	void FixedUpdate ()
	{

		Vector3 direction = new Vector3 (_currentX, 0.0f, this.transform.position.z + 5.0f);
		direction = direction - this.transform.position;
		direction.y = 0.0f;
		direction = direction.normalized;
		this.rigid.MovePosition (this.transform.position + direction * speed * Time.deltaTime);
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


}
