using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class New : MonoBehaviour {
    public GameObject screen;
    public GameObject back;
   
 
    public GameObject table;

    // Use this for initialization
    void Start() {
        screen.SetActive(false);

    }

    // Update is called once per frame
    void Update() {


    }

    public void SelectTable(GameObject btn)
    {
        screen.SetActive(true);
        table.GetComponent<Image>().sprite = btn.GetComponent<Image>().sprite;

        
       
    }
 






    public void Return(){
        screen.SetActive(false);
        }
}

