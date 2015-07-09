using UnityEngine;
using System.Collections;

public class WomanScript : MonoBehaviour
{
	private bool active;
	public Transform targetPoint;
	private bool reachedTarget;
	public float speed;
	private Animator anim;
	private int currentDance;
	public Material[] dresses;
	private float timer;

	// Use this for initialization
	void Start ()
	{
		currentDance = 1;
		anim = this.GetComponentInChildren<Animator> ();
		anim.SetInteger ("State", currentDance);
		if (this.GetComponentInChildren<Renderer> () != null) {
			this.GetComponentInChildren<Renderer> ().material = dresses [Random.Range (0, dresses.Length - 1)];
		} else {
			Debug.Log ("Oh no, no renderer!");
		}

		reachedTarget = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.transform.position.x >= targetPoint.position.x - 0.05f 
			&& this.transform.position.x <= targetPoint.position.x + 0.05f 
			&& this.transform.position.z >= targetPoint.position.z - 0.05f
			&& this.transform.position.z <= targetPoint.position.z + 0.05f
			&& !reachedTarget) {

			reachedTarget = true;
		}

		if (active && reachedTarget) {
			timer += Time.deltaTime;
		}

		if (timer >= 3.0f) {
			timer = 0.0f;
			this.currentDance = Random.Range (1, 2);
			anim.SetInteger ("State", currentDance);
		}

	}

	void FixedUpdate ()
	{
		if (active && !reachedTarget) {

			Vector3 dir = targetPoint.position + new Vector3 (0.0f, 1.0f, 0.0f) - this.transform.position + new Vector3 (0.0f, 1.0f, 0.0f);
			Ray ray = new Ray (this.transform.position + new Vector3 (0.0f, 1.0f, 0.0f), dir.normalized * speed);
			RaycastHit hit;
			Debug.DrawRay (this.transform.position + new Vector3 (0.0f, 1.0f, 0.0f), dir, Color.blue, 5.0f);
			if (Physics.Raycast (ray, out hit, 0.2f)) {
				if (hit.distance < 0.2) {
					Debug.Log ("Something is blocking my way, I stop and dance now");
					this.currentDance = Random.Range (1, 2);
					anim.SetInteger ("State", currentDance);
					reachedTarget = true;
				}
			} else {
				/*rigid.MovePosition (new Vector3 (this.transform.position.x + dir.normalized.x * speed * Time.deltaTime, 
				                                this.transform.position.y,
				                                this.transform.position.z + dir.normalized.z * speed * Time.deltaTime));*/
				this.transform.position = new Vector3 (this.transform.position.x + dir.normalized.x * speed * Time.deltaTime, 
			                                      this.transform.position.y,
			                                      this.transform.position.z + dir.normalized.z * speed * Time.deltaTime);
			}
		}
	}

	public void Activate ()
	{
		Debug.Log ("ACTIVATED WOMEN!");
		this.active = true;
		currentDance = 0;
		anim.SetInteger ("State", currentDance);
	}

}
