using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NotificationPopUp : MonoBehaviour {

    public Text title;
    public Text message;


	// Use this for initialization
	void Start () {
		
	}
	
    public void showPopUp(string title,string message){
        this.title.text = title;
        this.message.text = message;
    }


    public void closePanel(){
        transform.gameObject.SetActive(false);
    }


	// Update is called once per frame
	void Update () {
		
	}
}
