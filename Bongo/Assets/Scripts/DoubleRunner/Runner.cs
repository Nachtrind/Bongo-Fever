using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour
{

	//Components
	private Rigidbody rigid;
	private Animator anim;

	//Movement Variables
	public float speed;
	float xSpeed;
	float stageSpeed;
	float jumpForce;
	private bool inAir;
	private bool doubleAir;
	private float _currentX;
	private float _dummyZ;

	//Reached End
	public Vector3 stagePos;
	public bool moveToStage;

	//Status Variables
	private bool dead;
	private bool reachedEnd;
	public bool active;
	public bool lefty;

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
		anim = this.GetComponentInChildren<Animator> ();
		speed = GameManager.instance.speed;
		stageSpeed = GameManager.instance.stageSpeed;
		xSpeed = GameManager.instance.xSpeed;
		jumpForce = InputManager.instance.jumpForce;
		offsetTime = Random.Range (4.0f, 10.0f);
		offsetTimer = offsetTime;
		if (active) {
			anim.StopPlayback ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.IsGrounded ();
		if (!dead && active && !reachedEnd) {
			if (offsetTimer > offsetTime) {
				this.xOffset = Random.Range (-0.5f, 0.5f);
				this.zOffset = Random.Range (-0.3f, 0.3f);
				offsetTimer = 0;
			} else {
				offsetTimer += Time.deltaTime;
			}
		}
	}

	void FixedUpdate ()
	{
		if (!dead && active && !reachedEnd && !moveToStage) {
			Vector3 target = new Vector3 (_currentX + xOffset, this.transform.position.y, _dummyZ + zOffset);
			Vector3 direction = target - this.transform.position;
			direction = direction.normalized;
			this.transform.position = new Vector3 (this.transform.position.x + (direction.x * xSpeed * Time.deltaTime), 
			                                       this.transform.position.y, 
			                                       _dummyZ + zOffset);
		}

		if (reachedEnd && moveToStage) {
			//if (this.transform.position.x != stagePos.x && this.transform.position.z != stagePos.z) {

			if (!(this.transform.position.x >= stagePos.x - 0.05f 
				&& this.transform.position.x <= stagePos.x + 0.05f 
				&& this.transform.position.z >= stagePos.z - 0.05f
				&& this.transform.position.z <= stagePos.z + 0.05f)) {

				Vector3 direction = stagePos - this.transform.position;
				direction = new Vector3 (direction.x, 0.0f, direction.z);
				direction = direction.normalized;

				this.transform.position = new Vector3 (this.transform.position.x + (direction.x * stageSpeed * Time.deltaTime), 
				                                       this.transform.position.y, 
				                                       this.transform.position.z + (direction.z * stageSpeed * Time.deltaTime));
			} else {
				moveToStage = false;
				anim.SetBool ("OnStage", true);
			}

			Ray ray = new Ray (this.transform.position, stagePos);
			if (Physics.Raycast (ray, 0.2f)) {
				this.Jump ();
			}
		}

	}

	public void Jump ()
	{

		if (!dead) {
			if (IsGrounded () || !inAir) {
				rigid.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
				inAir = true;
				doubleAir = false;
				anim.SetBool ("Jump", true);
			} else if (!doubleAir) {
				rigid.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
				doubleAir = true;
				anim.SetBool ("Jump", true);
			}
		}
	}

	public bool IsGrounded ()
	{
		bool grounded = false;
		Vector3 down = transform.TransformDirection (-Vector3.up);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, down, out hit, 0.25f)) {
			anim.SetBool ("Jump", false);
			anim.SetFloat ("DistanceToGround", hit.distance);
			grounded = true;
		}
		anim.SetFloat ("DistanceToGround", 0.0f);
		InputManager.instance.SetGroupGrounded (grounded);

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

	public void SetStagePos (Vector3 _stagePos)
	{
		this.stagePos = _stagePos;
		moveToStage = true;
		reachedEnd = true;
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.collider.tag.Equals ("Obstacle")) {
			dead = true;
			InputManager.instance.RemoveRunner (this);
			anim.SetBool ("Dead", true);
		}

		if (collision.collider.tag.Equals ("InactiveBongo")) {
			collision.collider.GetComponent<Runner> ().active = true;
			collision.collider.GetComponent<Runner> ().jumpForce = this.jumpForce;
			collision.collider.gameObject.layer = 8;
			collision.collider.gameObject.tag = "Bongo";
			if (this.lefty) {
				InputManager.instance.lefties.Add (collision.collider.GetComponent<Runner> ());
				collision.collider.GetComponent<Runner> ().lefty = true;
			} else {
				InputManager.instance.righties.Add (collision.collider.GetComponent<Runner> ());
				collision.collider.GetComponent<Runner> ().lefty = false;
			}
		}
	}
}
