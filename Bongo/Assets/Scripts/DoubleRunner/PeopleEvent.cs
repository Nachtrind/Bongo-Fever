using UnityEngine;
using System.Collections;

public class PeopleEvent : MonoBehaviour
{

	public WomanScript[] peopleToActivate;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter (Collider other)
	{
		foreach (WomanScript woman in peopleToActivate) {
			woman.Activate ();
		}
	}

}
