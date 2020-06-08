using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using SimpleJSON;
namespace Lotto
{
    public class LottoGame : MonoBehaviour
    {

        public GameObject ticketWindow;
        public GameObject luckyNumberWindow;
        public GameObject myWallet;
        public GameObject loadingScreen;
        public GameObject newSessionScreen;
        public Text walletCounterText;
        public Text coin;
        public GameObject logOut;
        DateTime endBetTime;
        DateTime ticketAnnounceTime;
        int walletCounter = 0;

        bool BettingTime = false;
        bool callNextSession = false;
        // Use this for initialization
        void Start()
        {
            logOut.SetActive(false);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            endBetTime = DateTime.Now;
            openScreen(loadingScreen);

        }

        void openScreen(GameObject screen)
        {
            ticketWindow.SetActive(false);
            luckyNumberWindow.SetActive(false);
            myWallet.SetActive(false);
            newSessionScreen.SetActive(false);
            loadingScreen.SetActive(false);
            screen.SetActive(true);
        }

        public void backToLobby()
        {
            logOut.SetActive(true);
        }


        private IEnumerator ServerRequest()
        {
            // yield return new WaitForSeconds(3.0f);
            WWWForm form = new WWWForm();
            form.AddField("TAG", "LOTTO_GAME");
            WWW www = new WWW(Tags.URL, form);
            yield return www;
            if (www.error == null)
            {
                string response = www.text;
                Debug.Log("response" + response);
                try{
                JSONNode node = JSON.Parse(response);

                if (node != null)
                {
                    string result = node["status"];
                    if (result.Equals("OK"))
                    {
                        try
                        {
                            JSONNode data1 = node["data"];
                            JSONNode data = data1[0];
                            string nextsessionTime = data["next_session_time"];
                            PlayerPrefs.SetString(LOTTOGame.NEXT_SESSION, nextsessionTime);
                            this.callNextSession = true;
                            walletCounter = 0;
                            if (int.Parse(data["winning_ticket"]) == -1)
                            {
                                string session_id = data["session_id"];
                                string ticket_announce_time = data["ticket_announce"];
                                string stopBettingTime = data["end_time"];
                                PlayerPrefs.SetString(LOTTOGame.SESSION, session_id);
                                PlayerPrefs.SetString(LOTTOGame.TICKET_ANNOUNCE_TIME, ticket_announce_time);
                                PlayerPrefs.SetString(LOTTOGame.BETTING_STOP, stopBettingTime);
                                DateTime.TryParse(stopBettingTime, out this.endBetTime);
                                DateTime.TryParse(ticket_announce_time, out this.ticketAnnounceTime);
                                this.BettingTime = true;
                                Debug.Log("Current Session");
                                StartCoroutine(getTicketsData());

                            }
                            else
                            {
                                Debug.Log("Session closed");
                                openScreen(newSessionScreen);
                            }
                            if (data1.Count > 1)
                            {
                                JSONNode dat = data1[1];
                                string lastTicket = dat["winning_ticket"];
                                GameController.sendLastTicket(lastTicket);
                                PlayerPrefs.SetString(LOTTOGame.LastTicket, lastTicket);

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
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            else
            {

            }


        }


        void onRefreshWallet(JSONNode node)
        {
            walletCounter = 0;
            for (int i = 0; i < node.Count; i++)
            {
                walletCounter++;
            }
            walletCounterText.text = "" + walletCounter;

        }

        private IEnumerator refreshWallet()
        {
            WWWForm form = new WWWForm();
            form.AddField("TAG", "LOTTO_BET_DATA");
            form.AddField("session", PlayerPrefs.GetString(LOTTOGame.SESSION));
            form.AddField("user_id", PlayerPrefs.GetString(PlayerDetails.ConnectionId));
            WWW www = new WWW(Tags.URL, form);
            yield return www;
            if (www.error == null)
            {
                string response = www.text;
                Debug.Log(PlayerPrefs.GetString(LOTTOGame.SESSION) + " response" + response);
                try{
                JSONNode node = JSON.Parse(response);

                if (node != null)
                {
                    string result = node["status"];
                    if (result.Equals("OK"))
                    {
                        try
                        {
                            LottoGameController.refreshWallet(node["data"]);
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
                GameController.startGame();
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



        private IEnumerator getTicketsData()
        {
            WWWForm form = new WWWForm();
            form.AddField("TAG", "LOTTO_TICKETS");
            form.AddField("session", PlayerPrefs.GetString(LOTTOGame.SESSION));
            WWW www = new WWW(Tags.URL, form);
            yield return www;
            if (www.error == null)
            {
                string response = www.text;
                Debug.Log(PlayerPrefs.GetString(LOTTOGame.SESSION) + " response" + response);
                try{
                JSONNode node = JSON.Parse(response);

                if (node != null)
                {
                    string result = node["status"];
                    if (result.Equals("OK"))
                    {
                        try
                        {
                            StartCoroutine(refreshWallet());
                            openScreen(ticketWindow);
                            ticketWindow.GetComponent<LottoTicketWindow>().assignTickets(node["data"]);
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
                    GameController.startGame();
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

        void onUpdateWallet()
        {
            StartCoroutine(refreshWallet());
            walletCounterText.text = "";
        }

        private void OnEnable()
        {

            walletCounterText.text = "";
            StartCoroutine(ServerRequest());
            LottoGameController.onStartNewSession += onStartNewSession;
            LottoGameController.onSessionClose += onSessionClose;
            LottoGameController.onBettingStop += onBettingStop;
            LottoGameController.onRefreshWallet += onRefreshWallet;
            LottoGameController.onUpdateWallet += onUpdateWallet;
        }

        private void OnDisable()
        {
            LottoGameController.onStartNewSession -= onStartNewSession;
            LottoGameController.onSessionClose -= onSessionClose;
            LottoGameController.onBettingStop -= onBettingStop;
            LottoGameController.onRefreshWallet -= onRefreshWallet;
            LottoGameController.onUpdateWallet -= onUpdateWallet;
        }

        void onBettingStop()
        {
            openScreen(luckyNumberWindow);
        }
        private void onSessionClose()
        {
            openScreen(newSessionScreen);
        }

        void onStartNewSession()
        {
            StartCoroutine(ServerRequest());

        }
        void Update()
        {
            int abc = int.Parse(PlayerPrefs.GetString(PlayerDetails.Coin));


            coin.text = String.Format("{0:#,#0,##,##0}", abc);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                logOut.SetActive(true);
            }



        }
    }

}	