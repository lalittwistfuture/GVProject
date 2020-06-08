using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;
namespace jkq
{
    public class History : MonoBehaviour
    {
        public GameObject loadingPanel;
        public GameObject loading;
        public GameObject historyPanel;

        public GameObject cellSample;


        string winningCard = "0";
        string historyNo = "0";
        string cardAnnounceTime;
        public GameObject Cointaier;
        // Use this for initialization
        void Start()
        {

            historyPanel.SetActive(false);


        }

        private void OnEnable()
        {
            loadingPanel.SetActive(true);
            StartCoroutine(ServerRequest());


        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
            if (loading.activeSelf)
            {
                loading.transform.Rotate(Vector3.back, 5f);
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
            form.AddField("TAG", "JKQ_GAME_HISTORY");
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
                            for (int i = 0; i < data1.Count; i++)
                            {


                                JSONNode dat = data1[i];
                                GameObject btg = Instantiate(this.cellSample);
                                btg.GetComponent<JKQ_HISTORY_CELL>().serial.text = "" + (i + 1);
                                btg.GetComponent<JKQ_HISTORY_CELL>().resultTime.text = "" + dat["card_announce"];
                                btg.GetComponent<JKQ_HISTORY_CELL>().card.sprite = Resources.Load<Sprite>("Images/" + dat["winning_card"]);
                                btg.transform.SetParent(this.Cointaier.transform);
                                btg.transform.localScale = new Vector3(1f, 1f, 1f);

                            }
                            /* this.historyNo = dat["id"];
                             this.cardAnnounceTime = dat["card_announce"];


                             Debug.Log(response);


                             cardImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + this.winningCard);
                             ticketno.text = "" + (this.historyNo);
                             resultTime.text = "" + (this.cardAnnounceTime);*/


                            loadingPanel.SetActive(false);
                            historyPanel.SetActive(true);


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
            else
            {
                Debug.Log("Start Loading " + www.error);
            }

        }

    }
}




