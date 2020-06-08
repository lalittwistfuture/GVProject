using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
namespace jkq
{
    public class BetScript : MonoBehaviour
    {
        public GameObject cardimage;
        public GameObject totalcoin;
        public GameObject betamount;
        public GameObject userimage;
        private GameObject PopUpContainer;
        public GameObject loadingScreen;
        public GameObject loading;
        string newCard;
        public int betCoin = 0;


        // Use this for initialization
        void Start()
        {
            totalcoin.GetComponent<Text>().text = PlayerPrefs.GetString(PlayerDetails.Coin);



            PopUpContainer = Instantiate((GameObject)Resources.Load("_prefeb/PopUp 2"));
            PopUpContainer.transform.SetParent(transform);
            PopUpContainer.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            PopUpContainer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            PopUpContainer.GetComponent<CloseScript>().Msg.GetComponent<Text>().text = "Welcome";
            PopUpContainer.SetActive(false);
            loadingScreen.SetActive(false);
            loading.SetActive(true);

        }

        private void OnEnable()
        {
            totalcoin.GetComponent<Text>().text = PlayerPrefs.GetString(PlayerDetails.Coin);
            GameController.onBettingStop += onBettingStop;
            betamount.GetComponent<InputField>().text = "";
        }

        private void OnDisable()
        {
            GameController.onBettingStop -= onBettingStop;
        }

        void onBettingStop()
        {
            transform.gameObject.SetActive(false);
        }


        public void showImage(Sprite image, string cardName)
        {
            this.newCard = cardName;
            cardimage.GetComponent<Image>().sprite = image;
        }


        public void enterText(InputField textField)
        {
            int r = int.Parse(textField.text);
            int value = r / 10;
            value = value * 10;
            textField.text = "" + value;
            Debug.Log("Value change");
        }


        public void Clos()
        {
            transform.gameObject.SetActive(false);
        }
        public void Bet()

        {
            if ((int.Parse(betamount.GetComponent<InputField>().text)) <= (int.Parse(totalcoin.GetComponent<Text>().text)))
            {
                GameController.selectCard(this.newCard, this.betamount.GetComponent<InputField>().text);
                Debug.Log("Selected card " + this.newCard + " : " + this.betamount.GetComponent<InputField>().text);
                //appwarp.addCoin(this.newCard,this.betamount.GetComponent<InputField>().text);

                StartCoroutine(ServerRequest(betamount.GetComponent<InputField>().text, this.newCard));
                loadingScreen.SetActive(true);
                loading.SetActive(true);




            }

            else
            {
                PopUpContainer.GetComponent<CloseScript>().Msg.GetComponent<Text>().text = "You Do Not Have Sufficient Coin.";
                PopUpContainer.SetActive(true);

            }


        }


        private IEnumerator ServerRequest(string bet, string card)
        {
            Debug.Log("game");
            WWWForm form = new WWWForm();
            form.AddField("TAG", "PLACE_BET_JKQ");
            form.AddField("user_id", PlayerPrefs.GetString(PlayerDetails.ConnectionId));
            form.AddField("session", PlayerPrefs.GetString(JKQGame.SESSION));
            form.AddField("bet", bet);
            form.AddField("card", card);
            WWW www = new WWW(Tags.URL, form);
            // int sum = int.Parse(PlayerPrefs.GetString(PlayerDetails.BetCoin))+bet;
            this.betCoin = this.betCoin + int.Parse(bet);
            Debug.Log("coin update" + this.betCoin);
            PlayerPrefs.SetString(JKQGame.Bet, this.betCoin.ToString());
            Debug.Log("bet amount" + bet);
            yield return www;

            if (www.error == null)
            {
                string response = www.text;
                Debug.Log("response" + response);
                SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(response);
                if (node != null)
                {
                    try
                    {
                        string result = node["status"];
                        if (result.Equals("OK"))
                        {
                            SimpleJSON.JSONNode data1 = node["data"];
                            SimpleJSON.JSONNode data = data1[0];
                            string id = data["id"];
                            string name1 = data["name"];
                            string email1 = data["email"];
                            string password = data["password"];
                            string mobile = data["mobile"];
                            string picture = data["picture"];
                            int coin = int.Parse(data["balance"]);
                            int dealer = int.Parse(data["parent_id"]);
                            if (dealer > 0)
                            {
                                PlayerPrefs.SetInt(PlayerDetails.RealMoney, 1);
                            }
                            else
                            {
                                PlayerPrefs.SetInt(PlayerDetails.RealMoney, 0);
                            }
                            PlayerPrefs.SetString(PlayerDetails.ConnectionId, "" + id);
                            PlayerPrefs.SetString(PlayerDetails.Name, name1);
                            PlayerPrefs.SetString(PlayerDetails.Email, email1);
                            PlayerPrefs.SetString(PlayerDetails.Mobile, mobile);
                            PlayerPrefs.SetString(PlayerDetails.Coin, "" + coin);
                            PlayerPrefs.SetString(PlayerDetails.Password, password);
                            PlayerPrefs.SetString(PlayerDetails.Picture, picture);


                            loadingScreen.SetActive(false);
                            //loading.SetActive(false);
                            transform.gameObject.SetActive(false);
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
        // Update is called once per frame
        void Update()
        {

            totalcoin.GetComponent<Text>().text = PlayerPrefs.GetString(PlayerDetails.Coin);

            if (loading.activeSelf)
            {
                loading.transform.Rotate(Vector3.back, 5f);
            }
        }

    }
    public class Tags
    {

        public const string TAG = "TAG";
        public const string Sound = "Sound";
        public const string URL = "https://casinobluemoon.com/app/jkq/index.php";
    }
    public class JKQGame
    {

        public const string SESSION = "SESSION";
        public const string CARD_ANNOUNCE_TIME = "CARD_ANNOUNCE_TIME";
        public const string BETTING_STOP = "BETTING_STOP";
        public const string NEXT_SESSION = "NEXT_SESSION";
        public const string LastCard = "LastCard";
        public const string State = "State";
        public const int BettingTime = 1;
        public const int WaitingForCard = 2;
        public const int WaitingForSession = 3;
        public const string BetCoin = "100";
        public const string Bet = "";

    }
}