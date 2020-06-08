using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
namespace Lotto
{
    public class Player : MonoBehaviour
    {
        public GameObject backButton;
        public GameObject SoundObject;
        public GameObject Menupanel;
        public GameObject SettingPanel;
        public GameObject Setting;
        public GameObject CloseMenu;
        public GameObject CloseSetting;
        public GameObject NameText;
        public GameObject Score;
        public GameObject Cards;


        public GameObject Menu;


        // Use this for initialization
        void Start()
        {
            NameText.GetComponent<Text>().text = PlayerPrefs.GetString(PlayerDetails.Name);
            Score.GetComponent<Text>().text = PlayerPrefs.GetString(PlayerDetails.Coin);
            Menupanel.SetActive(false);
            CloseMenu.SetActive(false);
            SettingPanel.SetActive(false);



        }
        public void Back()
        {
            SceneManager.LoadScene(SceneClass.MAIN_LOBBY);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }

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
            updateSoundIcon();



        }

        void updateSoundIcon()
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {
                SoundObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/sound_on_button");
            }
            else
            {
                SoundObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/music_off_button");
            }
        }
        public void MenuBar()
        {
            Menupanel.SetActive(true);
            CloseMenu.SetActive(true);
        }
        public void SettingBar()
        {
            SettingPanel.SetActive(true);


        }
        public void CloseSettin()
        {
            SettingPanel.SetActive(false);
        }

    }
}