using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.SceneManagement;
public class UpdateProfile : MonoBehaviour
{

    public InputField nameText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void submitName()
    {
        StartCoroutine(ServerRequest());
    }

    public void close()
    {
        transform.gameObject.SetActive(false);
    }

    private IEnumerator ServerRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("TAG", "UPDATE_USER");
        form.AddField("name", nameText.text);
        form.AddField("id", PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_ID));
        UnityWebRequest www = UnityWebRequest.Post(TagsTeenpatti.URL, form);
        yield return www.SendWebRequest();
        if (www.error == null)
        {
            try
            {
                string response = www.downloadHandler.text;
                JSONNode node = JSON.Parse(response);
                Debug.Log(response);
                if (node != null)
                {

                    string result = node["status"];
                    string msg = node["message"];
                    if (result.Equals("OK"))
                    {

                        //GameController.showToast ("you account created successfully ");
                        // save data to game prefrance

                        JSONNode data1 = node["data"];
                        JSONNode data = data1[0];

                        // parse data using key

                        try
                        {
                            // parse data using key
                            string my_Id = data["id"];
                            string name1 = data["name"];
                            string mobile = data["mobile"];
                            string coin = data["balance"];
                            string parent_id = data["parent_id"];
                            string image = data["picture"];
                            string password = data["password"];

                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_ID, "" + my_Id);
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_NAME, "" + name1);
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_MOBILE, "" + mobile);
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_COIN, "" + coin);
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PARENT_ID, "" + parent_id);
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_IMAGE, "" + image);
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_PASSWORD, password);

                            SceneManager.LoadScene(SceneClass.MAIN_LOBBY);
                            Debug.Log("PlayerDetails.Password");

                            Debug.Log("my_id " + my_Id + " name " + name + "  mobile " + mobile + " balance " + coin + " parent_id " + parent_id + " picture " + image + " password " + password);

                        }
                        catch (System.Exception ex)
                        {
                           
                            Debug.Log("Natasha Exception " + ex.Message);
                        }


                        // call the next scene


                    }
                    else
                    {
                        //loading.SetActive (false);
                        GameControllerTeenPatti.showToast(node["message"]);

                       
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
            GameController.showToast("response " + www.error.ToString());
        }

    }


}
