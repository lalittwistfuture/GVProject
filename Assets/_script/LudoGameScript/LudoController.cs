using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJSON;
namespace Ludo
{

    public class LudoController : MonoBehaviour
    {
        public GameObject colorSelectionPanel;
        public GameObject winnerPanel;
        public GameObject lossPanel;
        public GameObject loading;
        public GameObject privateInfo;
        public GameObject messagePanel;
        int myColor = 0;
        public GameObject quitGamePanel;
        public static bool isGameStart = false;
        public GameObject chatButton;
        public  GameObject smileyButton;
        public GameObject smileyPanel;
        public GameObject Loader;
        public GameObject LoadingText;
        void Start()
        {

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            try
            {
               // loading = transform.Find("LoadingGamePanel").gameObject;
                LoadingText.SetActive(false);
                //chatButton = GameObject.FindGameObjectWithTag("ChatButton");
               // smileyButton = GameObject.FindGameObjectWithTag("SmileyButton");
               
            } catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
            try
            {
               // quitGamePanel = transform.Find("QuitGamePanel").gameObject;
              //  privateInfo = transform.Find("PrivateTablePage").gameObject;
                quitGamePanel.SetActive(false);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }

            try
            {
                
               // messagePanel = transform.Find("MessagePanel").gameObject;
               // smileyPanel = transform.Find("SmileyPanel").gameObject;
                messagePanel.SetActive(false);
                smileyPanel.SetActive(false);



            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }

            try
            {
                
               // winnerPanel = transform.Find("WinnerPanel").gameObject;
               // lossPanel = transform.Find("LossPanel").gameObject;
               // colorSelectionPanel = transform.Find("SelectColorPanel").gameObject;
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }

            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) != LudoTags.OFFLINE)
            {
                LudoDelegate.startGame();
            }

            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) != LudoTags.OFFLINE)
            {
                chatButton.SetActive(true);
                smileyButton.SetActive(true);
            }
            else
            {
                chatButton.SetActive(false);
                smileyButton.SetActive(false);
            }
        }

        public void showSmiley()
        {
            if (smileyPanel.activeSelf)
            {
                smileyPanel.SetActive(false);
            }
            else
            {
                smileyPanel.SetActive(true);
            }
        }

       public void showMessage()
        {
            if (messagePanel.activeSelf)
            {
                messagePanel.SetActive(false);
            }
            else
            {
                messagePanel.SetActive(true);
            }
        }



		private void FixedUpdate()
		{
            if(Loader.activeSelf){
                Loader.transform.Rotate(Vector3.back, 5, Space.World);
            }
		}

		void Update()
        {

        }

        void onReceivedServerMessage(string sender, string message)
        {
            try{
            JSONNode s = JSON.Parse(message);
            switch (s[ServerTags.TAG])
            {

                case ServerTags.START_DEAL:
                    {
                        colorSelectionPanel.SetActive(false);
                      
                    }

                    break;
               
            }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }



        public void StartGame()
        {
            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) != LudoTags.OFFLINE)
            {
                if (this.myColor > 0)
                {
                    appwarp.joinGame(this.myColor);

                    LoadingText.SetActive(true);

                    if (PlayerPrefs.GetInt(LudoTags.ROOM_TYPE) == LudoTags.PUBLIC)
                    {
                        //appwarp.ServerMessage = "Waiting for Opponent";
                        //loading.GetComponent<LoadingPanel>().startTimer(10, "No Opponent found. Try after some time.");
                    }
                    else
                    {
                        colorSelectionPanel.SetActive(false);
                        //loading.SetActive(true);
                        // appwarp.ServerMessage = "Creating Table";
                        //loading.GetComponent<LoadingPanel>().startTimer(15, "Unable to create private table. Please try again later.");
                    }
                }
            }
            else
            {
                if (this.myColor == 0)
                {
                    colorSelectionPanel.transform.Find("Box").GetComponent<ColorSelection>().defaultColor();
                }
                LudoDelegate.startGame();
                colorSelectionPanel.SetActive(false);
            }


        }


        void onSelectColorDone(Color color, string playerTag, int colorIndex)
        {
            if (playerTag.Equals("Player1"))
            {
                this.myColor = colorIndex;

                StartGame();
            }

        }


        public void backAction()
        {
            if (PlayerPrefs.GetInt(LudoTags.GAME_TYPE) != LudoTags.OFFLINE)
            {
                if (lossPanel.activeSelf)
                {
                    appwarp.Disconnect();
                    SceneManager.LoadSceneAsync("LobiScene");
                }
                else if (winnerPanel.activeSelf)
                {
                    appwarp.Disconnect();
                    SceneManager.LoadSceneAsync("LobiScene");
                }
                else if (loading.activeSelf)
                {
                    appwarp.Disconnect();
                    SceneManager.LoadSceneAsync("LobiScene");

                }
                else if (privateInfo.activeSelf)
                {
                    appwarp.Disconnect();
                    SceneManager.LoadSceneAsync("LobiScene");

                }
                else if (colorSelectionPanel.activeSelf)
                {
                    appwarp.Disconnect();
                    SceneManager.LoadSceneAsync("LobiScene");

                }
                else
                {

                    quitGamePanel.SetActive(true);
                }

            }
            else
            {
                if (winnerPanel.activeSelf)
                {
                    SceneManager.LoadSceneAsync("LobiScene");

                }else if(colorSelectionPanel.activeSelf){
                    
                    SceneManager.LoadSceneAsync("LobiScene");
                }
                else
                {
                    quitGamePanel.SetActive(true);

                }

            }


        }


        private void OnEnable()
        {
            LudoDelegate.onSelectColorDone += onSelectColorDone;
            LudoDelegate.onReceivedServerMessage += onReceivedServerMessage;

           
        }

        private void OnDisable()
        {
            LudoDelegate.onSelectColorDone -= onSelectColorDone;
            LudoDelegate.onReceivedServerMessage -= onReceivedServerMessage;
        }



    }

}
