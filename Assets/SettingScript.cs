using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class SettingScript : MonoBehaviour
{
    public Text Profile;
    public Text name1;
    public Text mobile;
    public GameObject logoutPanel;
    public GameObject profile;
    public GameObject sound;
    public GameObject vibration;
    public Sprite onImage;
    public Sprite offImage;
    public GameObject AvatarPanel;
     
    int soundOn;
    int vibrate;

    public GameObject updateNamePanel;
    // Start is called before the first frame update
    void Start()
    {
        AvatarPanel.SetActive(false);
        logoutPanel.SetActive(false);
        Profile.text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_NAME);
        name1.text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_NAME);
        mobile.text = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_MOBILE);
        profile.GetComponent<Image>().sprite = Resources.Load<Sprite>("Avtar/" + PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_IMAGE));
        updateNamePanel.SetActive(false);
    }

    public void updateAvatarAction()
    {
        AvatarPanel.SetActive(true);
    }

    private void OnEnable()
    {
        GameDelegateTeenPatti.onUpdsteImage += updateImage;
    }

    private void OnDisable()
    {
        GameDelegateTeenPatti.onUpdsteImage -= updateImage;
    }

    public void updateNameAction()
    {
        updateNamePanel.SetActive(true);
        transform.gameObject.SetActive(false);
    }


    void updateImage(string image)
    {
        StartCoroutine(ServerRequest(image));
    }
    private IEnumerator ServerRequest(string image)
    {
        WWWForm form = new WWWForm();
        form.AddField("TAG", "UPDATE_USER");
        form.AddField("pic", image);
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
                            string name2 = data["name"];
                            string mobile1 = data["mobile"];
                            string coin = data["balance"];
                            string parent_id = data["parent_id"];
                            string image1 = data["picture"];
                            string password = data["password"];

                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_ID, "" + my_Id);
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_NAME, "" + name2);
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_MOBILE, "" + mobile1);
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_COIN, "" + coin);
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PARENT_ID, "" + parent_id);
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_IMAGE, "" + image1);
                            PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_PASSWORD, password);

                            Debug.Log("PlayerDetails.Password");
                            SceneManager.LoadScene(SceneClass.MAIN_LOBBY);
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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Back()
    {
        transform.gameObject.SetActive(false);
    }
    public void SoundAction()
    {
       if (soundOn == 1){
            sound.GetComponent<Image>().sprite = offImage;
            PlayerPrefs.SetInt("Sound", 0);
            soundOn = 0;
        }
        else
        {
            sound.GetComponent<Image>().sprite = onImage;
            PlayerPrefs.SetInt("Sound", 1);
            soundOn = 1;
        }
    }
    public void Vibrat()
    {
        if (vibrate == 1)
        {
            vibration.GetComponent<Image>().sprite = offImage;

            vibrate = 0;
        }
        else
        {
            vibration.GetComponent<Image>().sprite = onImage;
         
            vibrate = 1;


        }
    }
    
    public void LogOut()
    {
        logoutPanel.SetActive(true);
    }
    public void privacyPolicy()
    {
        Application.OpenURL("http://casinobluemoon.com/privacy-policy.php");
    }
}
