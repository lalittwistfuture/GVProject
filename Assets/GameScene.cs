using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    public GameObject ipanel;
    public GameObject leftpanel;
    // Start is called before the first frame update
    void Start()
    {
        
       // ipanel.SetActive(false);
       
       // leftpanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Ipanel()
    {
       
            ipanel.SetActive(true);
        


    }
    public void LeftPanel()
    {
        leftpanel.SetActive(true);
    }
}
