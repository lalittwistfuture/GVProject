using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using SimpleJSON;
using UnityEngine.Networking;
public class LoginScriptTeenpatti : MonoBehaviour
{
    public InputField email;
    public InputField password;
    public GameObject loading;
    // public GameObject agreeBtn;
    public GameObject LoginBtn;
    public GameObject rembrBtn;
    public GameObject resetPwd;
    private GameObject PopUpContainer;

    private bool isSelected = false;
    private bool rememberSelected = false;


    // Use this for initialization
    void Start()
    {
        loading.SetActive(false);
        resetPwd.SetActive(false);

        rememberSelected = false;

        PopUpContainer = Instantiate((GameObject)Resources.Load("_prefeb/ShowMsgPanel"));
        PopUpContainer.transform.SetParent(transform);
        PopUpContainer.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        PopUpContainer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
       // PopUpContainer.GetComponent<PopUp>().msg.text = "Say Hello";
        PopUpContainer.SetActive(false);
        
        // LoginBtn.GetComponent<Button>().interactable = false;
        isSelected = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (loading.activeSelf)
        {
            loading.transform.Rotate(Vector3.back, 3, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync("FirstSceneTeenpatti");
        }
    }





   





    public void CreateAccount()
    {
        SceneManager.LoadSceneAsync("Create");
    }

    public void LoginAction()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            Debug.Log(" interconnectivity found");
            if (validateField())
            {


                WWWForm form = new WWWForm();
                form.AddField("TAG", "LOGIN");
                form.AddField("mobile", email.text);
                form.AddField("password", password.text);
                UnityWebRequest w = UnityWebRequest.Post(TagsTeenpatti.URL, form);
                Debug.Log(" interconnectivity found");
                StartCoroutine(ServerRequest(w));
                loading.SetActive(true);
            }
            else

            {
                Debug.Log("Validation failed.");
            }
        }
        else
        {
            PopUpContainer.GetComponent<PopUp>().msg.GetComponent<Text>().text = "Internet connection not found. Please try again.";
            PopUpContainer.SetActive(true);
        }

    } 

    



    private IEnumerator ServerRequest(UnityWebRequest www)
    {
        yield return www.SendWebRequest();
        Debug.Log("resp " + www.downloadHandler.text);
        Debug.Log("resp1 " + www.isHttpError);
        Debug.Log("resp2 " + www.isNetworkError);
        Debug.Log("resp3 " + www.error);
        if (www.error == null)
        {
            try{
            string response = www.downloadHandler.text;
            JSONNode node = JSON.Parse(response);

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
                        string name = data["name"];
                        string mobile = data["mobile"];
                        string coin = data["balance"];
                        string parent_id = data["parent_id"];
                        string image = data["picture"];
                        string password = data["password"];

                        PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_ID, "" + my_Id);
                        PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_NAME, "" + name);
                        PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_MOBILE, "" + mobile);
                        PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_COIN, "" + coin);
                        PlayerPrefs.SetString(GetPlayerDetailsTags.PARENT_ID, "" + parent_id);
                        PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_IMAGE, "" + image);
                        PlayerPrefs.SetString(GetPlayerDetailsTags.PLAYER_PASSWORD, password);

                        SceneManager.LoadSceneAsync("MainLobby");
                        Debug.Log("PlayerDetails.Password");

                        Debug.Log("my_id " + my_Id + " name " + name +"  mobile " + mobile + " balance " + coin + " parent_id " + parent_id + " picture " + image + " password " + password);

                    }
                    catch (System.Exception ex)
                    {
                        loading.SetActive(false);
                        Debug.Log("Natasha Exception " + ex.Message);
                    }
                   

                    // call the next scene
                   

                }
                else
                {
                    //loading.SetActive (false);
                    GameControllerTeenPatti.showToast(node["message"]);

                    loading.SetActive(false);
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
            GameController.showToast ("response " + www.error.ToString ());
        }

    }

    public void ForgetPwd()
    {
        resetPwd.SetActive(true);
    }
    public bool validateField()
    {

        if (email.text.Length == 0)
        {
            PopUpContainer.SetActive(true);
            PopUpContainer.GetComponent<PopUp>().msg.text = "Please enter your mobile number";
           
            return false;
        }


        if (password.text.Length == 0)
        {
            PopUpContainer.GetComponent<PopUp>().msg.text = "Please enter your password.";
            PopUpContainer.SetActive(true);
            return false;

        }
        return true;
    }


}


