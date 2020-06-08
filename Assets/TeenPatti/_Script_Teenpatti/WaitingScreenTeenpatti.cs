using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaitingScreenTeenpatti : MonoBehaviour
{

	public GameObject loading;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void FixedUpdate ()
	{
		if (loading.activeSelf) {
			loading.transform.Rotate (Vector3.back, 3, Space.World);
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadSceneAsync ("FirstSceneTeenpatti");
		}
	}


}
