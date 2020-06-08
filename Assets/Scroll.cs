using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour {
    GameObject rotater;


    // Use this for initialization
    void Start () {
        rotater = GameObject.Find("Image");
    }
    public void btnChangeHeight()
    {
        rotater.gameObject.transform.localScale += new Vector3(0, 500, 0);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
