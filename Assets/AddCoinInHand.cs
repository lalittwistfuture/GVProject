using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleJSON;
public class AddCoinInHand : MonoBehaviour
{
    public Text minCoin;
    public Text maxCoin;
    public Slider slider;
    public Text currentValue;
    public GameObject Loading;
    // Start is called before the first frame update
    void Start()
    {
        slider.wholeNumbers = true;
    }

    // Update is called once per frame()
    private void FixedUpdate()
    {
        if (Loading.activeSelf)
        {
            Loading.transform.Rotate(Vector3.back, 3, Space.World);
        }
    }


    public void onValueChange(){
        currentValue.text = "" + slider.value; 
    }

    private void OnEnable()
    {
        slider.value = GameControllerTeenPatti.MaxBetAmt;
        slider.minValue = GameControllerTeenPatti.MaxBetAmt;
        maxCoin.text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN);
        slider.maxValue = int.Parse(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN));
        minCoin.text = ""+GameControllerTeenPatti.MaxBetAmt;
        currentValue.text = ""+GameControllerTeenPatti.MaxBetAmt;
        Loading.SetActive(true);
        StartCoroutine(ServerRequest());
    }

    public void addCoin(){
        //appwrapTeenpatti.addCoinToGame(int.Parse(currentValue.text));
        //transform.gameObject.SetActive(false);
        if (!Loading.activeSelf)
        {
            int myCoin = int.Parse(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN));
            if (myCoin > GameControllerTeenPatti.MaxBetAmt)
            {
                slider.value = GameControllerTeenPatti.MaxBetAmt;
                slider.minValue = GameControllerTeenPatti.MaxBetAmt;
                slider.maxValue = myCoin;
                maxCoin.text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN);
                minCoin.text = "" + GameControllerTeenPatti.MaxBetAmt;
                currentValue.text = "" + GameControllerTeenPatti.MaxBetAmt;
                PlayerPrefs.SetString("InHand", currentValue.text);
                SceneManager.LoadScene("GameScene_Teenpatti");
            }
            else
            {
                GameController.showToast("You do not have sufficient coins to join this table.");
            }
           
        }
       
    }

    public void hidePopUp()
    {
        SceneManager.LoadScene("MainLobby");
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
            try
            {
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
                            Loading.SetActive(false);
                            string coin1 = data["balance"];
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_COIN, "" + (coin1));
                            int myCoin = int.Parse(coin1);
                            if (myCoin > GameControllerTeenPatti.MaxBetAmt)
                            {
                                slider.value = GameControllerTeenPatti.MaxBetAmt;
                                slider.minValue = GameControllerTeenPatti.MaxBetAmt;
                                slider.maxValue = int.Parse(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN));
                                maxCoin.text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN);
                                minCoin.text = "" + GameControllerTeenPatti.MaxBetAmt;
                                currentValue.text = "" + GameControllerTeenPatti.MaxBetAmt;
                            }
                            else
                            {
                                GameController.showToast("You do not have sufficient coins to join this table.");
                                slider.value = 0;
                                slider.minValue =0;
                                slider.maxValue = int.Parse(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN));
                                maxCoin.text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN);
                                minCoin.text = "0";
                                currentValue.text = "0";
                            }

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

                }
            }catch(System.Exception ex){
                Debug.Log(ex.Message);
            }
        }
        else
        {
            //loading.SetActive (false);
        }

    }

}
