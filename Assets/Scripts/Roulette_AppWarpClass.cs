using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading;
using System;
using System.Collections.Generic;




using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.listener;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client.message;
using com.shephertz.app42.gaming.multiplayer.client.transformer;


using SimpleJSON;
using UnityEngine.UI;
namespace Roullet
{


    public class Roulette_AppWarpClass : MonoBehaviour
    {

        // connect to local server

        //
        			//private string appKey = "e2383b63-fdc1-4ec5-a";
        			//private string ipAddress = "192.168.1.4";
        			//private int port = 12346;
        //

        //
        private string appKey = "3684e136-a310-4395-9";
        private string ipAddress = "132.148.25.71";
        private int port = 12344;





        // connect to global server
        //	private string appKey = "c84e6f67-1de4-491d-a";
        //	private string ipAddress = "208.109.234.119";
        //	private  int port = 12347;

        public static string roomID = "";
        public static string username = "Lalit";
        public int sessionID = 0;

        public bool useUDP = true;
        public Thread serverThread;
        public static int state = 0;
        protected bool sendData = false;
        public static bool InternetConnectivity = true;
        protected string data = "";
        protected Queue sendDataQueue;
        Roulette_ListnerClass listen;
        public static int attempt_server = 0;

        public GameObject QuitGamePanel;

        void Start()
        {
            Screen.orientation = ScreenOrientation.Landscape;
            Application.targetFrameRate = 30;
           // QuitGamePanel = Instantiate((GameObject)Resources.Load("_prefeb/QuitGame"));
           // QuitGamePanel.transform.SetParent(transform);
           // QuitGamePanel.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
          //  QuitGamePanel.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //QuitGamePanel.GetComponent<ErrorPanelScript> ().msgtext.GetComponent<Text> ().text = "Say Hello";
          //  QuitGamePanel.SetActive(false);

            try
            {
                if (PlayerPrefs.GetInt("Sound") == 0)
                {
                    GetComponent<AudioSource>().Stop();
                }

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }

            Connection();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                QuitGamePanel.GetComponent<QuitGame>().msgText.GetComponent<Text>().text = "Are you sure you want to quit the game. You may also lose coins?";
                QuitGamePanel.SetActive(true);
            }

        }

        public void HomeAction()
        {

            QuitGamePanel.GetComponent<QuitGame>().msgText.GetComponent<Text>().text = "Are you sure you want to quit the game. You may also lose coins?";
            QuitGamePanel.SetActive(true);
        }




        public static void add_coin(JSONClass data)
        {
            if (PlayerPrefs.GetInt(PlayerDetails.RealMoney) == 1)
            {
                Debug.Log(data.ToString());
                Roulette_AppWarpClass.sendChat(data.ToString());
            }
        }


        static void sendChat(string msg)
        {
            try
            {
                if (WarpClient.GetInstance() != null)
                {
                    if (!msg.Equals(""))
                    {
                        WarpClient.GetInstance().SendChat(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                print(ex);
            }
        }



        public void Connection()
        {

            string player_name = PlayerPrefs.GetString(PlayerDetails.ConnectionId);
            Debug.Log("player_name " + player_name);
            //player_name = "Ram";
            WarpClient.initialize(appKey, ipAddress, port);
            WarpClient.setRecoveryAllowance(100);
            listen = new Roulette_ListnerClass();
            WarpClient.GetInstance().AddConnectionRequestListener(listen);
            WarpClient.GetInstance().AddChatRequestListener(listen);
            WarpClient.GetInstance().AddLobbyRequestListener(listen);
            WarpClient.GetInstance().AddNotificationListener(listen);
            WarpClient.GetInstance().AddRoomRequestListener(listen);
            WarpClient.GetInstance().AddUpdateRequestListener(listen);
            WarpClient.GetInstance().AddZoneRequestListener(listen);
            WarpClient.GetInstance().Connect(player_name, "");

        }

        public static void Disconnect()
        {
            if (WarpClient.GetInstance() != null)
            {
                WarpClient.GetInstance().Disconnect();

            }
        }
    }
}
