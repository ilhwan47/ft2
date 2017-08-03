using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_Dev : MonoBehaviour {

	public void SceneChange(string IslandName){
		SceneManager.LoadScene (IslandName);
	}

}
