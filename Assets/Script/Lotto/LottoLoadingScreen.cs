using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LottoLoadingScreen : MonoBehaviour {

    [SerializeField]
    GameObject loadingIcon;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (loadingIcon.activeSelf)
        {
            loadingIcon.transform.Rotate(Vector3.back, 5f);
        }
	}
}
