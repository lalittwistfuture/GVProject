using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class BottomScript : MonoBehaviour
    {

        public GameObject peoplepanel;
        public GameObject redeempanel;
        public GameObject Settingpanel;
        public GameObject loading;
        public GameObject cell;
        public GameObject container;
        public GameObject NoFriendFound;

        // Use this for initialization
        void Start()
        {

            peoplepanel.SetActive(false);
            redeempanel.SetActive(false);
            Settingpanel.SetActive(false);
            loading.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (loading.activeSelf)
            {
                loading.transform.Rotate(Vector3.back, 3, Space.World);
            }

        }

        //1867629276901214
        public void LogoutAction()
        {
            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_ID, "");
            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_NAME, "");
            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_IMAGE, "");
            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_EMAIL, "");
            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_COIN, "");
            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_PASSWORD, "");
            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_FBID, "");
         //   GameConstantData.MyFriendsList.Clear();
            transform.gameObject.SetActive(false);
            SceneManager.LoadSceneAsync("firstScene");

        }
        public void PeopleAction()
        {

            string msg = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_NAME) + " is invite you to play Dreamz Club Ludo. Please download the game from: " + PlayerPrefs.GetString(Tags.APP_DOWNLOAD_URL);
            GameConstantData.shareText(msg);




            /*

            for (int i = 0; i < container.transform.childCount; i++) {
                Destroy (container.transform.GetChild (i).gameObject);
            }

            peoplepanel.SetActive (true);
            WWWForm form = new WWWForm ();
            form.AddField ("TAG", "ALLUSER");
            form.AddField ("id", PlayerPrefs.GetString (GetPlayerDetailsTags.PLAYER_ID));
            WWW w = new WWW (Tags.URL, form);
            StartCoroutine (ServerRequest (w));
            loading.SetActive (true);*/
            //		if (PlayerPrefs.GetString (GetPlayerDetailsTags.PLAYER_FBID).Length != 0) {
            //			peoplepanel.SetActive (true);
            //			peoplepanel.GetComponent<InviteFacebookFriends> ().ShowPanel ();
            //			if (GameConstantData.MyFriendsList.Count > 0) {
            //				NoFriendFound.SetActive (false);
            //			}
            //			foreach (FacebookFrndDetails frndList in GameConstantData.MyFriendsList) {
            //				string name = frndList.name;
            //				string fbID = frndList.fbID;
            //				string fbImage = frndList.imageUrl;
            //				//print (" Name " + name + " fb id " + fbID + " fb image " + fbImage);
            //
            //				GameObject newCell = Instantiate (cell);
            //				newCell.transform.SetParent (container.transform);
            //				newCell.transform.localScale = new Vector3 (1, 1, 1);
            //				newCell.GetComponent<FacebookFriendCell> ().UpdateCell (fbID, name, fbImage, false);
            //
            //			}
            //		} else {
            //
            //			 ("Please login with facebook? to share friend"); 
            //
            //		}

        }

        public void SettingAction()
        {
            try
            {
                Settingpanel.SetActive(true);
                Settingpanel.GetComponent<SettingPanelScript>().ShowPopup();
            }
            catch (System.Exception ex)
            {
                Debug.Log("SettingAction exception " + ex.Message);
            }
        }

        public void RedeemCashAction()
        {
            redeempanel.SetActive(true);
         //   redeempanel.GetComponent<RedeemPanel>().ShowPanel();
        }

        private IEnumerator ServerRequest(WWW www)
        {
            yield return www;

            if (www.error == null)
            {

                string response = www.text;
                Debug.Log("response " + response);
                try
                {
                    JSONNode node = JSON.Parse(response);
                    loading.SetActive(false);
                    if (node != null)
                    {

                        string result = node["status"];
                        if (result.Equals("OK"))
                        {

                            NoFriendFound.SetActive(false);
                            JSONNode FrndList = node["data"];
                            Debug.Log("count " + FrndList.Count);
                            for (int i = 0; i < FrndList.Count; i++)
                            {
                                JSONNode data = FrndList[i];
                                string FrndName = data["name"];
                                string MyID = data["id"];
                                string myImage = data["image"];
                                GameObject newCell = Instantiate(cell);
                                newCell.transform.SetParent(container.transform);
                                newCell.transform.localScale = new Vector3(1, 1, 1);
                                //    newCell.GetComponent<FacebookFriendCell>().UpdateCell(MyID, FrndName, myImage, false);
                            }


                        }
                        else
                        {
                            NoFriendFound.SetActive(true);
                            GameConstantData.showToast(transform, node["message"]);

                        }

                    }
                    else
                    {
                        NoFriendFound.SetActive(true);
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            else
            {
                NoFriendFound.SetActive(true);
                loading.SetActive(false);
            }

        }
    }
}