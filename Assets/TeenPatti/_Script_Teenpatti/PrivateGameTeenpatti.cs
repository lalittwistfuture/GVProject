using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrivateGameTeenpatti : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadSceneAsync ("MainLobby");
		}
	}

	public void CreateTable ()
	{
		GameControllerTeenPatti.PrivateGameType = TagsTeenpatti.CREATE_PRIVATE_TABLE;
		SceneManager.LoadSceneAsync ("AmountSelectionTeenpatti");
	}

	public void JoinTable ()
	{
		//PlayerPrefs.SetString (PrefebTagsTeenpatti.PRIVATE_TYPE,PrefebTagsTeenpatti.JOIN);
		GameControllerTeenPatti.PrivateGameType = TagsTeenpatti.JOIN_PRIVATE_TABLE;
		SceneManager.LoadSceneAsync ("GameScene_Teenpatti");
	}

}
