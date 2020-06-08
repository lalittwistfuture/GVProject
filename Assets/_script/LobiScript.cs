using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;
namespace Ludo
{


    public class LobiScript : MonoBehaviour
    {
        public Text name;
        public Text coin;
        public GameObject image;
        private string image1;
        //public GameObject InviteFacebookPanel;
        //	public GameObject FrndReachOnlinePanel;
        //public GameObject ClosePanel;
        public GameObject CreatePrivateTablePanel;
       // public GameObject QuitApplication;
       // public GameObject MenuBarPanel;
        //	public GameObject cell;

        //	public GameObject container;
       // public GameObject NoFriendFound;
        //public GameObject OneOnOnePanel;
        //public GameObject SelectAmountPanel;
      //  public GameObject profilePanel;
      //  public GameObject redeemCoinPanel;
      //  public GameObject SettingPanel;
      //  public GameObject BuyCoinPanel;
      //  public GameObject InviteFacebookPanel;
        public GameObject ErrorMsgPanel;
      //  public GameObject TransactionPanel;
      //  public GameObject FrndReachOnlinePanel;
      //  public GameObject ClosePanel;
     //   public GameObject ChangePasswordPanel;
        // Use this for initialization
        //public GameObject numberOfType;
        //	public GameObject offlineBtn;
        //	public GameObject oneOnoneBtn;
        //	public GameObject frndsBtn;

        private string fcm_token;
        private IDictionary<string, string> dict;

        //public Text msg;

        void Start()
        {
            PlayerPrefs.SetInt(GameTags.MUSIC_ON, 1);
            name.text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_NAME);
            coin.text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN);
            image1 = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_IMAGE);
            image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Avtar/" + image1);
            //InviteFacebookPanel.SetActive (false);
            //ClosePanel.SetActive (false);
            CreatePrivateTablePanel.SetActive(false);
          //  QuitApplication.SetActive(false);
            //SelectAmountPanel.SetActive (false);
          //  MenuBarPanel.SetActive(false);
          //  BuyCoinPanel.SetActive(false);
           // profilePanel.SetActive(false);
          ////  redeemCoinPanel.SetActive(false);
          //  SettingPanel.SetActive(false);
         //   InviteFacebookPanel.SetActive(false);
            ErrorMsgPanel.SetActive(false);
         //   TransactionPanel.SetActive(false);
          //  ChangePasswordPanel.SetActive(false);
          //  FrndReachOnlinePanel.SetActive(false);
         //   ClosePanel.SetActive(false);
            GetUserID();
            PlayerPrefs.SetString(GameTags.FACEBOOK_FRIEND, "");
            PlayerPrefs.SetString(GameTags.CREATE_TABLE, "");
            PlayerPrefs.SetString(GameTags.CHALLENGE_FRIEND, "");
            PlayerPrefs.SetString(GameTags.PRIVATE_TABLE_TYPE, "");
            PlayerPrefs.SetString(GameTags.OFFLINE, "");
            PlayerPrefs.SetString(GameTags.CHALLENGE_FRIEND, "");
            PlayerPrefs.SetString(GetPlayerDetailsTags.ROOM_ID, "");
            GameController.Message = "";
            GameController.Message1 = "";

            if (PlayerPrefs.GetInt(GameTags.MUSIC_ON) == 1)
            {

                GetComponent<AudioSource>().loop = true;
                GetComponent<AudioSource>().Play();

            }
            else
            {
                GetComponent<AudioSource>().Stop();
            }





            PlayerPrefs.SetInt(LudoTags.GOTI_LIMIT, 4);


        }


        private void OnEnable()
        {
            LudoDelegate.onChangeSoundSetting += onChangeSoundSetting;

        }

        private void OnDisable()
        {
            LudoDelegate.onChangeSoundSetting -= onChangeSoundSetting;

        }


        void onChangeSoundSetting(bool value)
        {
            Debug.Log("Working " + value);
            if (value)
            {
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                if (GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().Stop();
                }
            }
        }


        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }
             /*   if (MenuBarPanel.activeSelf)
                {
                    MenuBarPanel.SetActive(false);
                    ClosePanel.SetActive(false);
                }
                else if (CreatePrivateTablePanel.activeSelf)
                {
                    PlayerPrefs.SetString(GameTags.GAME_TYPE, "");
                    CreatePrivateTablePanel.SetActive(false);
                }
                else if (profilePanel.activeSelf)
                {
                    profilePanel.SetActive(false);
                }
                else if (BuyCoinPanel.activeSelf)
                {
                    BuyCoinPanel.SetActive(false);
                }
                else if (redeemCoinPanel.activeSelf)
                {
                    redeemCoinPanel.SetActive(false);
                }
                else if (SettingPanel.activeSelf)
                {
                    SettingPanel.SetActive(false);
                }
                else if (InviteFacebookPanel.activeSelf)
                {
                    InviteFacebookPanel.SetActive(false);
                }
                else if (ErrorMsgPanel.activeSelf)
                {
                    ErrorMsgPanel.SetActive(false);
                }
                else if (TransactionPanel.activeSelf)
                {
                    TransactionPanel.SetActive(false);
                }
                else if (FrndReachOnlinePanel.activeSelf)
                {
                    FrndReachOnlinePanel.SetActive(false);
                    ClosePanel.SetActive(false);
                }
                else if (ChangePasswordPanel.activeSelf)
                {
                    ChangePasswordPanel.SetActive(false);

                }
                else
                {
                    QuitApplication.SetActive(true);
                }*/
         

        void HidePopUp()
        {
            GameObject pop = GameObject.FindGameObjectWithTag("PopUp");
            if (pop)
                pop.SetActive(false);
        }








        public void one_on_one()
        {
           
            PlayerPrefs.SetInt(LudoTags.ROOM_TYPE, LudoTags.PUBLIC);
            PlayerPrefs.SetInt(LudoTags.GAME_TYPE, LudoTags.ONE_ONE);
            PlayerPrefs.SetInt(LudoTags.USER_LIMIT, 2);
            CreatePrivateTablePanel.SetActive(true);
            // CreatePrivateTablePanel.GetComponent<LudoGame.CreatePrivateTable>().ShowPopup("1 ON 1");

        }

        public void one_on_Four()
        {
            PlayerPrefs.SetInt(LudoTags.ROOM_TYPE, LudoTags.PUBLIC);
            PlayerPrefs.SetInt(LudoTags.GAME_TYPE, LudoTags.ONE_FOUR);
            PlayerPrefs.SetInt(LudoTags.USER_LIMIT, 4);
            CreatePrivateTablePanel.SetActive(true);
            // CreatePrivateTablePanel.GetComponent<LudoGame.CreatePrivateTable>().ShowPopup("4 PLAYERS");
        }

        public void PracticeGame()
        {
            PlayerPrefs.SetInt(LudoTags.GAME_TYPE, LudoTags.OFFLINE);
            SceneManager.LoadSceneAsync("GameScene");

        }

        public void PlayWithFrnds()
        {
            PlayerPrefs.SetInt(LudoTags.ROOM_TYPE, LudoTags.PRIVATE);
            PlayerPrefs.SetInt(LudoTags.GAME_TYPE, LudoTags.ONE_FOUR);
            PlayerPrefs.SetInt(LudoTags.USER_LIMIT, 2);
            SceneManager.LoadSceneAsync("GameScene");

        }

        public void facebookAction()
        {
            Application.OpenURL("http://www.facebook.com/");
        }

        public void rateUsAction()
        {
            Application.OpenURL("http://www.facebook.com/");
        }



      /*  public void ClosePanelAction()
        {
            ClosePanel.SetActive(false);
            FrndReachOnlinePanel.SetActive(false);
            MenuBarPanel.SetActive(false);
        }*/

        public void GetUserID()
        {
            string userID = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID);
            WWWForm form = new WWWForm();
            form.AddField("TAG", "GET_ACCOUNT");
            form.AddField("user_id", userID);
            WWW w = new WWW(Tags.URL, form);
            StartCoroutine(GetUserData(w));

        }





        private IEnumerator GetUserData(WWW www)
        {
            yield return www;
            if (www.error == null)
            {
                string response = www.text;
                try{
                //GameController.showToast ("response " + response);
                JSONNode node = JSON.Parse(response);
                if (node != null)
                {

                    string result = node["status"];
                    JSONNode data1 = node["data"];
                    JSONNode data = data1[0];
                    if (result.Equals("OK"))
                    {

                        ///GameController.showToast ("data found successfully  ");
                        string id = data["id"];
                        string name1 = data["name"];
                        string number = data["number"];
                        string BankIfsc = data["ifsc"];
                        string mobile = data["mobile"];

                        // set user data
                        PlayerPrefs.SetString(RedeemRequestTag.AccountHolderNames, name1);
                        PlayerPrefs.SetString(RedeemRequestTag.AccountNumber, number);
                        PlayerPrefs.SetString(RedeemRequestTag.PaytmMobile, mobile);
                        PlayerPrefs.SetString(RedeemRequestTag.IFSC, BankIfsc);


                    }
                    else
                    {
                        //print ("failed?");
                        //GameController.showToast (node ["message"]);

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
                //loading.SetActive (false);
            }

        }
        public void Back()
        {
            SceneManager.LoadScene("MainLobby");
        }















    }
}