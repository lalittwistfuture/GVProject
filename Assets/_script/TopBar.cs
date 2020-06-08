using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.SceneManagement;
namespace Ludo
{
    public class TopBar : MonoBehaviour
    {

        public Image Pic;
        public Text Name;
        public Text Coin;
       // public GameObject BuyCoinPanel;
     //   public GameObject userProfilePanel;
        public GameObject ClosePanel;
        public GameObject MenuPanel;

        // Use this for initialization
        void Start()
        {
          //  BuyCoinPanel.SetActive(false);
         //   userProfilePanel.SetActive(false);
            ClosePanel.SetActive(false);
            MenuPanel.SetActive(false);

        }


        // Update is called once per frame
        void Update()
        {

        }


        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Debug.Log("pauseStatus true ");
                UpdateUserCoin();
            }
            else
            {
                Debug.Log("pauseStatus false");

            }
        }


        void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Debug.Log("Active Lobby ");
                UpdateUserCoin();
            }
            else
            {
                Debug.Log("Lobby is not active");

            }
        }

        void OnEnable()
        {

            //PlayerPrefs.GetString (GetPlayerDetailsTags.PLAYER_NAME);
            string name_upper = UppercaseFirst(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_NAME));
            Name.text = name_upper;
            UpdateUserCoin();
            Coin.text = "" + PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN);
            string playerImage = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_IMAGE);
            if (playerImage.Length != 0)
            {
                if (playerImage.Contains("AVTAR"))
                {
                    string[] img = playerImage.Split(new string[] { "-" }, System.StringSplitOptions.None);
                    Debug.Log("bdhvhjbds jndscjn " + img[1]);
                    Pic.sprite = Resources.Load<Sprite>("Avtar/" + img[1]);
                }
                else
                {
                    StartCoroutine(loadImage(playerImage));
                }

            }
            else
            {
                Debug.Log("profilePic null");
                Pic.sprite = Resources.Load<Sprite>("images/user-default");
            }
        }

        IEnumerator loadImage(string url)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.error == null)
            {
                Debug.Log("Downloading start " + url);
                Pic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                Debug.Log("Downloading complited");
            }
            else
            {
                Debug.Log("Error occur while downloading");
                Pic.sprite = Resources.Load<Sprite>("images/user-default");
            }
        }

        public string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public void BuyCoinAction()
        {
           //print ("BuyCoinAction");
         //   BuyCoinPanel.SetActive(true);
          //  BuyCoinPanel.GetComponent<AddCash>().ShowPanel();
        }

        public void UserProfileAction()
        {
            //print ("UserProfileAction");
         //   userProfilePanel.SetActive(true);
         //   userProfilePanel.GetComponent<PersonalProfile>().ShowPanel();
        }

        public void MenuBarAction()
        {
            ClosePanel.SetActive(true);
            MenuPanel.SetActive(true);
            MenuPanel.GetComponent<MenuBarScript>().ShowMenuBar();
        }

        public void UpdateUserCoin()
        {
            try
            {
                //if (PlayerPrefs.GetString (GetPlayerDetailsTags.PLAYER_ID).Length > 1) {
                WWWForm form = new WWWForm();
                form.AddField("TAG", "USER_INFO");
                form.AddField("id", PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID));
                WWW w = new WWW(Tags.URL, form);
                StartCoroutine(ServerRequest(w));
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        private IEnumerator ServerRequest(WWW www)
        {
            yield return www;

            if (www.error == null)
            {

                try
                {
                    string response = www.text;
                    Debug.Log("response " + response);
                    JSONNode node = JSON.Parse(response);
                    if (node != null)
                    {
                        //loading.SetActive (false);
                        try
                        {
                            string result = node["status"];
                            JSONNode data = node["data"];
                            if (result.Equals("OK"))
                            {
                                JSONNode data1 = data[0];
                                string coin_str = data1["coin"];
                                PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_COIN, coin_str);
                                Coin.text = "" + coin_str;

                            }
                            else
                            {
                                PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_ID, "");
                                PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_NAME, "");
                                PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_IMAGE, "");
                                PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_EMAIL, "");
                                PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_COIN, "");
                                PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_PASSWORD, "");
                                PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_FBID, "");
                              //  GameConstantData.MyFriendsList.Clear();
                                transform.gameObject.SetActive(false);
                                SceneManager.LoadSceneAsync("firstScene");
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

        }


    }
}