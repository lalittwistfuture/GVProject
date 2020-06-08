using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Lotto
{

    public class GamePlay : MonoBehaviour
    {
        public GameObject BackButton;
        public GameObject nameText;
        public GameObject score;


        // Use this for initialization
        void Start()
        {

            nameText.GetComponent<Text>().text = PlayerPrefs.GetString(PlayerDetails.Name);
        }

        public void Back()
        {
            //  SceneManager.LoadScene("FinalLobby");
            SoundManager.buttonClick();
            SceneManager.LoadScene(SceneClass.MAIN_LOBBY);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(SceneClass.MAIN_LOBBY);
            }
            score.GetComponent<Text>().text = PlayerPrefs.GetString(PlayerDetails.Coin);


        }


        public void SoundAction()
        {
            Debug.Log("SoundAction working " + PlayerPrefs.GetInt("Sound"));

            if (PlayerPrefs.GetInt("Sound") == 1)
            {
                PlayerPrefs.SetInt("Sound", 0);
                Debug.Log("off" + PlayerPrefs.GetInt("Sound"));
            }
            else
            {
                PlayerPrefs.SetInt("Sound", 1);
                Debug.Log("on >>> " + PlayerPrefs.GetInt("Sound"));
            }
            Debug.Log("SoundAction working " + PlayerPrefs.GetInt("Sound"));



        }



    }

}