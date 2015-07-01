using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stage : MonoBehaviour
{

	//Stage Space
	float xExtends;
	float zExtends;
	Vector3 stageCenter;
	public Renderer stageRender;

	// Use this for initialization
	void Start ()
	{
		xExtends = stageRender.bounds.extents.x * 0.9f;
		zExtends = stageRender.bounds.extents.z * 0.5f;

		stageCenter = stageRender.bounds.center;
	}
	
	public void SetStagePos (List<Runner> _lefties, List<Runner> _righties)
	{
		for (int i = 0; i < _lefties.Count; i++) {
			float x = stageCenter.x - (xExtends / _lefties.Count * i) - 0.5f - Random.Range (-0.3f, 0.3f);
			float z = stageCenter.z + (Random.Range (-1.0f, 1.0f) * zExtends);
			_lefties [i].SetStagePos (new Vector3 (x, 0.0f, z));
		}

		for (int i = 0; i < _righties.Count; i++) {
			float x = stageCenter.x + (xExtends / _righties.Count * i) + 0.5f + Random.Range (-0.3f, 0.3f);
			float z = stageCenter.z + (Random.Range (-1.0f, 1.0f) * zExtends);
			_righties [i].SetStagePos (new Vector3 (x, 0.0f, z));
		}
	}




}
