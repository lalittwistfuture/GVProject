using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TeenPattiPrivate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void backAction(){
        SceneManager.LoadSceneAsync("MainLobby");   
    }



    public void createTableAction(){
        GameControllerTeenPatti.PrivateGameType = TagsTeenpatti.CREATE_PRIVATE_TABLE;
        SceneManager.LoadSceneAsync("AmountSelectionTeenpatti"); 
    }

    public void joinTableAction() { 
        GameControllerTeenPatti.PrivateGameType = TagsTeenpatti.JOIN_PRIVATE_TABLE;
        PlayerPrefs.SetString("InHand", "");
        SceneManager.LoadSceneAsync("GameScene_Teenpatti"); 
    }

}
