using UnityEngine;
using System.Collections;

public class BigHouse : MonoBehaviour
{

	public Material[] mats;
	
	// Use this for initialization
	void Start ()
	{
		int ran = Random.Range (0, mats.Length - 1);
		this.GetComponent<Renderer> ().material = mats [ran];
		Renderer[] rens = GetComponentsInChildren<Renderer> ();

		foreach (Renderer ren in rens) {
			ren.material = mats [ran];
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
