using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour
{

	public Material[] carMats;

	// Use this for initialization
	void Start ()
	{
		this.GetComponent<Renderer> ().material = carMats [Random.Range (0, carMats.Length - 1)];
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
