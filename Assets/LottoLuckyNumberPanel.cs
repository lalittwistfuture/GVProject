using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;
using UnityEngine.SceneManagement;
namespace Lotto
{

    public class LottoLuckyNumberPanel : MonoBehaviour
    {

        DateTime waitingTime;
        bool waiting = false;
        [SerializeField]
        Text[] NumberPanel;
        [SerializeField]
        Text Timer;
        public GameObject anim;
        public GameObject logOut;
        // Use this for initialization
        void Start()
        {
            anim.SetActive(false);
            logOut.SetActive(false);



        }

        private void OnEnable()
        {
            for (int i = 0; i < 7; i++)
            {
                NumberPanel[i].text = "";
            }
            waiting = true;
            DateTime.TryParse(PlayerPrefs.GetString(LOTTOGame.TICKET_ANNOUNCE_TIME), out this.waitingTime);
        }


        private IEnumerator getTicket()
        {
            // yield return new WaitForSeconds(3.0f);

            WWWForm form = new WWWForm();
            form.AddField("TAG", "LOTTO_GAME_SESSION");
            form.AddField("session", PlayerPrefs.GetString(LOTTOGame.SESSION));
            WWW www = new WWW(Tags.URL, form);
            yield return www;
            if (www.error == null)
            {
                string response = www.text;
                Debug.Log(PlayerPrefs.GetString(LOTTOGame.SESSION) + " response" + response);
                JSONNode node;
               
                node = JSON.Parse(response);
               
              
                if (node != null)
                {
                    string result = node["status"];
                    if (result.Equals("OK"))
                    {
                        string number = "";
                        try
                        {

                            JSONNode data1 = node["data"];
                            JSONNode data = data1[0];
                            number = data["winning_ticket"];
                            anim.SetActive(true);



                        }
                        catch (System.Exception ex)
                        {
                            Debug.Log(ex.Message);
                        }

                        if (!number.Equals(""))
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                NumberPanel[i].text = "" + number.Substring(i, 1);
                                yield return new WaitForSeconds(1.0f);




                            }
                            StartCoroutine(closeSession());
                        }
                        else
                        {
                            StartCoroutine(getTicket());

                        }

                    }
                    else
                    {

                    }
                }
                GameController.startGame();
                anim.SetActive(false);
                
            }
            else
            {

            }


        }

        IEnumerator closeSession()
        {
            yield return new WaitForSeconds(2.0f);
            LottoGameController.closeSession();
        }



        // Update is called once per frame
        void Update()
        {






            if (this.waiting)
            {
                Timer.text = "0" + (this.waitingTime - DateTime.Now).Minutes + ":" + (this.waitingTime - DateTime.Now).Seconds;

                if (DateTime.Now >= this.waitingTime)
                {
                    this.waiting = false;
                    Debug.Log("call for ticket..");
                    StartCoroutine(getTicket());

                }
                if ((this.waitingTime - DateTime.Now).Seconds < 10)
                {
                    Timer.text = "0" + (this.waitingTime - DateTime.Now).Minutes + ":" + "0" + (this.waitingTime - DateTime.Now).Seconds;
                }
            }

        }
        public void Back()
        {
            logOut.SetActive(true);

        }
    }
}