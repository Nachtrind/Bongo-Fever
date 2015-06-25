using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	public void LoadLevel(string levelName)
	{
		GameManager.instance.LoadLevel(levelName);
	}

}
