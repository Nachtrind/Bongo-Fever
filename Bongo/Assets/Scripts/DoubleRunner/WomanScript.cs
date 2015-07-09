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
	float hitDistance;
	public bool women;

	// Use this for initialization
	void Start ()
	{
		hitDistance = 1.00f;
		currentDance = 1;
		anim = this.GetComponentInChildren<Animator> ();
		anim.SetInteger ("State", currentDance);
		if (this.GetComponentInChildren<Renderer> () != null) {
			if (women) {
				this.GetComponentInChildren<Renderer> ().material = dresses [Random.Range (0, dresses.Length - 1)];
			}
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
			if (women) {
				this.currentDance = Random.Range (1, 2);
			} else {
				this.currentDance = 1;
			}
			anim.SetInteger ("State", currentDance);
		}

	}

	void FixedUpdate ()
	{
		if (active && !reachedTarget) {
			bool hitSomething = false;
			Vector3 dir = targetPoint.position - this.transform.position + new Vector3 (0.0f, 1.0f, 0.0f);
			Ray ray = new Ray (this.transform.position + new Vector3 (0.0f, 1.0f, 0.0f), dir.normalized);
			RaycastHit[] hits = Physics.RaycastAll (ray, 2.0f);
			Debug.DrawRay (this.transform.position + new Vector3 (0.0f, 1.0f, 0.0f), dir.normalized * 2.0f, Color.blue, 4.0f);

			if (hits.Length > 0) {
				foreach (RaycastHit hit in hits) {
					if (hit.collider != this.GetComponent<Collider> () && hit.distance < hitDistance) {
						Debug.Log ("Something is blocking my way, I stop and dance now");
						Debug.Log ("I, " + this.name + ", hit: " + hit.collider.gameObject.name);
						if (women) {
							this.currentDance = Random.Range (1, 2);
						} else {
							this.currentDance = 1;
						}
						anim.SetInteger ("State", currentDance);
						hitSomething = true;
						reachedTarget = true;
						break;
					}
				}
			}

			if (!hitSomething) {
				this.transform.position = new Vector3 (this.transform.position.x + dir.normalized.x * speed * Time.deltaTime, 
				                                       this.transform.position.y,
				                                       this.transform.position.z + dir.normalized.z * speed * Time.deltaTime);
			}

		}
	}

	public void Activate ()
	{
		this.active = true;
		currentDance = 0;
		anim.SetInteger ("State", currentDance);
	}

}
