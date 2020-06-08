using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class QuitGameScript : MonoBehaviour
    {

        public GameObject msgText;
        // Use this for initialization
        void Start()
        {
            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) != LudoTags.OFFLINE)
            {
                msgText.GetComponent<Text>().text = "Are you sure you want to leave the game.You will lose your money.";
            }else{
                msgText.GetComponent<Text>().text = "Are you sure you want to leave the game."; 
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void YesAction()
        {
            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) != LudoTags.OFFLINE)
            {
                if (appwarp.socketConnected)
                {
                    appwarp.leaveGame();
                }else{
                    SceneManager.LoadSceneAsync("MainLobby");  
                }
            }else{
                LudoDelegate.leaveGame();
               // SceneManager.LoadSceneAsync("LobiScene");
            }
            transform.gameObject.SetActive(false);
          
        }
        public void NoAction()
        {
            transform.gameObject.SetActive(false);
        }


    }
}
