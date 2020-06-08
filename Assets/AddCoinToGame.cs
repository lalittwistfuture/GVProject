using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
public class AddCoinToGame : MonoBehaviour
{
    public Text minCoin;
    public Text maxCoin;
    public Slider slider;
    public Text currentValue;
    public GameObject Loading;
    private int initailCoin = 0;
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
        slider.value = 0;
        slider.minValue = 0;
        maxCoin.text = "0";
        slider.maxValue = 0;
        minCoin.text = "0" ;
        currentValue.text = "0" ;
        Loading.SetActive(true);
        initailCoin = 0;
        StartCoroutine(ServerRequest());
	}

    public void setInitialCoin(int value)
    {
        initailCoin = value;
    }

    public void addCoin(){
        if (!Loading.activeSelf)
        {
            appwrapTeenpatti.addCoinToGame(int.Parse(currentValue.text));
            transform.gameObject.SetActive(false);
        }
    }

    public void hidePopUp()
    {
        transform.gameObject.SetActive(false);
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
                            if (int.Parse(coin1) > GameControllerTeenPatti.MaxBetAmt)
                            {
                                PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_COIN, "" + (coin1));
                                slider.value = GameControllerTeenPatti.MaxBetAmt >= initailCoin ? GameControllerTeenPatti.MaxBetAmt :initailCoin;
                                slider.minValue = GameControllerTeenPatti.MaxBetAmt >= initailCoin ? GameControllerTeenPatti.MaxBetAmt : initailCoin;
                                maxCoin.text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN);
                                minCoin.text = GameControllerTeenPatti.MaxBetAmt >= initailCoin ? "" + GameControllerTeenPatti.MaxBetAmt : "" + initailCoin;
                                currentValue.text = GameControllerTeenPatti.MaxBetAmt >= initailCoin ? "" + GameControllerTeenPatti.MaxBetAmt:""+initailCoin;
                                slider.maxValue = int.Parse(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN));
                                // Debug.Log(PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_COIN));

                            }
                            else
                            {
                                GameController.showToast("You do not have sufficient coins to continue game.");
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
            }catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
        }
        else
        {
            //loading.SetActive (false);
        }

    }


}
