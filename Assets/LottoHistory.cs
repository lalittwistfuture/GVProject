using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;
namespace Lotto
{
    public class LottoHistory : MonoBehaviour
    {

        public GameObject loadingPanel;
        public GameObject loading;
        public GameObject historyPanel;
        public GameObject cellSample;
        public GameObject Cointaier;
        string winningCard = "0";
        string historyNo = "0";
        string cardAnnounceTime;

        // Use this for initialization
        void Start()
        {
            loadingPanel.SetActive(true);

        }
        private void OnEnable()
        {
            loadingPanel.SetActive(true);
            StartCoroutine(ServerRequest());


        }

        // Update is called once per frame
        void Update()
        {
            if (loading.activeSelf)
            {
                loading.transform.Rotate(Vector3.back, 5f);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }
        public void Back()
        {
            SoundManager.buttonClick();
            transform.gameObject.SetActive(false);
        }
        private IEnumerator ServerRequest()
        {
            Debug.Log("Start Loading");
            WWWForm form = new WWWForm();
            form.AddField("TAG", "LOTTO_GAME_HISTORY");
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
                            for (int i = 0; i < data1.Count; i++)
                            {


                                JSONNode dat = data1[i];
                                GameObject btg = Instantiate(this.cellSample);
                                btg.GetComponent<LOTTO_HISTORY>().serial.text = "" + (i + 1);
                                btg.GetComponent<LOTTO_HISTORY>().resultTime.text = "" + dat["ticket_announce"];
                                btg.GetComponent<LOTTO_HISTORY>().ticket.text = "" + dat["winning_ticket"];
                                btg.transform.SetParent(this.Cointaier.transform);
                                btg.transform.localScale = new Vector3(1f, 1f, 1f);
                            }


                            loadingPanel.SetActive(false);
                            historyPanel.SetActive(true);




                            /* JSONNode data1 = node["data"];
                         JSONNode dat = data1[0];
                         this.winningCard = dat["winning_ticket"];
                         this.historyNo = dat["id"];
                         this.cardAnnounceTime = dat["ticket_announce"];


                         Debug.Log(response);


                       /*  ticket.text = "" + this.winningCard;
                         ticketno.text = "" + (this.historyNo);
                         resultTime.text = "" + (this.cardAnnounceTime);
                         */






                        }
                        catch
                        {
                            Debug.Log("Message");
                        }




                    }
                    else
                    {
                        Debug.Log("Session closed");
                        //  loadingPanel.SetActive(true);


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
                Debug.Log("Start Loading " + www.error);
            }

        }

    }
}