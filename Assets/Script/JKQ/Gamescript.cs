using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using SimpleJSON;
namespace jkq
{
    public class Gamescript : MonoBehaviour
    {
        public GameObject BetSelection;
        public GameObject PopUpContainer;

        // Use this for initialization
        void Start()
        {
            PopUpContainer = Instantiate((GameObject)Resources.Load("_prefeb/PopUp 2"));
            PopUpContainer.transform.SetParent(transform);
            PopUpContainer.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            PopUpContainer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            PopUpContainer.GetComponent<CloseScript>().Msg.GetComponent<Text>().text = "Welcome";
            PopUpContainer.SetActive(false);
            BetSelection.SetActive(false);
            onChangeTable();
        }


        void onChangeTable()
        {
            int number = Random.Range(1, 12);
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Table/t" + number);
        }


        public void Cards(GameObject btn)
        {
            if (GameController.bettingTime)
            {
                BetSelection.SetActive(true);
                BetSelection.GetComponent<BetScript>().showImage(btn.GetComponent<Image>().sprite, btn.name);
            }
            else
            {
                Debug.Log("Betting time is close");
                PopUpContainer.SetActive(true);
                PopUpContainer.GetComponent<CloseScript>().Msg.GetComponent<Text>().text = "Betting time is close";

            }
            ScaleUp(btn);
        }

        void ScaleUp(GameObject btn)
        {
            Hashtable paramHashtable = new Hashtable();
            paramHashtable.Add("value", btn);
            iTween.ScaleTo(btn, iTween.Hash("scale", new Vector3(1.1f, 1.1f, 1.1f), "time", 0.2, "oncomplete", "ScaleDown", "transition", "easeInOutExpo", "oncompletetarget", this.gameObject, "oncompleteparams", paramHashtable));
        }

        void ScaleDown(object cmpParams)
        {
            Hashtable hstbl = (Hashtable)cmpParams;
            GameObject btn = (GameObject)hstbl["value"];
            iTween.ScaleTo(btn, iTween.Hash("scale", new Vector3(1, 1, 1), "time", 0.2));
        }

        private void OnEnable()
        {
            GameController.onRecievedMessage += onRecievedMessage;
            GameController.onChangeTable += onChangeTable;
        }

        private void OnDisable()
        {
            GameController.onRecievedMessage -= onRecievedMessage;
            GameController.onChangeTable -= onChangeTable;
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
                            if (BetSelection.activeSelf)
                            {

                                // if()



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


        // Update is called once per frame
        void Update()
        {

        }

    }
}