using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;
namespace jkq
{
    public class ResultCard : MonoBehaviour
    {


        public Text timerText;
        public GameObject cardImage;
        public Image lastCardImage;
        public GameObject restTime;
        public GameObject table;
        public GameObject blackLayer;
        DateTime cardAnnounceTime;
        public GameObject timer;
        bool startBetting = false;
        public int a = 00;

        bool cardnotDeclared = false;
        string winningCard = "-1";
        int lastSeconds = 60;
        Vector3 lastPosition;
        // Use this for initialization
        void Start()
        {
            blackLayer.SetActive(false);
            restTime.SetActive(false);
            timer.SetActive(false);



        }


        private void OnEnable()
        {
            GameController.onRecievedMessage += onRecievedMessage;
            GameController.onLastCardInfo += onLastCardInfo;
            GameController.onStartGame += onStartGame;

        }

        private void OnDisable()
        {
            GameController.onRecievedMessage -= onRecievedMessage;
            GameController.onLastCardInfo -= onLastCardInfo;
            GameController.onStartGame -= onStartGame;
        }

        void onStartGame()
        {
            DateTime.TryParse(PlayerPrefs.GetString(JKQGame.CARD_ANNOUNCE_TIME), out this.cardAnnounceTime);

            startBetting = true;
            this.cardnotDeclared = true;
        }


        public void onLastCardInfo(string card_name)
        {
            lastCardImage.sprite = Resources.Load<Sprite>("Images/" + card_name);
            cardImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + -1);
        }

        void onRecievedMessage(string sender, string message)
        {
            JSONNode node = JSON.Parse(message);
            try
            {
                switch (node["TAG"])
                {
                    //case "CARD_TIME":
                    //{
                    //    string value = node["VALUE"];
                    //    string lastCard = node["LAST_CARD"];
                    //    string resultCard = node["WINNING_CARD"];
                    //    timerText.text = value;

                    //}
                    //break;
                    case "WINNING_CARD":
                        {
                            string value = node["VALUE"];
                            cardImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + value);

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


        void ScaleUp()
        {
            blackLayer.SetActive(true);
            iTween.ScaleTo(cardImage, iTween.Hash("scale", new Vector3(1.2f, 1.2f, 2.1f), "time", 1.0f, "oncomplete", "FlipUp", "transition", "easeInOutExpo", "oncompletetarget", this.gameObject));
            lastPosition = cardImage.transform.position;
            iTween.MoveTo(cardImage, iTween.Hash("position", table.transform.position, "easetype", iTween.EaseType.easeInOutSine, "time", 1.0f));
        }

        void FlipUp()
        {
            iTween.RotateTo(cardImage, iTween.Hash("y", 90, "time", 1, "transition", "easeInOutExpo", "oncomplete", "FlipDown", "oncompletetarget", this.gameObject));
        }

        void FlipDown()
        {
            cardImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + this.winningCard);
            iTween.RotateTo(cardImage, iTween.Hash("y", 0, "time", 1, "transition", "easeInOutExpo", "oncomplete", "ScaleDown", "oncompletetarget", this.gameObject));


        }

        void ScaleDown()
        {
            GameController.startAnimation();
            iTween.MoveTo(cardImage, iTween.Hash("position", lastPosition, "easetype", iTween.EaseType.easeInOutSine, "time", 1.0f, "oncomplete", "FinishAnimation", "oncompletetarget", this.gameObject));
            iTween.ScaleTo(cardImage, iTween.Hash("scale", new Vector3(0, 0, 0), "time", 1.0f));
        }

        void FinishAnimation()
        {
            blackLayer.SetActive(false);
            GameController.sendWinCard(this.winningCard);
        }



        private IEnumerator ServerRequest()
        {
            //  yield return new WaitForSeconds(5.0f);
            WWWForm form = new WWWForm();
            form.AddField("TAG", "JKQ_GAME_SESSION");
            form.AddField("session", PlayerPrefs.GetString(JKQGame.SESSION));
            WWW www = new WWW(Tags.URL, form);
            yield return www;
            if (www.error == null)
            {
                string response = www.text;
                Debug.Log("response" + response);



                JSONNode node = JSON.Parse(response);

                if (node != null)
                {
                    string result = node["status"];
                    if (result.Equals("OK"))

                    {

                        try
                        {
                            JSONNode data1 = node["data"];
                            JSONNode dat = data1[0];
                            this.winningCard = dat["winning_card"];
                            if (!this.winningCard.Equals("-1"))
                            {
                                PlayerPrefs.SetInt(JKQGame.State, JKQGame.WaitingForSession);
                                ScaleUp();

                                StartCoroutine(UserCoin());

                            }
                            else
                            {
                                StartCoroutine(ServerRequest());
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
            }
            else
            {

            }


        }
        IEnumerator UserCoin()
        {
            yield return new WaitForSeconds(5.0f);
            WWWForm form = new WWWForm();
            form.AddField("TAG", "USER_COIN");
            form.AddField("user_id", PlayerPrefs.GetString(PlayerDetails.ConnectionId));
            WWW www = new WWW(Tags.URL, form);
            yield return www;
            if (www.error == null)
            {
                string response = www.text;
                Debug.Log("response" + response);



                JSONNode node = JSON.Parse(response);

                if (node != null)
                {
                    string result = node["status"];
                    if (result.Equals("OK"))
                    {
                        try
                        {
                            JSONNode data1 = node["data"];
                            JSONNode dat = data1[0];
                            string coin = dat["balance"];

                            PlayerPrefs.SetString(PlayerDetails.Coin, coin);


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
            }
            else
            {

            }




        }

        // Update is called once per frame
        void Update()
        {
            int a = 00;
            if (GameController.bettingTime == false)
            {

                if (DateTime.Now < this.cardAnnounceTime)
                {
                    timer.SetActive(true);

                    timerText.text = "0" + (this.cardAnnounceTime - DateTime.Now).Minutes + ":" + (this.cardAnnounceTime - DateTime.Now).Seconds;
                    if (lastSeconds != (this.cardAnnounceTime - DateTime.Now).Seconds)
                    {
                        SoundManager.buttonClick();
                        lastSeconds = (this.cardAnnounceTime - DateTime.Now).Seconds;
                    }
                    if ((this.cardAnnounceTime - DateTime.Now).Seconds < 10)
                    {
                        timerText.text = "0" + (this.cardAnnounceTime - DateTime.Now).Minutes + ":" + "0" + (this.cardAnnounceTime - DateTime.Now).Seconds;
                    }

                }
                if (DateTime.Now >= this.cardAnnounceTime && this.cardnotDeclared)
                {
                    if (PlayerPrefs.GetInt(JKQGame.State) == JKQGame.WaitingForCard)
                    {
                        cardnotDeclared = false;
                        Debug.Log("Card is announced ");
                        StartCoroutine(ServerRequest());
                        UserCoin();
                        PlayerPrefs.SetString(JKQGame.Bet, a.ToString());
                    }
                }

            }
            else
            {

            }
        }

    }
}