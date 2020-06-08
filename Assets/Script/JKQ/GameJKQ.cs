using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
namespace jkq
{

    public class GameJKQ : MonoBehaviour
    {
        public GameObject notificationPopUp;
        public Text coin;
        public GameObject[] animationArray;
        int counterIndex = 0;
        [SerializeField]
        GameObject Loading;
        [SerializeField]
        GameObject newSessionPanel;
        public Text currentTime;
        public Text betAmount;
        public GameObject logOutPanel;
        bool Time = false;

        float animationTime = 10.0f;
        // Use this for initialization
        void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Screen.orientation = ScreenOrientation.Landscape;
            Loading.SetActive(true);
            notificationPopUp.SetActive(false);
            logOutPanel.SetActive(false);
            Time = true;




        }

        void stopAnimation()
        {
            foreach (GameObject obj in animationArray)
            {
                obj.SetActive(false);
            }
        }

        void onStartAnimation()
        {
            try
            {
                if (!newSessionPanel.activeSelf)
                {
                    stopAnimation();
                    animationArray[counterIndex++].SetActive(true);

                    if (counterIndex == animationArray.Length)
                    {
                        counterIndex = 0;
                    }
                }
                Invoke("StopAnimationNotify", this.animationTime);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
                GameController.stopAnimation();
            }
        }

        void StopAnimationNotify()
        {
            stopAnimation();
            GameController.stopAnimation();
        }


        public void Back()
        {
            SoundManager.buttonClick();
            logOutPanel.SetActive(true);
        }


        private void OnEnable()
        {
            betAmount.text = "";

            // Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
            GameController.onStartAnimation += onStartAnimation;




        }

        private void OnDisable()
        {

            // Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
            GameController.onStartAnimation -= onStartAnimation;



        }


        // public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
        // {
        //     UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
        //     IDictionary<string, string> dict = e.Message.Data;

        //     string ss1 = e.Message.Notification.Title;
        //     string ss = "";
        //     foreach (var item in dict)
        //     {

        //         ss += item.Key + ":" + item.Value + "----";

        //         if (item.Key.Equals("game_data"))
        //         {
        //             SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse(item.Value);
        //             ss += node["TAG"];
        //             GameController.chatRecived("0000", item.Value);


        //         }
        //     }
        //     notificationPopUp.SetActive(true);
        //     notificationPopUp.GetComponent<NotificationPopUp>().showPopUp(ss1, ss);

        // }



        // Update is called once per frame
        void Update()

        {


            int abc = int.Parse(PlayerPrefs.GetString(PlayerDetails.Coin));


            coin.text = String.Format("{0:#,##0,##}", abc);


            betAmount.text = PlayerPrefs.GetString(JKQGame.Bet);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }







            if (this.Time)
            {
                currentTime.text = "" + (DateTime.Now).Hour + ":" + (DateTime.Now).Minute + ":" + (DateTime.Now).Second;
                if ((DateTime.Now).Hour < 10)
                {
                    currentTime.text = "0" + (DateTime.Now).Hour + ":" + (DateTime.Now).Minute + ":" + (DateTime.Now).Second;
                }
                if ((DateTime.Now).Minute < 10)
                {
                    currentTime.text = "" + (DateTime.Now).Hour + ":" + "0" + (DateTime.Now).Minute + ":" + (DateTime.Now).Second;

                }
                if ((DateTime.Now).Second < 10)
                {
                    currentTime.text = "" + (DateTime.Now).Hour + ":" + (DateTime.Now).Minute + ":" + "0" + (DateTime.Now).Second;
                }


            }


        }

        void StartLoading()
        {

        }




    }
}