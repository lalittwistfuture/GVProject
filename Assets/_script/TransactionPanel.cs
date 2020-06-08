using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;
namespace Ludo
{
    public class TransactionPanel : MonoBehaviour
    {

        public GameObject CloseBtn;
        public GameObject PurchaseBtn;
        public GameObject RedeemBtn;
        public GameObject NoRecord;
        public GameObject loading;

        public GameObject container;
        public GameObject purchase_cell;
        public GameObject redeem_cell;

        // Use this for initialization
        void Start()
        {
            loading.SetActive(false);
            NoRecord.SetActive(false);
        }


        // Update is called once per frame
        void Update()
        {
            if (loading.activeSelf)
            {
                loading.transform.Rotate(Vector3.back, 3, Space.World);
            }
        }

        public void ShowPanel()
        {
            Debug.Log("ShowPanel working ");
            CoinPurchaseAction();
        }


        public void ClosePanel()
        {

            Debug.Log("ClosePanel working ");
            transform.gameObject.SetActive(false);
        }

        public void CoinPurchaseAction()
        {
            PurchaseBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/blue_panal");
            RedeemBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/gray_panal_samll");
            GameObject despop = GameObject.FindGameObjectWithTag("PopUp");
            Destroy(despop);
            try
            {
                GameObject[] histories = GameObject.FindGameObjectsWithTag("Redeem");
                foreach (GameObject his in histories)
                {
                    Destroy(his);
                }
                GameObject[] histories1 = GameObject.FindGameObjectsWithTag("purchase");
                foreach (GameObject his in histories1)
                {
                    Destroy(his);
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("CoinPurchaseAction Exception " + ex.Message);
            }
            getPurchaseHistory();

        }

        public void CoinRedeemAction()

        {
            GameObject despop = GameObject.FindGameObjectWithTag("PopUp");
            Destroy(despop);
            RedeemBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/blue_panal");
            PurchaseBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/gray_panal_samll");

            try
            {

                GameObject[] histories = GameObject.FindGameObjectsWithTag("Redeem");
                foreach (GameObject his in histories)
                {
                    Destroy(his);
                }
                GameObject[] histories1 = GameObject.FindGameObjectsWithTag("purchase");
                foreach (GameObject his in histories1)
                {
                    Destroy(his);
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("CoinRedeemAction Exception " + ex.Message);
            }
            Debug.Log("CoinRedeemAction working ");
            getRedeemHistory();

        }

        void getPurchaseHistory()
        {
            //user_id=84
            //	loading.SetActive (false);
            Debug.Log("player id " + PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID));
            WWWForm form = new WWWForm();
            form.AddField("TAG", "PAYMENT_HISTORY");
            form.AddField("user_id", PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID));
            //form.AddField ("password", pass);
            WWW w = new WWW(Tags.URL, form);
            StartCoroutine(ServerRequestPurchase(w));
            loading.SetActive(true);

        }

        private IEnumerator ServerRequestPurchase(WWW www)
        {
            yield return www;

            if (www.error == null)
            {
                loading.SetActive(false);
                string response = www.text;
                try{
                Debug.Log("Responce " + response);
                JSONNode node = JSON.Parse(response);
                Debug.Log("node " + node.ToString());
                loading.SetActive(false);
                if (node != null)
                {
                    try
                    {
                        string status = node["status"];
                        JSONNode data = node["data"];

                        if (status.Equals("OK"))
                        {

                            if (data.Count <= 0)
                            {
                                NoRecord.SetActive(true);
                            }
                            else
                            {
                                NoRecord.SetActive(false);
                            }
                            for (int i = 0; i < data.Count; i++)
                            {

                                string status_type = "";

                                JSONNode userHis = data[i];
                                Debug.Log("userHis " + userHis.ToString());
                                string request_id = userHis["request_id"];
                                string payed_amount = userHis["payed_amount"];
                                string payment_date = userHis["payment_date"];
                                string status_str = userHis["status"];

                                GameObject newCell = Instantiate(purchase_cell);
                                newCell.transform.SetParent(container.transform);
                                newCell.transform.localScale = new Vector3(1, 1, 1);

                                if (status_str.Equals("1"))
                                {
                                    status_type = "success";
                                }
                                else
                                {
                                    status_type = "Failed";
                                }
                             //   newCell.GetComponent<PurchaseCell>().UpdateCell(payed_amount, request_id, payment_date, status_type);

                            }

                        }
                        else
                        {
                            //print ("failed?");
                            GameConstantData.showToast(transform, node["message"]);

                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.Log(ex.Message);
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
                loading.SetActive(false);
            }

        }


        void getRedeemHistory()
        {
            //user_id=84
            loading.SetActive(false);
            Debug.Log("player id " + PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID));
            WWWForm form = new WWWForm();
            form.AddField("TAG", "REDEEM_HISTORY");
            form.AddField("user_id", PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID));
            //form.AddField ("password", pass);
            WWW w = new WWW(Tags.URL, form);
            StartCoroutine(ServerRequestRedeem(w));
            loading.SetActive(true);

        }

        private IEnumerator ServerRequestRedeem(WWW www)
        {
            yield return www;

            if (www.error == null)
            {
                loading.SetActive(false);
                string response = www.text;
                try{
                Debug.Log("Responce " + response);
                JSONNode node = JSON.Parse(response);
                loading.SetActive(false);
                if (node != null)
                {

                    string status = node["status"];
                    JSONNode data = node["data"];
                    //Debug.Log ("Data1 " + data1);
                    Debug.Log("Data " + data.ToString());
                    Debug.Log("status " + status);
                    if (status.Equals("OK"))
                    {

                        if (data.Count <= 0)
                        {
                            NoRecord.SetActive(true);
                        }
                        else
                        {
                            NoRecord.SetActive(false);
                        }
                        for (int i = 0; i < data.Count; i++)
                        {
                            string status_type = "";
                            JSONNode userHis = data[i];
                            string amount = userHis["amount"];
                            string date = userHis["date"];
                            string status_str = userHis["status"];
                            if (status_str.Equals("1"))
                            {
                                status_type = "success";
                            }
                            else
                            {
                                status_type = "Failed";
                            }
                            GameObject newCell = Instantiate(redeem_cell);
                            newCell.transform.SetParent(container.transform);
                            newCell.transform.localScale = new Vector3(1, 1, 1);
                           // newCell.GetComponent<RedeemCell>().UpdateCell(amount, date, status_type);
                        }

                    }
                    else
                    {
                        //print ("failed?");
                        NoRecord.SetActive(true);
                        GameConstantData.showToast(transform, node["message"]);

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
                loading.SetActive(false);
            }

        }





    }
}