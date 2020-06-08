using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
namespace jkq
{
    public class CardScript : MonoBehaviour
    {
        public GameObject betStrip;
        public GameObject betamount;
        public GameObject coinAnimtion;
        public Text userCoin;
        public int bets = 000;
        private string winAmount;


        private string coin;
        // Use this for initialization
        void Start()
        {
            betStrip.SetActive(false);
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + gameObject.name);
            coinAnimtion.SetActive(false);

        }
        void onStartGame()
        {
            coinAnimtion.SetActive(false);

        }
        private void OnEnable()
        {
            GameController.onCardSelected += onCardSelected;
            GameController.onRecievedMessage += onRecievedMessage;
            GameController.onWinningCard += onWinningCard;
            GameController.onStartGame += onStartGame;


        }

        private void OnDisable()
        {
            GameController.onCardSelected -= onCardSelected;
            GameController.onRecievedMessage -= onRecievedMessage;
            GameController.onWinningCard -= onWinningCard;
            GameController.onStartGame -= onStartGame;
        }

        void onWinningCard(string card)
        {

            if (betStrip.activeSelf)
            {
                betStrip.SetActive(false);

                if (card.Equals(gameObject.name))
                {
                    WinAnimation();
                }
                else
                {
                    LossAnimation();

                }
            }
        }

        void onRecievedMessage(string sender, string message)
        {
            JSONNode node = JSON.Parse(message);
            try
            {
                switch (node["TAG"])
                {

                    case "WINNING_CARD":
                        {
                            string value = node["VALUE"];
                            if (betStrip.activeSelf)
                            {
                                betStrip.SetActive(false);

                                if (value.Equals(gameObject.name))
                                {
                                    WinAnimation();
                                }
                                else
                                {
                                    LossAnimation();

                                }
                            }
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
        public void Move()
        {


            iTween.MoveTo(coinAnimtion, iTween.Hash("position", new Vector3(0, -167, 0), "easetype", iTween.EaseType.easeInOutSine, "time", 5f));
        }

        void WinAnimation()
        {
            coinAnimtion.GetComponent<Text>().text = this.coin;
            coinAnimtion.SetActive(true);
            Move();









        }





        void LossAnimation()
        {

        }

        void onCardSelected(string cardname, string coinss)
        {
            if (cardname.Equals(transform.name))
            {
                betStrip.SetActive(true);
                this.coin = coinss;

                bets = bets + int.Parse(coinss);

                betamount.GetComponent<Text>().text = "" + bets;






            }
        }


        // Update is called once per frame
        void Update()
        {
            userCoin.text = PlayerPrefs.GetString(PlayerDetails.Coin);

        }
    }
}