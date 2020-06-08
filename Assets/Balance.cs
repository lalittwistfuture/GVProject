using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balance : MonoBehaviour {
    public Text msg;

	// Use this for initialization
	void Start () {
        msg.text = "You Don't Have  Enough Balance for This Game.";
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Cancel()
    {
        transform.gameObject.SetActive(false);
    }
}
