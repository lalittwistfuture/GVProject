using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VersionPopUp : MonoBehaviour {


    string url;
	// Use this for initialization
	void Start () {
		
	}

    public void setMessage(string appURL){
        this.url = appURL; 
    }

    public void openUrl(){
        Application.OpenURL(this.url);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
