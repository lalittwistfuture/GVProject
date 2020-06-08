using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAccountTeenpatti : MonoBehaviour {

    public InputField playerName;
    //public InputField playerMobile;
    public InputField playerPassword;
    public InputField playerConfPassword;
    public InputField mobileNo;
    public InputField refCode;
    public GameObject loading;
    //public GameObject tick;
    public GameObject SignupBtn;
    public bool isSelected = false;
    private GameObject PopUpContainer;
    public GameObject avatarPanel;
    public GameObject avatar;
    private string AvatarName = "AVTAR-0";

    // Use this for initialization
    void Start () {

        loading.SetActive (false);
        avatarPanel.SetActive (false);
        PopUpContainer = Instantiate ((GameObject) Resources.Load ("_prefeb/ShowMsgPanel"));
        PopUpContainer.transform.SetParent (transform);
        PopUpContainer.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
        PopUpContainer.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
        PopUpContainer.GetComponent<PopUp> ().msg.GetComponent<Text> ().text = "Say Hello";
        PopUpContainer.SetActive (false);

        PlayerPrefs.SetString (GetPlayerDetailsTags.PLAYER_IMAGE, "0");

    }

    // Update is called once per frame
    void Update () {

    }

    public void LogIn () {
        SceneManager.LoadScene ("LogIn");
    }

    void FixedUpdate () {
        if (loading.activeSelf) {
            loading.transform.Rotate (Vector3.back, 3, Space.World);
        }

        if (Input.GetKeyDown (KeyCode.Escape)) {
            Application.Quit ();
        }
    }

    public void CreateAccountAction () {
        //  AvatarName = PlayerPrefs.GetString(GetPlayerDetailsTags.PLAYER_IMAGE);
        Debug.Log (AvatarName);

        if (Application.internetReachability != NetworkReachability.NotReachable) {
            if (ValidateField ()) {
                loading.SetActive (true);
                WWWForm form = new WWWForm ();
                form.AddField ("TAG", "SIGNUP");
                form.AddField ("name", playerName.GetComponent<InputField> ().text);
                form.AddField ("password", playerPassword.GetComponent<InputField> ().text);
                form.AddField ("mobile", mobileNo.GetComponent<InputField> ().text);
                form.AddField ("refralCode", refCode.GetComponent<InputField> ().text);
                form.AddField ("picture", PlayerPrefs.GetString (GetPlayerDetailsTags.PLAYER_IMAGE));
                WWW w = new WWW (TagsTeenpatti.URL, form);
                StartCoroutine (ServerRequest (w));
            } else {
                Debug.Log ("SignUp validation failed?");
            }
        } else {
            PopUpContainer.GetComponent<PopUp> ().msg.GetComponent<Text> ().text = "Internet connection not found. Please try again.";
            PopUpContainer.SetActive (true);
        }
    }
    private IEnumerator ServerRequest (WWW www) {
        yield return www;
        Debug.Log ("resp " + www.text);
        if (www.error == null) {
            try {
                string response = www.text;
                JSONNode node = JSON.Parse (response);

                if (node != null) {

                    string result = node["status"];
                    string msg = node["message"];
                    if (result.Equals ("OK")) {

                        JSONNode data1 = node["data"];
                        JSONNode data = data1[0];

                        try {
                            // parse data using key
                            string my_Id = data["id"];
                            string name = data["name"];

                            string mobile = data["mobile"];
                            string coin = data["balance"];
                            string parent_id = data["parent_id"];
                            string image = data["picture"];
                            string password = data["password"];
                            Debug.Log (image);

                            Debug.Log ("my_id " + my_Id + " name " + name + " mobile " + mobile + " balance " + coin + " parent_id " + parent_id + " picture " + image + " password " + password);
                            PlayerPrefs.SetString (GetPlayerDetailsTags.PLAYER_ID, "" + my_Id);
                            PlayerPrefs.SetString (GetPlayerDetailsTags.PLAYER_NAME, "" + name);

                            PlayerPrefs.SetString (GetPlayerDetailsTags.PLAYER_MOBILE, "" + mobile);
                            PlayerPrefs.SetString (GetPlayerDetailsTags.PLAYER_COIN, "" + coin);
                            PlayerPrefs.SetString (GetPlayerDetailsTags.PARENT_ID, "" + parent_id);
                            PlayerPrefs.SetString (GetPlayerDetailsTags.PLAYER_IMAGE, "" + image);
                            PlayerPrefs.SetString (GetPlayerDetailsTags.PLAYER_PASSWORD, password);

                            SceneManager.LoadSceneAsync ("MainLobby");

                        } catch (System.Exception ex) {
                            loading.SetActive (false);
                            Debug.Log ("Natasha Exception " + ex.Message);
                        }

                    } else {
                        //loading.SetActive (false);
                        GameControllerTeenPatti.showToast (node["message"]);

                        loading.SetActive (false);
                    }

                }
            } catch (System.Exception ex) {
                Debug.Log (ex.Message);
            }
        } else {
            loading.SetActive (false);
            //GameController.showToast ("response " + www.error.ToString ());
        }

    }
    public void SelectAvatar () {
        avatarPanel.SetActive (true);
    }

    bool ValidateField () {

        // validateName
        if (playerName.text.Length >= 1) {

        } else {
            PopUpContainer.SetActive (true);
            PopUpContainer.GetComponent<PopUp> ().msg.GetComponent<Text> ().text = "Please Enter Your Name";

            return false;
        }

        // validate email

        if (playerPassword.text.Length >= 1) {

        } else {
            PopUpContainer.SetActive (true);
            PopUpContainer.GetComponent<PopUp> ().msg.GetComponent<Text> ().text = "Enter Password";

            return false;
        }
        if (mobileNo.text.Length >= 1) {

            if (mobileNo.text.Length >= 10) {

            } else {
                PopUpContainer.SetActive (true);
                PopUpContainer.GetComponent<PopUp> ().msg.GetComponent<Text> ().text = "Invalid Mobile No";

                return false;

            }
        } else {
            PopUpContainer.SetActive (true);
            PopUpContainer.GetComponent<PopUp> ().msg.GetComponent<Text> ().text = "Enter Mobile No";

            return false;
        }
        if (playerConfPassword.text.Length >= 1) {

            if (playerPassword.text.Equals (playerConfPassword.text)) {

                return true;
            } else {
                PopUpContainer.SetActive (true);
                PopUpContainer.GetComponent<PopUp> ().msg.GetComponent<Text> ().text = "InValid  Confirm Password";

                return false;

            }
        } else {
            PopUpContainer.SetActive (true);
            PopUpContainer.GetComponent<PopUp> ().msg.GetComponent<Text> ().text = "Enter Password";

            return false;
        }

    }
}