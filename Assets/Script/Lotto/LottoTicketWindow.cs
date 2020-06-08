using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;
namespace Lotto
{
    public class LottoTicketWindow : MonoBehaviour
    {

        [SerializeField]
        GameObject container;
        [SerializeField]
        GameObject cellSample;
        [SerializeField]
        Text ticketCounterText;
        public Text resultTime;
        public Text systemTime;
        public GameObject confirm;
        public GameObject checkAmount;
        public Text betTime;
        public GameObject resultTimePanel;
        DateTime endBetTime;
        DateTime ticketAnnounceText;
        bool BettingTime = false;
        bool Result = false;
        bool Time = false;

        int counter;

        string result;
        // Use this for initialization
        void Start()
        {



            Result = true;
            Time = true;
            confirm.SetActive(false);
            checkAmount.SetActive(false);



        }

        public void assignTickets(JSONNode node)
        {
            //Debug.Log(node);
            foreach (Transform t in container.transform)
            {
                Destroy(t.gameObject);
            }


            for (int i = 0; i < node.Count; i++)
            {
                JSONNode data = node[i];

                GameObject obj = Instantiate(cellSample);
                obj.GetComponent<LottoTicket>().addTicket(int.Parse(data["id"]), data["ticketno"]);
                obj.transform.localScale = new Vector3(1f, 1f, 1f);
                obj.transform.SetParent(container.transform);
                obj.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        void onTicketClick()
        {
            counter = 0;
            foreach (Transform t in container.transform)
            {
                if (t.GetComponent<LottoTicket>().isSelected)
                {
                    counter++;
                }
            }


            ticketCounterText.text = "" + counter + " = Rs. " + counter * 100;
            SoundManager.buttonClick();

        }

        void onRefreshWallet(JSONNode node)
        {
            for (int i = 0; i < node.Count; i++)
            {

                JSONNode data = node[i];
                string ticket = data["ticket"];
                Debug.Log("Working " + ticket);
                foreach (Transform t in container.transform)
                {
                    Debug.Log("Ticket found " + t.GetComponent<LottoTicket>().ticketNumberValue);

                    if (ticket.Equals(t.GetComponent<LottoTicket>().ticketNumberValue))
                    {
                        Debug.Log("Ticket found");
                        t.GetComponent<LottoTicket>().isOpen = false;
                        t.GetComponent<LottoTicket>().isSelected = false;
                        t.GetComponent<Image>().color = Color.red;
                    }
                }
            }
        }


        private void OnEnable()
        {
            resultTimePanel.SetActive(false);

            DateTime.TryParse(PlayerPrefs.GetString(LOTTOGame.TICKET_ANNOUNCE_TIME), out this.ticketAnnounceText);
            ticketCounterText.text = "";
            this.BettingTime = true;
            DateTime.TryParse(PlayerPrefs.GetString(LOTTOGame.BETTING_STOP), out this.endBetTime);
            LottoGameController.onTicketClick += onTicketClick;
            LottoGameController.onRefreshWallet += onRefreshWallet;
        }

        private void OnDisable()
        {

            LottoGameController.onTicketClick -= onTicketClick;
            LottoGameController.onRefreshWallet -= onRefreshWallet;
        }
        // Update is called once per frame
        void Update()
        {
            if (this.BettingTime)
            {
                //Timer.text = "" + (this.nextSessionTime - DateTime.Now).Minutes + ":" + (this.nextSessionTime - DateTime.Now).Seconds;

                if (DateTime.Now >= this.endBetTime)
                {
                    this.BettingTime = false;
                    Debug.Log("StopBetting");
                    LottoGameController.stopBetting();
                }
            }

            if (this.endBetTime == null)
            {

                if (this.Result)
                {
                    resultTimePanel.SetActive(true);

                    resultTime.text = "0" + (this.ticketAnnounceText - DateTime.Now).Minutes + ":" + (this.ticketAnnounceText - DateTime.Now).Seconds;
                    if ((this.ticketAnnounceText - DateTime.Now).Seconds < 10)
                    {
                        resultTime.text = "0" + (this.ticketAnnounceText - DateTime.Now).Minutes + ":" + "0" + (this.ticketAnnounceText - DateTime.Now).Seconds;
                    }

                }
            }

            if (this.Time)
            {
                systemTime.text = "" + (DateTime.Now).Hour + ":" + (DateTime.Now).Minute + ":" + (DateTime.Now).Second;

                if ((DateTime.Now).Hour < 10)
                {
                    systemTime.text = "0" + (DateTime.Now).Hour + ":" + (DateTime.Now).Minute + ":" + (DateTime.Now).Second;
                }
                if ((DateTime.Now).Minute < 10)
                {
                    systemTime.text = "" + (DateTime.Now).Hour + ":" + "0" + (DateTime.Now).Minute + ":" + (DateTime.Now).Second;

                }
                if ((DateTime.Now).Second < 10)
                {
                    systemTime.text = "" + (DateTime.Now).Hour + ":" + (DateTime.Now).Minute + ":" + "0" + (DateTime.Now).Second;
                }





            }
            if (DateTime.Now < this.endBetTime)
            {



                betTime.text = "Time Left" + " " + "0" + (this.endBetTime - DateTime.Now).Minutes + ":" + (this.endBetTime - DateTime.Now).Seconds;
                if ((this.endBetTime - DateTime.Now).Seconds < 10)
                {
                    betTime.text = "Time Left" + " " + "0" + (this.endBetTime - DateTime.Now).Minutes + ":" + "0" + (this.endBetTime - DateTime.Now).Seconds;
                }


            }
        }



        public void buyTickets()
        {
            /* if ((counter * 50) < int.Parse(PlayerPrefs.GetString(PlayerDetails.Coin)))*/

            try
            {
                confirm.SetActive(true);
                confirm.GetComponent<ConfirmBet>().msg.text = "Buy" + " " + counter + " " + "Tickets" + " " + "for" + " " + counter * 100 + " " + "?";
            }catch(System.Exception ex){
                Debug.Log(ex.Message);
            }


            //  checkAmount.SetActive(true);

        }

        public void Tickets()
        {


            JSONArray array = new JSONArray();
            int counter = 0;
            foreach (Transform t in container.transform)
            {
                if (t.GetComponent<LottoTicket>().isSelected)
                {
                    counter++;
                    array.Add(t.GetComponent<LottoTicket>().ticketNumberValue);
                }
            }

            StartCoroutine(sendTicketsData(array, counter * 100));

            Debug.Log("Selected Ticket " + array.ToString());
            confirm.SetActive(false);
            // = "" + result;*/ 

        }
        private IEnumerator sendTicketsData(JSONArray array, int price)
        {
            WWWForm form = new WWWForm();
            form.AddField("TAG", "PLACE_BET_LOTTO");
            form.AddField("session", PlayerPrefs.GetString(LOTTOGame.SESSION));
            form.AddField("user_id", PlayerPrefs.GetString(PlayerDetails.ConnectionId));
            form.AddField("bet", price);
            form.AddField("ticket", array.ToString());
            WWW www = new WWW(Tags.URL, form);
            yield return www;
            if (www.error == null)
            {
                string response = www.text;
                Debug.Log(PlayerPrefs.GetString(LOTTOGame.SESSION) + " Bet Result" + response);
                try{
                JSONNode node = JSON.Parse(response);

                if (node != null)
                {
                    try
                    {
                        string result = node["status"];
                        if (result.Equals("OK"))
                        {
                            try
                            {
                                LottoGameController.updateWallet();
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
                    catch (System.Exception ex)
                    {
                        Debug.Log(ex.Message);
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
    }
    public class LOTTOGame
    {

        public const string SESSION = "SESSION";
        public const string TICKET_ANNOUNCE_TIME = "CARD_ANNOUNCE_TIME";
        public const string BETTING_STOP = "BETTING_STOP";
        public const string NEXT_SESSION = "NEXT_SESSION";
        public const string LastTicket = "LastCard";

    }
    public class Tags
    {

        public const string TAG = "TAG";
        public const string Sound = "Sound";
        public const string URL = "https://casinobluemoon.com/app/lotto/index.php";//"http://deathwish.in/dreamscard/app/index.php";
    }

}