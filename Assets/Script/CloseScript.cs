using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseScript : MonoBehaviour {
    public GameObject Msg;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Close()
    {
        transform.gameObject.SetActive(false);
    }
}
