using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGameType : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectGame(int value){
        Debug.Log("User Game " + value);
        appwrapTeenpatti.sendGameType(value);
    }



}
