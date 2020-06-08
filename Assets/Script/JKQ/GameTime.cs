using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;
namespace jkq
{
    public class GameTime : MonoBehaviour
    {
        public Text timerText;
        public GameObject PopUpContainer;
        public GameObject timePanel;
        string abc = "00";

        // public Text loadingTime;
        DateTime endBetTime;
        DateTime cardAnnounceTime;
        DateTime nextSessionTime;

        bool BettingTime = false;
        bool callNextSession = false;
        // Use this for initialization
        void Start()
        {

            timePanel.SetActive(false);
            PopUpContainer = Instantiate((GameObject)Resources.Load("_prefeb/PopUp 2"));
            PopUpContainer.transform.SetParent(transform);
            PopUpContainer.transform.localPosition = new Vector3(01.0f, 01.0f, 01.0f);
            PopUpContainer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            PopUpContainer.GetComponent<CloseScript>().Msg.GetComponent<Text>().text = "Welcome";
            PopUpContainer.SetActive(false);
            GameController.bettingTime = false;
            endBetTime = DateTime.Now;
        }


        private IEnumerator ServerRequest()
        {
            // yield return new WaitForSeconds(5.0f);
            WWWForm form = new WWWForm();
            form.AddField("TAG", "JKQ_GAME");

            WWW www = new WWW(Tags.URL, form);
            yield return www;
            if (www.error == null)
            {
                string response = www.text;
                Debug.Log("Session Response" + response);
                JSONNode node = JSON.Parse(response);

                if (node != null)
                {
                    string result = node["status"];
                    if (result.Equals("OK"))
                    {
                        try
                        {
                            GameController.changeTable();
                            JSONNode data1 = node["data"];
                            JSONNode data = data1[0];
                            string nextsessionTime = data["next_session_time"];
                            PlayerPrefs.SetString(JKQGame.NEXT_SESSION, nextsessionTime);
                            DateTime.TryParse(nextsessionTime, out this.nextSessionTime);
                            this.callNextSession = true;
                            Debug.Log("New Session start " + nextsessionTime);

                            if (int.Parse(data["winning_card"]) == -1)
                            {
                                string session_id = data["session_id"];
                                string card_announce_time = data["card_announce"];
                                string stopBettingTime = data["end_time"];
                                PlayerPrefs.SetString(JKQGame.SESSION, session_id);
                                PlayerPrefs.SetString(JKQGame.CARD_ANNOUNCE_TIME, card_announce_time);
                                DateTime.TryParse(stopBettingTime, out this.endBetTime);
                                DateTime.TryParse(card_announce_time, out this.cardAnnounceTime);
                                Debug.Log(cardAnnounceTime.ToString());
                                Debug.Log(endBetTime.ToString());
                                this.BettingTime = true;
                                timePanel.SetActive(false);
                                StartCoroutine(getCardsData());
                                PlayerPrefs.SetInt(JKQGame.State, JKQGame.BettingTime);


                            }
                            else
                            {
                                timePanel.SetActive(true);
                                PlayerPrefs.SetInt(JKQGame.State, JKQGame.WaitingForSession);
                                GameController.startGame();
                            }
                            if (data1.Count > 1)
                            {
                                JSONNode dat = data1[1];
                                string lastCard = dat["winning_card"];
                                GameController.sendLastCard(lastCard);
                                PlayerPrefs.SetString(JKQGame.LastCard, lastCard);

                            }
                        }
                        catch (System.Exception ex)
                        {
                            Debug.Log(ex.Message);
                        }
                    }
                    else
                    {


                    }
                }
                else
                {

                }
            }


        }


        private IEnumerator getCardsData()
        {
            WWWForm form = new WWWForm();
            form.AddField("TAG", "JKQ_BET_DATA");
            form.AddField("user_id", PlayerPrefs.GetString(PlayerDetails.ConnectionId));
            form.AddField("session", PlayerPrefs.GetString(JKQGame.SESSION));
            WWW www = new WWW(Tags.URL, form);
            yield return www;
            if (www.error == null)
            {
                string response = www.text;
                Debug.Log("My Data response" + response);



                JSONNode node = JSON.Parse(response);

                if (node != null)


                {
                    string result = node["status"];
                    if (result.Equals("OK"))
                    {
                        try
                        {
                            JSONNode data1 = node["data"];
                            for (int i = 0; i < data1.Count; i++)
                            {
                                JSONNode data = data1[i];
                                string card = data["card"];
                                string price = data["price"];
                                GameController.selectCard(card, price);


                            }

                        }
                        catch (System.Exception ex)
                        {
                            Debug.Log(ex.Message);
                        }
                    }
                    else
                    {

                    }
                }
                timePanel.SetActive(false);
                GameController.startGame();

            }
            else
            {

            }


        }


        private void OnEnable()
        {
            StartCoroutine(ServerRequest());

            GameController.onRecievedMessage += onRecievedMessage;
            GameController.onWinningCard += onWinningCard;
            GameController.onStopAnimation += onStopAnimation;
        }

        private void OnDisable()
        {
            GameController.onRecievedMessage -= onRecievedMessage;
            GameController.onWinningCard -= onWinningCard;
            GameController.onStopAnimation -= onStopAnimation;
        }

        void onStopAnimation()
        {
            timePanel.SetActive(true);
        }
        void onWinningCard(string card)
        {

        }

        void onRecievedMessage(string sender, string message)
        {
            JSONNode node = JSON.Parse(message);
            try
            {
                switch (node["TAG"])
                {
                    case "BETTING_START":
                        {

                            GameController.bettingTime = true;

                            StartCoroutine(ServerRequest());


                        }
                        break;
                    case "BETTING_STOP":
                        {
                            GameController.bettingTime = false;
                            PlayerPrefs.SetString(JKQGame.Bet, abc);

                        }
                        break;


                    default:

                        break;
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        // Update is called once per frame
        void Update()
        {


            if (DateTime.Now < this.endBetTime)
            {

                GameController.bettingTime = true;

                timerText.text = "Time Left" + " " + "0" + (this.endBetTime - DateTime.Now).Minutes + ":" + (this.endBetTime - DateTime.Now).Seconds;
                if ((this.endBetTime - DateTime.Now).Seconds < 10)
                {
                    timerText.text = "Time Left" + " " + "0" + (this.endBetTime - DateTime.Now).Minutes + ":" + "0" + (this.endBetTime - DateTime.Now).Seconds;
                }


            }
            else
            {
                timerText.text = "Betting Stop";
            }
            if (DateTime.Now >= this.endBetTime && this.BettingTime)
            {
                this.BettingTime = false;
                GameController.stopBetting();
                GameController.bettingTime = false;
                PlayerPrefs.SetInt(JKQGame.State, JKQGame.WaitingForCard);
                Debug.Log("Betting is Stop ");
            }

            if (DateTime.Now >= this.nextSessionTime && this.callNextSession)
            {
                this.callNextSession = false;
                if (PlayerPrefs.GetInt(JKQGame.State) == JKQGame.WaitingForSession)
                {
                    StartCoroutine(ServerRequest());
                    PlayerPrefs.SetString(JKQGame.Bet, abc);
                }
            }
            else
            {

            }
        }
        void Loading()
        {

        }


    }
}