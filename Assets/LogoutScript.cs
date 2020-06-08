using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LogoutScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	

    public void yesAction()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadSceneAsync("firstScene");
    }

    public void noAction()
    {
        gameObject.SetActive(false);

    }
}
