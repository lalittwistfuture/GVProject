using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class LobbyPanel : MonoBehaviour
{
    public GameObject setting;
    public GameObject variationPanel;
    public Text name;
    public Text coin;
    public GameObject image;
    private string image1;
    int Coins;
    public GameObject AppUpdatePopUp;
    public GameObject LoadingScreen;
    public GameObject LoadingIcon;

    // Start is called before the first frame update
    void Start()
    {

        Screen.orientation = ScreenOrientation.Portrait;


        try
        {
            Debug.Log("Lobby " + PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN));
            Coins = int.Parse(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN));
            setting.SetActive(false);
            variationPanel.SetActive(false);
            name.text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_NAME);
            coin.text = Coins.ToString();
            Debug.Log(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_IMAGE));
            image1 = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_IMAGE);
            image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Avtar/" + image1);
            PlayerPrefs.SetInt("Sound", 1);

        }catch(System.Exception ex){
            Debug.Log(ex.Message);
        }


    }
    private void OnEnable()
    {
        LoadingScreen.SetActive(true);
        StartCoroutine(ServerRequest());
       
    }
    public void Balance()
    {
    }

    void FixedUpdate()
    {
        if (LoadingIcon.activeSelf)
        {
            LoadingIcon.transform.Rotate(Vector3.back, 5, Space.World);
        }


    }

    public void Setting()
    {

        setting.SetActive(true);
    }
    public void Variation()
    {
        GameControllerTeenPatti.isChallenge = false;
        GameControllerTeenPatti.variation = true;
        GameControllerTeenPatti.GameType = TagsTeenpatti.PUBLIC;
        GameControllerTeenPatti.PrivateGameType = "";
        //  MyGameController.rummyGame.GameType = Game.PRACTICE;
        SceneManager.LoadSceneAsync("AmountSelectionTeenpatti");
    }
    public void PlayNow()
    {
        GameControllerTeenPatti.isChallenge = false;
        GameControllerTeenPatti.variation = false;
        GameControllerTeenPatti.GameType = TagsTeenpatti.PUBLIC;
        GameControllerTeenPatti.PrivateGameType = "";
        //  MyGameController.rummyGame.GameType = Game.PRACTICE;
        SceneManager.LoadSceneAsync("AmountSelectionTeenpatti");
    }
    public void PlayPrivateAction()
    {
        GameControllerTeenPatti.isChallenge = true;
        GameControllerTeenPatti.variation = true;
        GameControllerTeenPatti.GameType = TagsTeenpatti.PRIVATE;
        SceneManager.LoadSceneAsync("TeenPattiPrivate");

    }


    public void playLudo()
    {
       
        SceneManager.LoadSceneAsync("LobiScene");

    }

    public void PlayLotto()
    {
        
        SceneManager.LoadSceneAsync("LottoLobby");

    }

    private IEnumerator ServerRequest()
    {


        Debug.Log("Start Loading");
        WWWForm form = new WWWForm();
        form.AddField("TAG", "GET_COIN");
        form.AddField("mobile", PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_MOBILE));
      
        WWW www = new WWW(TagsTeenpatti.URL, form);
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
                        JSONNode data = data1[0];
                     
                        string coin1 = data["balance"];
                            string appversion = data["app_version"];
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_COIN, "" + (coin1));
                        coin.text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN);

                            if (float.Parse(appversion) > float.Parse(Application.version))
                            {
                                AppUpdatePopUp.SetActive(true);
                            }
                            else
                            {
                                AppUpdatePopUp.SetActive(false);
                            }
                           
                            // Debug.Log(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN));





                        }
                    catch
                    {
                        Debug.Log("Message");
                    }
                }
                else
                {
                    //print ("failed?");
                    //GameController.showToast (node ["message"]);

                }
                     LoadingScreen.SetActive(false);

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


    public void updateApp()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.casinobluemoon.app");
    }

    public void Jkq()
    {
        SceneManager.LoadScene("JKQLobby");
    }
    public void Roulette()
    {
        SceneManager.LoadScene("RoulletLobby");
    }
}
