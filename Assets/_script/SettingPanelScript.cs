using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ludo
{
    public class SettingPanelScript : MonoBehaviour
    {

        public GameObject musicBtn;
        public GameObject soundBtn;
        public GameObject notificationBtn;
        public GameObject vibrationBtn;
        int musicOn;
        int soundOn;
        int notificationOn;
        int vibrationOn;
        // Use this for initialization
        void Start()
        {


        }

        public void ShowPopup()
        {
            musicOn = PlayerPrefs.GetInt(GameTags.MUSIC_ON);
            soundOn = PlayerPrefs.GetInt(GameTags.SOUND_ON);
            notificationOn = PlayerPrefs.GetInt(GameTags.NOTIFICATION_ON);
            vibrationOn = PlayerPrefs.GetInt(GameTags.VIBRATION_ON);
            Debug.Log("musicOn " + musicOn + " soundOn " + soundOn + " notificationOn " + notificationOn);

            if (musicOn == 1)
            {
                musicBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/selected_box");
            }
            else
            {
                musicBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/unselected_box");
            }

            if (soundOn == 1)
            {
                soundBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/selected_box");
            }
            else
            {
                soundBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/unselected_box");
            }

            if (notificationOn == 1)
            {
                notificationBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/selected_box");
            }
            else
            {
                notificationBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/unselected_box");
            }

            if (vibrationOn == 1)
            {
                vibrationBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/selected_box");
            }
            else
            {
                vibrationBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/unselected_box");
            }

        }
        // Update is called once per frame
        void Update()
        {

        }

        public void ClosePanel()
        {
            transform.gameObject.SetActive(false);
        }

        public void MusicAction()
        {
            Debug.Log("MUSIC " + musicOn);
            if (musicOn == 1)
            {
                LudoDelegate.changeSoundSetting(false);
                musicBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/unselected_box");
                PlayerPrefs.SetInt(GameTags.MUSIC_ON, 0);
                musicOn = 0;
            }
            else
            {
                LudoDelegate.changeSoundSetting(true);
                musicBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/selected_box");
                PlayerPrefs.SetInt(GameTags.MUSIC_ON, 1);
                musicOn = 1;
            }

        }

        public void SoundAction()
        {
            Debug.Log("SOUND " + soundOn);
            if (soundOn == 1)
            {
                soundBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/unselected_box");
                PlayerPrefs.SetInt(GameTags.SOUND_ON, 0);
                soundOn = 0;
            }
            else
            {
                soundBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/selected_box");
                PlayerPrefs.SetInt(GameTags.SOUND_ON, 1);
                soundOn = 1;
            }


        }

        public void VibrationAction()
        {
            if (vibrationOn == 1)
            {
                vibrationBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/unselected_box");
                PlayerPrefs.SetInt(GameTags.VIBRATION_ON, 0);
                vibrationOn = 0;
            }
            else
            {
                vibrationBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/selected_box");
                PlayerPrefs.SetInt(GameTags.VIBRATION_ON, 1);
                vibrationOn = 1;
            }
        }


        public void NotificationAction()
        {
            if (notificationOn == 1)
            {
                notificationBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/unselected_box");
                PlayerPrefs.SetInt(GameTags.NOTIFICATION_ON, 0);
                notificationOn = 0;
            }
            else
            {
                notificationBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/new/selected_box");
                PlayerPrefs.SetInt(GameTags.NOTIFICATION_ON, 1);
                notificationOn = 1;
            }
        }
    }
}