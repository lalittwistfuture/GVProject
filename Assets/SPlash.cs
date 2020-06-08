using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SPlash : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
        Invoke("MyLoadingFunction", 2f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MyLoadingFunction()
    {
        if (PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID).Length != 0)
        {
            SceneManager.LoadScene("MainLobby");
        }
        else
        {
            SceneManager.LoadScene("LogIn");
        }
    }
}
